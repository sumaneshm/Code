// Type: System.Linq.Parallel.PartitionerQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class PartitionerQueryOperator<TElement> : QueryOperator<TElement>
  {
    private Partitioner<TElement> m_partitioner;

    internal bool Orderable
    {
      get
      {
        return this.m_partitioner is OrderablePartitioner<TElement>;
      }
    }

    internal override OrdinalIndexState OrdinalIndexState
    {
      get
      {
        return PartitionerQueryOperator<TElement>.GetOrdinalIndexState(this.m_partitioner);
      }
    }

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal PartitionerQueryOperator(Partitioner<TElement> partitioner)
      : base(false, QuerySettings.Empty)
    {
      this.m_partitioner = partitioner;
    }

    internal override QueryResults<TElement> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TElement>) new PartitionerQueryOperator<TElement>.PartitionerQueryOperatorResults(this.m_partitioner, settings);
    }

    internal override IEnumerable<TElement> AsSequentialQuery(CancellationToken token)
    {
      using (IEnumerator<TElement> enumerator = this.m_partitioner.GetPartitions(1)[0])
      {
        while (enumerator.MoveNext())
          yield return enumerator.Current;
      }
    }

    internal static OrdinalIndexState GetOrdinalIndexState(Partitioner<TElement> partitioner)
    {
      OrderablePartitioner<TElement> orderablePartitioner = partitioner as OrderablePartitioner<TElement>;
      if (orderablePartitioner == null || !orderablePartitioner.KeysOrderedInEachPartition)
        return OrdinalIndexState.Shuffled;
      return orderablePartitioner.KeysNormalized ? OrdinalIndexState.Correct : OrdinalIndexState.Increasing;
    }

    private class PartitionerQueryOperatorResults : QueryResults<TElement>
    {
      private Partitioner<TElement> m_partitioner;
      private QuerySettings m_settings;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal PartitionerQueryOperatorResults(Partitioner<TElement> partitioner, QuerySettings settings)
      {
        this.m_partitioner = partitioner;
        this.m_settings = settings;
      }

      internal override void GivePartitionedStream(IPartitionedStreamRecipient<TElement> recipient)
      {
        int partitionCount = this.m_settings.DegreeOfParallelism.Value;
        OrderablePartitioner<TElement> orderablePartitioner = this.m_partitioner as OrderablePartitioner<TElement>;
        OrdinalIndexState indexState = orderablePartitioner != null ? PartitionerQueryOperator<TElement>.GetOrdinalIndexState((Partitioner<TElement>) orderablePartitioner) : OrdinalIndexState.Shuffled;
        PartitionedStream<TElement, int> partitionedStream = new PartitionedStream<TElement, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), indexState);
        if (orderablePartitioner != null)
        {
          IList<IEnumerator<KeyValuePair<long, TElement>>> orderablePartitions = orderablePartitioner.GetOrderablePartitions(partitionCount);
          if (orderablePartitions == null)
            throw new InvalidOperationException(System.Linq.SR.GetString("PartitionerQueryOperator_NullPartitionList"));
          if (orderablePartitions.Count != partitionCount)
            throw new InvalidOperationException(System.Linq.SR.GetString("PartitionerQueryOperator_WrongNumberOfPartitions"));
          for (int index = 0; index < partitionCount; ++index)
          {
            IEnumerator<KeyValuePair<long, TElement>> sourceEnumerator = orderablePartitions[index];
            if (sourceEnumerator == null)
              throw new InvalidOperationException(System.Linq.SR.GetString("PartitionerQueryOperator_NullPartition"));
            partitionedStream[index] = (QueryOperatorEnumerator<TElement, int>) new PartitionerQueryOperator<TElement>.OrderablePartitionerEnumerator(sourceEnumerator);
          }
        }
        else
        {
          IList<IEnumerator<TElement>> partitions = this.m_partitioner.GetPartitions(partitionCount);
          if (partitions == null)
            throw new InvalidOperationException(System.Linq.SR.GetString("PartitionerQueryOperator_NullPartitionList"));
          if (partitions.Count != partitionCount)
            throw new InvalidOperationException(System.Linq.SR.GetString("PartitionerQueryOperator_WrongNumberOfPartitions"));
          for (int index = 0; index < partitionCount; ++index)
          {
            IEnumerator<TElement> sourceEnumerator = partitions[index];
            if (sourceEnumerator == null)
              throw new InvalidOperationException(System.Linq.SR.GetString("PartitionerQueryOperator_NullPartition"));
            partitionedStream[index] = (QueryOperatorEnumerator<TElement, int>) new PartitionerQueryOperator<TElement>.PartitionerEnumerator(sourceEnumerator);
          }
        }
        recipient.Receive<int>(partitionedStream);
      }
    }

    private class OrderablePartitionerEnumerator : QueryOperatorEnumerator<TElement, int>
    {
      private IEnumerator<KeyValuePair<long, TElement>> m_sourceEnumerator;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal OrderablePartitionerEnumerator(IEnumerator<KeyValuePair<long, TElement>> sourceEnumerator)
      {
        this.m_sourceEnumerator = sourceEnumerator;
      }

      internal override bool MoveNext(ref TElement currentElement, ref int currentKey)
      {
        if (!this.m_sourceEnumerator.MoveNext())
          return false;
        KeyValuePair<long, TElement> current = this.m_sourceEnumerator.Current;
        currentElement = current.Value;
        currentKey = checked ((int) current.Key);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_sourceEnumerator.Dispose();
      }
    }

    private class PartitionerEnumerator : QueryOperatorEnumerator<TElement, int>
    {
      private IEnumerator<TElement> m_sourceEnumerator;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal PartitionerEnumerator(IEnumerator<TElement> sourceEnumerator)
      {
        this.m_sourceEnumerator = sourceEnumerator;
      }

      internal override bool MoveNext(ref TElement currentElement, ref int currentKey)
      {
        if (!this.m_sourceEnumerator.MoveNext())
          return false;
        currentElement = this.m_sourceEnumerator.Current;
        currentKey = 0;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_sourceEnumerator.Dispose();
      }
    }
  }
}
