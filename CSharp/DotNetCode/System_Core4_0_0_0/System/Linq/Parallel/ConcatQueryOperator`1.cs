// Type: System.Linq.Parallel.ConcatQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class ConcatQueryOperator<TSource> : BinaryQueryOperator<TSource, TSource, TSource>
  {
    private readonly bool m_prematureMergeLeft;
    private readonly bool m_prematureMergeRight;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal ConcatQueryOperator(ParallelQuery<TSource> firstChild, ParallelQuery<TSource> secondChild)
      : base(firstChild, secondChild)
    {
      this.m_outputOrdered = this.LeftChild.OutputOrdered || this.RightChild.OutputOrdered;
      this.m_prematureMergeLeft = ExchangeUtilities.IsWorseThan(this.LeftChild.OrdinalIndexState, OrdinalIndexState.Increasing);
      this.m_prematureMergeRight = ExchangeUtilities.IsWorseThan(this.RightChild.OrdinalIndexState, OrdinalIndexState.Increasing);
      if (this.LeftChild.OrdinalIndexState == OrdinalIndexState.Indexible && this.RightChild.OrdinalIndexState == OrdinalIndexState.Indexible)
        this.SetOrdinalIndex(OrdinalIndexState.Indexible);
      else
        this.SetOrdinalIndex(ExchangeUtilities.Worse(OrdinalIndexState.Increasing, ExchangeUtilities.Worse(this.LeftChild.OrdinalIndexState, this.RightChild.OrdinalIndexState)));
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return ConcatQueryOperator<TSource>.ConcatQueryOperatorResults.NewResults(this.LeftChild.Open(settings, preferStriping), this.RightChild.Open(settings, preferStriping), this, settings, preferStriping);
    }

    public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TSource, TLeftKey> leftStream, PartitionedStream<TSource, TRightKey> rightStream, IPartitionedStreamRecipient<TSource> outputRecipient, bool preferStriping, QuerySettings settings)
    {
      if (this.m_prematureMergeLeft)
        this.WrapHelper<int, TRightKey>(QueryOperator<TSource>.ExecuteAndCollectResults<TLeftKey>(leftStream, leftStream.PartitionCount, this.LeftChild.OutputOrdered, preferStriping, settings).GetPartitionedStream(), rightStream, outputRecipient, settings, preferStriping);
      else
        this.WrapHelper<TLeftKey, TRightKey>(leftStream, rightStream, outputRecipient, settings, preferStriping);
    }

    private void WrapHelper<TLeftKey, TRightKey>(PartitionedStream<TSource, TLeftKey> leftStreamInc, PartitionedStream<TSource, TRightKey> rightStream, IPartitionedStreamRecipient<TSource> outputRecipient, QuerySettings settings, bool preferStriping)
    {
      if (this.m_prematureMergeRight)
      {
        PartitionedStream<TSource, int> partitionedStream = QueryOperator<TSource>.ExecuteAndCollectResults<TRightKey>(rightStream, leftStreamInc.PartitionCount, this.LeftChild.OutputOrdered, preferStriping, settings).GetPartitionedStream();
        this.WrapHelper2<TLeftKey, int>(leftStreamInc, partitionedStream, outputRecipient);
      }
      else
        this.WrapHelper2<TLeftKey, TRightKey>(leftStreamInc, rightStream, outputRecipient);
    }

    private void WrapHelper2<TLeftKey, TRightKey>(PartitionedStream<TSource, TLeftKey> leftStreamInc, PartitionedStream<TSource, TRightKey> rightStreamInc, IPartitionedStreamRecipient<TSource> outputRecipient)
    {
      int partitionCount = leftStreamInc.PartitionCount;
      IComparer<ConcatKey<TLeftKey, TRightKey>> keyComparer = ConcatKey<TLeftKey, TRightKey>.MakeComparer(leftStreamInc.KeyComparer, rightStreamInc.KeyComparer);
      PartitionedStream<TSource, ConcatKey<TLeftKey, TRightKey>> partitionedStream = new PartitionedStream<TSource, ConcatKey<TLeftKey, TRightKey>>(partitionCount, keyComparer, this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TSource, ConcatKey<TLeftKey, TRightKey>>) new ConcatQueryOperator<TSource>.ConcatQueryOperatorEnumerator<TLeftKey, TRightKey>(leftStreamInc[index], rightStreamInc[index]);
      outputRecipient.Receive<ConcatKey<TLeftKey, TRightKey>>(partitionedStream);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Concat<TSource>(this.LeftChild.AsSequentialQuery(token), this.RightChild.AsSequentialQuery(token));
    }

    private class ConcatQueryOperatorEnumerator<TLeftKey, TRightKey> : QueryOperatorEnumerator<TSource, ConcatKey<TLeftKey, TRightKey>>
    {
      private QueryOperatorEnumerator<TSource, TLeftKey> m_firstSource;
      private QueryOperatorEnumerator<TSource, TRightKey> m_secondSource;
      private bool m_begunSecond;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ConcatQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TLeftKey> firstSource, QueryOperatorEnumerator<TSource, TRightKey> secondSource)
      {
        this.m_firstSource = firstSource;
        this.m_secondSource = secondSource;
      }

      internal override bool MoveNext(ref TSource currentElement, ref ConcatKey<TLeftKey, TRightKey> currentKey)
      {
        if (!this.m_begunSecond)
        {
          TLeftKey currentKey1 = default (TLeftKey);
          if (this.m_firstSource.MoveNext(ref currentElement, ref currentKey1))
          {
            currentKey = ConcatKey<TLeftKey, TRightKey>.MakeLeft(currentKey1);
            return true;
          }
          else
            this.m_begunSecond = true;
        }
        TRightKey currentKey2 = default (TRightKey);
        if (!this.m_secondSource.MoveNext(ref currentElement, ref currentKey2))
          return false;
        currentKey = ConcatKey<TLeftKey, TRightKey>.MakeRight(currentKey2);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_firstSource.Dispose();
        this.m_secondSource.Dispose();
      }
    }

    private class ConcatQueryOperatorResults : BinaryQueryOperator<TSource, TSource, TSource>.BinaryQueryOperatorResults
    {
      private ConcatQueryOperator<TSource> m_concatOp;
      private int m_leftChildCount;
      private int m_rightChildCount;

      internal override bool IsIndexible
      {
        get
        {
          return true;
        }
      }

      internal override int ElementsCount
      {
        get
        {
          return this.m_leftChildCount + this.m_rightChildCount;
        }
      }

      private ConcatQueryOperatorResults(QueryResults<TSource> leftChildQueryResults, QueryResults<TSource> rightChildQueryResults, ConcatQueryOperator<TSource> concatOp, QuerySettings settings, bool preferStriping)
        : base(leftChildQueryResults, rightChildQueryResults, (BinaryQueryOperator<TSource, TSource, TSource>) concatOp, settings, preferStriping)
      {
        this.m_concatOp = concatOp;
        this.m_leftChildCount = leftChildQueryResults.ElementsCount;
        this.m_rightChildCount = rightChildQueryResults.ElementsCount;
      }

      public static QueryResults<TSource> NewResults(QueryResults<TSource> leftChildQueryResults, QueryResults<TSource> rightChildQueryResults, ConcatQueryOperator<TSource> op, QuerySettings settings, bool preferStriping)
      {
        if (leftChildQueryResults.IsIndexible && rightChildQueryResults.IsIndexible)
          return (QueryResults<TSource>) new ConcatQueryOperator<TSource>.ConcatQueryOperatorResults(leftChildQueryResults, rightChildQueryResults, op, settings, preferStriping);
        else
          return (QueryResults<TSource>) new BinaryQueryOperator<TSource, TSource, TSource>.BinaryQueryOperatorResults(leftChildQueryResults, rightChildQueryResults, (BinaryQueryOperator<TSource, TSource, TSource>) op, settings, preferStriping);
      }

      internal override TSource GetElement(int index)
      {
        if (index < this.m_leftChildCount)
          return this.m_leftChildQueryResults.GetElement(index);
        else
          return this.m_rightChildQueryResults.GetElement(index - this.m_leftChildCount);
      }
    }
  }
}
