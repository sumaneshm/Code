// Type: System.Linq.Parallel.SortHelper`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class SortHelper<TInputOutput, TKey> : SortHelper<TInputOutput>, IDisposable
  {
    private QueryOperatorEnumerator<TInputOutput, TKey> m_source;
    private int m_partitionCount;
    private int m_partitionIndex;
    private QueryTaskGroupState m_groupState;
    private int[][] m_sharedIndices;
    private GrowingArray<TKey>[] m_sharedKeys;
    private TInputOutput[][] m_sharedValues;
    private Barrier[,] m_sharedBarriers;
    private OrdinalIndexState m_indexState;
    private IComparer<TKey> m_keyComparer;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private SortHelper(QueryOperatorEnumerator<TInputOutput, TKey> source, int partitionCount, int partitionIndex, QueryTaskGroupState groupState, int[][] sharedIndices, OrdinalIndexState indexState, IComparer<TKey> keyComparer, GrowingArray<TKey>[] sharedkeys, TInputOutput[][] sharedValues, Barrier[,] sharedBarriers)
    {
      this.m_source = source;
      this.m_partitionCount = partitionCount;
      this.m_partitionIndex = partitionIndex;
      this.m_groupState = groupState;
      this.m_sharedIndices = sharedIndices;
      this.m_indexState = indexState;
      this.m_keyComparer = keyComparer;
      this.m_sharedKeys = sharedkeys;
      this.m_sharedValues = sharedValues;
      this.m_sharedBarriers = sharedBarriers;
    }

    internal static SortHelper<TInputOutput, TKey>[] GenerateSortHelpers(PartitionedStream<TInputOutput, TKey> partitions, QueryTaskGroupState groupState)
    {
      int partitionCount = partitions.PartitionCount;
      SortHelper<TInputOutput, TKey>[] sortHelperArray = new SortHelper<TInputOutput, TKey>[partitionCount];
      int num1 = 1;
      int length = 0;
      while (num1 < partitionCount)
      {
        ++length;
        num1 <<= 1;
      }
      int[][] sharedIndices = new int[partitionCount][];
      GrowingArray<TKey>[] sharedkeys = new GrowingArray<TKey>[partitionCount];
      TInputOutput[][] sharedValues = new TInputOutput[partitionCount][];
      Barrier[,] sharedBarriers = new Barrier[length, partitionCount];
      if (partitionCount > 1)
      {
        int num2 = 1;
        for (int index1 = 0; index1 < sharedBarriers.GetLength(0); ++index1)
        {
          for (int index2 = 0; index2 < sharedBarriers.GetLength(1); ++index2)
          {
            if (index2 % num2 == 0)
              sharedBarriers[index1, index2] = new Barrier(2);
          }
          num2 *= 2;
        }
      }
      for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
        sortHelperArray[partitionIndex] = new SortHelper<TInputOutput, TKey>(partitions[partitionIndex], partitionCount, partitionIndex, groupState, sharedIndices, partitions.OrdinalIndexState, partitions.KeyComparer, sharedkeys, sharedValues, sharedBarriers);
      return sortHelperArray;
    }

    public void Dispose()
    {
      if (this.m_partitionIndex != 0)
        return;
      for (int index1 = 0; index1 < this.m_sharedBarriers.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.m_sharedBarriers.GetLength(1); ++index2)
        {
          Barrier barrier = this.m_sharedBarriers[index1, index2];
          if (barrier != null)
            barrier.Dispose();
        }
      }
    }

    internal override TInputOutput[] Sort()
    {
      GrowingArray<TKey> keys = (GrowingArray<TKey>) null;
      List<TInputOutput> values = (List<TInputOutput>) null;
      this.BuildKeysFromSource(ref keys, ref values);
      this.QuickSortIndicesInPlace(keys, values, this.m_indexState);
      if (this.m_partitionCount > 1)
        this.MergeSortCooperatively();
      return this.m_sharedValues[this.m_partitionIndex];
    }

    private void BuildKeysFromSource(ref GrowingArray<TKey> keys, ref List<TInputOutput> values)
    {
      values = new List<TInputOutput>();
      CancellationToken cancellationToken = this.m_groupState.CancellationState.MergedCancellationToken;
      try
      {
        TInputOutput currentElement = default (TInputOutput);
        TKey currentKey = default (TKey);
        bool flag = this.m_source.MoveNext(ref currentElement, ref currentKey);
        if (keys == null)
          keys = new GrowingArray<TKey>();
        if (!flag)
          return;
        int num = 0;
        do
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(cancellationToken);
          keys.Add(currentKey);
          values.Add(currentElement);
        }
        while (this.m_source.MoveNext(ref currentElement, ref currentKey));
      }
      finally
      {
        this.m_source.Dispose();
      }
    }

    private void QuickSortIndicesInPlace(GrowingArray<TKey> keys, List<TInputOutput> values, OrdinalIndexState ordinalIndexState)
    {
      int[] indices = new int[values.Count];
      for (int index = 0; index < indices.Length; ++index)
        indices[index] = index;
      if (indices.Length > 1 && ExchangeUtilities.IsWorseThan(ordinalIndexState, OrdinalIndexState.Increasing))
        this.QuickSort(0, indices.Length - 1, keys.InternalArray, indices, this.m_groupState.CancellationState.MergedCancellationToken);
      if (this.m_partitionCount == 1)
      {
        TInputOutput[] inputOutputArray = new TInputOutput[values.Count];
        for (int index = 0; index < indices.Length; ++index)
          inputOutputArray[index] = values[indices[index]];
        this.m_sharedValues[this.m_partitionIndex] = inputOutputArray;
      }
      else
      {
        this.m_sharedIndices[this.m_partitionIndex] = indices;
        this.m_sharedKeys[this.m_partitionIndex] = keys;
        this.m_sharedValues[this.m_partitionIndex] = new TInputOutput[values.Count];
        values.CopyTo(this.m_sharedValues[this.m_partitionIndex]);
      }
    }

    private void MergeSortCooperatively()
    {
      CancellationToken cancellationToken = this.m_groupState.CancellationState.MergedCancellationToken;
      int length1 = this.m_sharedBarriers.GetLength(0);
      for (int phase = 0; phase < length1; ++phase)
      {
        bool flag = phase == length1 - 1;
        int partnerIndex = this.ComputePartnerIndex(phase);
        if (partnerIndex < this.m_partitionCount)
        {
          int[] numArray1 = this.m_sharedIndices[this.m_partitionIndex];
          GrowingArray<TKey> growingArray1 = this.m_sharedKeys[this.m_partitionIndex];
          TKey[] internalArray1 = growingArray1.InternalArray;
          TInputOutput[] inputOutputArray1 = this.m_sharedValues[this.m_partitionIndex];
          this.m_sharedBarriers[phase, Math.Min(this.m_partitionIndex, partnerIndex)].SignalAndWait(cancellationToken);
          if (this.m_partitionIndex < partnerIndex)
          {
            int[] numArray2 = this.m_sharedIndices[partnerIndex];
            TKey[] internalArray2 = this.m_sharedKeys[partnerIndex].InternalArray;
            TInputOutput[] inputOutputArray2 = this.m_sharedValues[partnerIndex];
            this.m_sharedIndices[partnerIndex] = numArray1;
            this.m_sharedKeys[partnerIndex] = growingArray1;
            this.m_sharedValues[partnerIndex] = inputOutputArray1;
            int length2 = inputOutputArray1.Length;
            int length3 = inputOutputArray2.Length;
            int length4 = length2 + length3;
            int[] numArray3 = (int[]) null;
            TInputOutput[] inputOutputArray3 = new TInputOutput[length4];
            if (!flag)
              numArray3 = new int[length4];
            this.m_sharedIndices[this.m_partitionIndex] = numArray3;
            this.m_sharedKeys[this.m_partitionIndex] = growingArray1;
            this.m_sharedValues[this.m_partitionIndex] = inputOutputArray3;
            this.m_sharedBarriers[phase, this.m_partitionIndex].SignalAndWait(cancellationToken);
            int num = (length4 + 1) / 2;
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;
            for (; index1 < num; ++index1)
            {
              if ((index1 & 63) == 0)
                CancellationState.ThrowIfCanceled(cancellationToken);
              if (index2 < length2 && (index3 >= length3 || this.m_keyComparer.Compare(internalArray1[numArray1[index2]], internalArray2[numArray2[index3]]) <= 0))
              {
                if (flag)
                  inputOutputArray3[index1] = inputOutputArray1[numArray1[index2]];
                else
                  numArray3[index1] = numArray1[index2];
                ++index2;
              }
              else
              {
                if (flag)
                  inputOutputArray3[index1] = inputOutputArray2[numArray2[index3]];
                else
                  numArray3[index1] = length2 + numArray2[index3];
                ++index3;
              }
            }
            if (!flag && length2 > 0)
              Array.Copy((Array) inputOutputArray1, 0, (Array) inputOutputArray3, 0, length2);
            this.m_sharedBarriers[phase, this.m_partitionIndex].SignalAndWait(cancellationToken);
          }
          else
          {
            this.m_sharedBarriers[phase, partnerIndex].SignalAndWait(cancellationToken);
            int[] numArray2 = this.m_sharedIndices[this.m_partitionIndex];
            TKey[] internalArray2 = this.m_sharedKeys[this.m_partitionIndex].InternalArray;
            TInputOutput[] inputOutputArray2 = this.m_sharedValues[this.m_partitionIndex];
            int[] numArray3 = this.m_sharedIndices[partnerIndex];
            GrowingArray<TKey> growingArray2 = this.m_sharedKeys[partnerIndex];
            TInputOutput[] inputOutputArray3 = this.m_sharedValues[partnerIndex];
            int length2 = inputOutputArray2.Length;
            int length3 = inputOutputArray1.Length;
            int num1 = length2 + length3;
            int num2 = (num1 + 1) / 2;
            int index1 = num1 - 1;
            int index2 = length2 - 1;
            int index3 = length3 - 1;
            for (; index1 >= num2; --index1)
            {
              if ((index1 & 63) == 0)
                CancellationState.ThrowIfCanceled(cancellationToken);
              if (index2 >= 0 && (index3 < 0 || this.m_keyComparer.Compare(internalArray2[numArray2[index2]], internalArray1[numArray1[index3]]) > 0))
              {
                if (flag)
                  inputOutputArray3[index1] = inputOutputArray2[numArray2[index2]];
                else
                  numArray3[index1] = numArray2[index2];
                --index2;
              }
              else
              {
                if (flag)
                  inputOutputArray3[index1] = inputOutputArray1[numArray1[index3]];
                else
                  numArray3[index1] = length2 + numArray1[index3];
                --index3;
              }
            }
            if (!flag && inputOutputArray1.Length > 0)
            {
              growingArray2.CopyFrom(internalArray1, inputOutputArray1.Length);
              Array.Copy((Array) inputOutputArray1, 0, (Array) inputOutputArray3, length2, inputOutputArray1.Length);
            }
            this.m_sharedBarriers[phase, partnerIndex].SignalAndWait(cancellationToken);
            break;
          }
        }
      }
    }

    private int ComputePartnerIndex(int phase)
    {
      int num = 1 << phase;
      return this.m_partitionIndex + (this.m_partitionIndex % (num * 2) == 0 ? num : -num);
    }

    private void QuickSort(int left, int right, TKey[] keys, int[] indices, CancellationToken cancelToken)
    {
      if (right - left > 63)
        CancellationState.ThrowIfCanceled(cancelToken);
      do
      {
        int left1 = left;
        int right1 = right;
        int index = indices[left1 + (right1 - left1 >> 1)];
        TKey y = keys[index];
        do
        {
          while (this.m_keyComparer.Compare(keys[indices[left1]], y) < 0)
            ++left1;
          while (this.m_keyComparer.Compare(keys[indices[right1]], y) > 0)
            --right1;
          if (left1 <= right1)
          {
            if (left1 < right1)
            {
              int num = indices[left1];
              indices[left1] = indices[right1];
              indices[right1] = num;
            }
            ++left1;
            --right1;
          }
          else
            break;
        }
        while (left1 <= right1);
        if (right1 - left <= right - left1)
        {
          if (left < right1)
            this.QuickSort(left, right1, keys, indices, cancelToken);
          left = left1;
        }
        else
        {
          if (left1 < right)
            this.QuickSort(left1, right, keys, indices, cancelToken);
          right = right1;
        }
      }
      while (left < right);
    }
  }
}
