// Type: System.Linq.Parallel.JoinQueryOperator`4
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class JoinQueryOperator<TLeftInput, TRightInput, TKey, TOutput> : BinaryQueryOperator<TLeftInput, TRightInput, TOutput>
  {
    private readonly Func<TLeftInput, TKey> m_leftKeySelector;
    private readonly Func<TRightInput, TKey> m_rightKeySelector;
    private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;
    private readonly IEqualityComparer<TKey> m_keyComparer;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal JoinQueryOperator(ParallelQuery<TLeftInput> left, ParallelQuery<TRightInput> right, Func<TLeftInput, TKey> leftKeySelector, Func<TRightInput, TKey> rightKeySelector, Func<TLeftInput, TRightInput, TOutput> resultSelector, IEqualityComparer<TKey> keyComparer)
      : base(left, right)
    {
      this.m_leftKeySelector = leftKeySelector;
      this.m_rightKeySelector = rightKeySelector;
      this.m_resultSelector = resultSelector;
      this.m_keyComparer = keyComparer;
      this.m_outputOrdered = this.LeftChild.OutputOrdered;
      this.SetOrdinalIndex(OrdinalIndexState.Shuffled);
    }

    public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TLeftInput, TLeftKey> leftStream, PartitionedStream<TRightInput, TRightKey> rightStream, IPartitionedStreamRecipient<TOutput> outputRecipient, bool preferStriping, QuerySettings settings)
    {
      if (this.LeftChild.OutputOrdered)
        this.WrapPartitionedStreamHelper<TLeftKey, TRightKey>(ExchangeUtilities.HashRepartitionOrdered<TLeftInput, TKey, TLeftKey>(leftStream, this.m_leftKeySelector, this.m_keyComparer, (IEqualityComparer<TLeftInput>) null, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
      else
        this.WrapPartitionedStreamHelper<int, TRightKey>(ExchangeUtilities.HashRepartition<TLeftInput, TKey, TLeftKey>(leftStream, this.m_leftKeySelector, this.m_keyComparer, (IEqualityComparer<TLeftInput>) null, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
    }

    private void WrapPartitionedStreamHelper<TLeftKey, TRightKey>(PartitionedStream<Pair<TLeftInput, TKey>, TLeftKey> leftHashStream, PartitionedStream<TRightInput, TRightKey> rightPartitionedStream, IPartitionedStreamRecipient<TOutput> outputRecipient, CancellationToken cancellationToken)
    {
      int partitionCount = leftHashStream.PartitionCount;
      PartitionedStream<Pair<TRightInput, TKey>, int> partitionedStream1 = ExchangeUtilities.HashRepartition<TRightInput, TKey, TRightKey>(rightPartitionedStream, this.m_rightKeySelector, this.m_keyComparer, (IEqualityComparer<TRightInput>) null, cancellationToken);
      PartitionedStream<TOutput, TLeftKey> partitionedStream2 = new PartitionedStream<TOutput, TLeftKey>(partitionCount, leftHashStream.KeyComparer, this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream2[index] = (QueryOperatorEnumerator<TOutput, TLeftKey>) new HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, TKey, TOutput>(leftHashStream[index], partitionedStream1[index], this.m_resultSelector, (Func<TLeftInput, IEnumerable<TRightInput>, TOutput>) null, this.m_keyComparer, cancellationToken);
      outputRecipient.Receive<TLeftKey>(partitionedStream2);
    }

    internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TOutput>) new BinaryQueryOperator<TLeftInput, TRightInput, TOutput>.BinaryQueryOperatorResults(this.LeftChild.Open(settings, false), this.RightChild.Open(settings, false), (BinaryQueryOperator<TLeftInput, TRightInput, TOutput>) this, settings, false);
    }

    internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Join<TLeftInput, TRightInput, TKey, TOutput>(CancellableEnumerable.Wrap<TLeftInput>(this.LeftChild.AsSequentialQuery(token), token), CancellableEnumerable.Wrap<TRightInput>(this.RightChild.AsSequentialQuery(token), token), this.m_leftKeySelector, this.m_rightKeySelector, this.m_resultSelector, this.m_keyComparer);
    }
  }
}
