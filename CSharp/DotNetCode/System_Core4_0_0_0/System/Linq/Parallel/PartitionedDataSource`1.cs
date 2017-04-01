// Type: System.Linq.Parallel.PartitionedDataSource`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class PartitionedDataSource<T> : PartitionedStream<T, int>
  {
    internal PartitionedDataSource(IEnumerable<T> source, int partitionCount, bool useStriping)
      : base(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), source is IList<T> ? OrdinalIndexState.Indexible : OrdinalIndexState.Correct)
    {
      this.InitializePartitions(source, partitionCount, useStriping);
    }

    private void InitializePartitions(IEnumerable<T> source, int partitionCount, bool useStriping)
    {
      ParallelEnumerableWrapper<T> enumerableWrapper = source as ParallelEnumerableWrapper<T>;
      if (enumerableWrapper != null)
        source = enumerableWrapper.WrappedEnumerable;
      IList<T> data1 = source as IList<T>;
      if (data1 != null)
      {
        QueryOperatorEnumerator<T, int>[] operatorEnumeratorArray = new QueryOperatorEnumerator<T, int>[partitionCount];
        int count = data1.Count;
        T[] data2 = source as T[];
        int maxChunkSize = -1;
        if (useStriping)
        {
          maxChunkSize = Scheduling.GetDefaultChunkSize<T>();
          if (maxChunkSize < 1)
            maxChunkSize = 1;
        }
        for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
          operatorEnumeratorArray[partitionIndex] = data2 == null ? (!useStriping ? (QueryOperatorEnumerator<T, int>) new PartitionedDataSource<T>.ListContiguousIndexRangeEnumerator(data1, partitionCount, partitionIndex) : (QueryOperatorEnumerator<T, int>) new PartitionedDataSource<T>.ListIndexRangeEnumerator(data1, partitionCount, partitionIndex, maxChunkSize)) : (!useStriping ? (QueryOperatorEnumerator<T, int>) new PartitionedDataSource<T>.ArrayContiguousIndexRangeEnumerator(data2, partitionCount, partitionIndex) : (QueryOperatorEnumerator<T, int>) new PartitionedDataSource<T>.ArrayIndexRangeEnumerator(data2, partitionCount, partitionIndex, maxChunkSize));
        this.m_partitions = operatorEnumeratorArray;
      }
      else
        this.m_partitions = PartitionedDataSource<T>.MakePartitions(source.GetEnumerator(), partitionCount);
    }

    private static QueryOperatorEnumerator<T, int>[] MakePartitions(IEnumerator<T> source, int partitionCount)
    {
      QueryOperatorEnumerator<T, int>[] operatorEnumeratorArray = new QueryOperatorEnumerator<T, int>[partitionCount];
      object sourceSyncLock = new object();
      Shared<int> currentIndex = new Shared<int>(0);
      Shared<int> degreeOfParallelism = new Shared<int>(partitionCount);
      Shared<bool> exceptionTracker = new Shared<bool>(false);
      for (int index = 0; index < partitionCount; ++index)
        operatorEnumeratorArray[index] = (QueryOperatorEnumerator<T, int>) new PartitionedDataSource<T>.ContiguousChunkLazyEnumerator(source, exceptionTracker, sourceSyncLock, currentIndex, degreeOfParallelism);
      return operatorEnumeratorArray;
    }

    internal sealed class ArrayIndexRangeEnumerator : QueryOperatorEnumerator<T, int>
    {
      private readonly T[] m_data;
      private readonly int m_elementCount;
      private readonly int m_partitionCount;
      private readonly int m_partitionIndex;
      private readonly int m_maxChunkSize;
      private readonly int m_sectionCount;
      private PartitionedDataSource<T>.ArrayIndexRangeEnumerator.Mutables m_mutables;

      internal ArrayIndexRangeEnumerator(T[] data, int partitionCount, int partitionIndex, int maxChunkSize)
      {
        this.m_data = data;
        this.m_elementCount = data.Length;
        this.m_partitionCount = partitionCount;
        this.m_partitionIndex = partitionIndex;
        this.m_maxChunkSize = maxChunkSize;
        int num = maxChunkSize * partitionCount;
        this.m_sectionCount = this.m_elementCount / num + (this.m_elementCount % num == 0 ? 0 : 1);
      }

      internal override bool MoveNext(ref T currentElement, ref int currentKey)
      {
        PartitionedDataSource<T>.ArrayIndexRangeEnumerator.Mutables mutables = this.m_mutables ?? (this.m_mutables = new PartitionedDataSource<T>.ArrayIndexRangeEnumerator.Mutables());
        if (++mutables.m_currentPositionInChunk >= mutables.m_currentChunkSize && !this.MoveNextSlowPath())
          return false;
        currentKey = mutables.m_currentChunkOffset + mutables.m_currentPositionInChunk;
        currentElement = this.m_data[currentKey];
        return true;
      }

      private bool MoveNextSlowPath()
      {
        PartitionedDataSource<T>.ArrayIndexRangeEnumerator.Mutables mutables = this.m_mutables;
        int num1 = ++mutables.m_currentSection;
        int num2 = this.m_sectionCount - num1;
        if (num2 <= 0)
          return false;
        int num3 = num1 * this.m_partitionCount * this.m_maxChunkSize;
        mutables.m_currentPositionInChunk = 0;
        if (num2 > 1)
        {
          mutables.m_currentChunkSize = this.m_maxChunkSize;
          mutables.m_currentChunkOffset = num3 + this.m_partitionIndex * this.m_maxChunkSize;
        }
        else
        {
          int num4 = this.m_elementCount - num3;
          int num5 = num4 / this.m_partitionCount;
          int num6 = num4 % this.m_partitionCount;
          mutables.m_currentChunkSize = num5;
          if (this.m_partitionIndex < num6)
            ++mutables.m_currentChunkSize;
          if (mutables.m_currentChunkSize == 0)
            return false;
          mutables.m_currentChunkOffset = num3 + this.m_partitionIndex * num5 + (this.m_partitionIndex < num6 ? this.m_partitionIndex : num6);
        }
        return true;
      }

      private class Mutables
      {
        internal int m_currentSection;
        internal int m_currentChunkSize;
        internal int m_currentPositionInChunk;
        internal int m_currentChunkOffset;

        internal Mutables()
        {
          this.m_currentSection = -1;
        }
      }
    }

    internal sealed class ArrayContiguousIndexRangeEnumerator : QueryOperatorEnumerator<T, int>
    {
      private readonly T[] m_data;
      private readonly int m_startIndex;
      private readonly int m_maximumIndex;
      private Shared<int> m_currentIndex;

      internal ArrayContiguousIndexRangeEnumerator(T[] data, int partitionCount, int partitionIndex)
      {
        this.m_data = data;
        int num1 = data.Length / partitionCount;
        int num2 = data.Length % partitionCount;
        int num3 = partitionIndex * num1 + (partitionIndex < num2 ? partitionIndex : num2);
        this.m_startIndex = num3 - 1;
        this.m_maximumIndex = num3 + num1 + (partitionIndex < num2 ? 1 : 0);
      }

      internal override bool MoveNext(ref T currentElement, ref int currentKey)
      {
        if (this.m_currentIndex == null)
          this.m_currentIndex = new Shared<int>(this.m_startIndex);
        int index = ++this.m_currentIndex.Value;
        if (index >= this.m_maximumIndex)
          return false;
        currentKey = index;
        currentElement = this.m_data[index];
        return true;
      }
    }

    internal sealed class ListIndexRangeEnumerator : QueryOperatorEnumerator<T, int>
    {
      private readonly IList<T> m_data;
      private readonly int m_elementCount;
      private readonly int m_partitionCount;
      private readonly int m_partitionIndex;
      private readonly int m_maxChunkSize;
      private readonly int m_sectionCount;
      private PartitionedDataSource<T>.ListIndexRangeEnumerator.Mutables m_mutables;

      internal ListIndexRangeEnumerator(IList<T> data, int partitionCount, int partitionIndex, int maxChunkSize)
      {
        this.m_data = data;
        this.m_elementCount = data.Count;
        this.m_partitionCount = partitionCount;
        this.m_partitionIndex = partitionIndex;
        this.m_maxChunkSize = maxChunkSize;
        int num = maxChunkSize * partitionCount;
        this.m_sectionCount = this.m_elementCount / num + (this.m_elementCount % num == 0 ? 0 : 1);
      }

      internal override bool MoveNext(ref T currentElement, ref int currentKey)
      {
        PartitionedDataSource<T>.ListIndexRangeEnumerator.Mutables mutables = this.m_mutables ?? (this.m_mutables = new PartitionedDataSource<T>.ListIndexRangeEnumerator.Mutables());
        if (++mutables.m_currentPositionInChunk >= mutables.m_currentChunkSize && !this.MoveNextSlowPath())
          return false;
        currentKey = mutables.m_currentChunkOffset + mutables.m_currentPositionInChunk;
        currentElement = this.m_data[currentKey];
        return true;
      }

      private bool MoveNextSlowPath()
      {
        PartitionedDataSource<T>.ListIndexRangeEnumerator.Mutables mutables = this.m_mutables;
        int num1 = ++mutables.m_currentSection;
        int num2 = this.m_sectionCount - num1;
        if (num2 <= 0)
          return false;
        int num3 = num1 * this.m_partitionCount * this.m_maxChunkSize;
        mutables.m_currentPositionInChunk = 0;
        if (num2 > 1)
        {
          mutables.m_currentChunkSize = this.m_maxChunkSize;
          mutables.m_currentChunkOffset = num3 + this.m_partitionIndex * this.m_maxChunkSize;
        }
        else
        {
          int num4 = this.m_elementCount - num3;
          int num5 = num4 / this.m_partitionCount;
          int num6 = num4 % this.m_partitionCount;
          mutables.m_currentChunkSize = num5;
          if (this.m_partitionIndex < num6)
            ++mutables.m_currentChunkSize;
          if (mutables.m_currentChunkSize == 0)
            return false;
          mutables.m_currentChunkOffset = num3 + this.m_partitionIndex * num5 + (this.m_partitionIndex < num6 ? this.m_partitionIndex : num6);
        }
        return true;
      }

      private class Mutables
      {
        internal int m_currentSection;
        internal int m_currentChunkSize;
        internal int m_currentPositionInChunk;
        internal int m_currentChunkOffset;

        internal Mutables()
        {
          this.m_currentSection = -1;
        }
      }
    }

    internal sealed class ListContiguousIndexRangeEnumerator : QueryOperatorEnumerator<T, int>
    {
      private readonly IList<T> m_data;
      private readonly int m_startIndex;
      private readonly int m_maximumIndex;
      private Shared<int> m_currentIndex;

      internal ListContiguousIndexRangeEnumerator(IList<T> data, int partitionCount, int partitionIndex)
      {
        this.m_data = data;
        int num1 = data.Count / partitionCount;
        int num2 = data.Count % partitionCount;
        int num3 = partitionIndex * num1 + (partitionIndex < num2 ? partitionIndex : num2);
        this.m_startIndex = num3 - 1;
        this.m_maximumIndex = num3 + num1 + (partitionIndex < num2 ? 1 : 0);
      }

      internal override bool MoveNext(ref T currentElement, ref int currentKey)
      {
        if (this.m_currentIndex == null)
          this.m_currentIndex = new Shared<int>(this.m_startIndex);
        int index = ++this.m_currentIndex.Value;
        if (index >= this.m_maximumIndex)
          return false;
        currentKey = index;
        currentElement = this.m_data[index];
        return true;
      }
    }

    private class ContiguousChunkLazyEnumerator : QueryOperatorEnumerator<T, int>
    {
      private readonly IEnumerator<T> m_source;
      private readonly object m_sourceSyncLock;
      private readonly Shared<int> m_currentIndex;
      private readonly Shared<int> m_activeEnumeratorsCount;
      private readonly Shared<bool> m_exceptionTracker;
      private PartitionedDataSource<T>.ContiguousChunkLazyEnumerator.Mutables m_mutables;
      private const int chunksPerChunkSize = 7;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ContiguousChunkLazyEnumerator(IEnumerator<T> source, Shared<bool> exceptionTracker, object sourceSyncLock, Shared<int> currentIndex, Shared<int> degreeOfParallelism)
      {
        this.m_source = source;
        this.m_sourceSyncLock = sourceSyncLock;
        this.m_currentIndex = currentIndex;
        this.m_activeEnumeratorsCount = degreeOfParallelism;
        this.m_exceptionTracker = exceptionTracker;
      }

      internal override bool MoveNext(ref T currentElement, ref int currentKey)
      {
        PartitionedDataSource<T>.ContiguousChunkLazyEnumerator.Mutables mutables = this.m_mutables ?? (this.m_mutables = new PartitionedDataSource<T>.ContiguousChunkLazyEnumerator.Mutables());
        T[] objArray;
        int index;
        while (true)
        {
          objArray = mutables.m_chunkBuffer;
          index = ++mutables.m_currentChunkIndex;
          if (index >= mutables.m_currentChunkSize)
          {
            lock (this.m_sourceSyncLock)
            {
              int local_3 = 0;
              if (this.m_exceptionTracker.Value)
                return false;
              try
              {
                for (; local_3 < mutables.m_nextChunkMaxSize; ++local_3)
                {
                  if (this.m_source.MoveNext())
                    objArray[local_3] = this.m_source.Current;
                  else
                    break;
                }
              }
              catch
              {
                this.m_exceptionTracker.Value = true;
                throw;
              }
              mutables.m_currentChunkSize = local_3;
              if (local_3 == 0)
                return false;
              mutables.m_chunkBaseIndex = this.m_currentIndex.Value;
              checked { this.m_currentIndex.Value += local_3; }
            }
            if (mutables.m_nextChunkMaxSize < objArray.Length && (mutables.m_chunkCounter++ & 7) == 7)
            {
              mutables.m_nextChunkMaxSize = mutables.m_nextChunkMaxSize * 2;
              if (mutables.m_nextChunkMaxSize > objArray.Length)
                mutables.m_nextChunkMaxSize = objArray.Length;
            }
            mutables.m_currentChunkIndex = -1;
          }
          else
            break;
        }
        currentElement = objArray[index];
        currentKey = mutables.m_chunkBaseIndex + index;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        if (Interlocked.Decrement(ref this.m_activeEnumeratorsCount.Value) != 0)
          return;
        this.m_source.Dispose();
      }

      private class Mutables
      {
        internal readonly T[] m_chunkBuffer;
        internal int m_nextChunkMaxSize;
        internal int m_currentChunkSize;
        internal int m_currentChunkIndex;
        internal int m_chunkBaseIndex;
        internal int m_chunkCounter;

        internal Mutables()
        {
          this.m_nextChunkMaxSize = 1;
          this.m_chunkBuffer = new T[Scheduling.GetDefaultChunkSize<T>()];
          this.m_currentChunkSize = 0;
          this.m_currentChunkIndex = -1;
          this.m_chunkBaseIndex = 0;
          this.m_chunkCounter = 0;
        }
      }
    }
  }
}
