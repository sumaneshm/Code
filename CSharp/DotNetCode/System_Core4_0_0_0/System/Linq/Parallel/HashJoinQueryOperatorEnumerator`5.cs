// Type: System.Linq.Parallel.HashJoinQueryOperatorEnumerator`5
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, THashKey, TOutput> : QueryOperatorEnumerator<TOutput, TLeftKey>
  {
    private readonly QueryOperatorEnumerator<Pair<TLeftInput, THashKey>, TLeftKey> m_leftSource;
    private readonly QueryOperatorEnumerator<Pair<TRightInput, THashKey>, int> m_rightSource;
    private readonly Func<TLeftInput, TRightInput, TOutput> m_singleResultSelector;
    private readonly Func<TLeftInput, IEnumerable<TRightInput>, TOutput> m_groupResultSelector;
    private readonly IEqualityComparer<THashKey> m_keyComparer;
    private readonly CancellationToken m_cancellationToken;
    private HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, THashKey, TOutput>.Mutables m_mutables;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal HashJoinQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TLeftInput, THashKey>, TLeftKey> leftSource, QueryOperatorEnumerator<Pair<TRightInput, THashKey>, int> rightSource, Func<TLeftInput, TRightInput, TOutput> singleResultSelector, Func<TLeftInput, IEnumerable<TRightInput>, TOutput> groupResultSelector, IEqualityComparer<THashKey> keyComparer, CancellationToken cancellationToken)
    {
      this.m_leftSource = leftSource;
      this.m_rightSource = rightSource;
      this.m_singleResultSelector = singleResultSelector;
      this.m_groupResultSelector = groupResultSelector;
      this.m_keyComparer = keyComparer;
      this.m_cancellationToken = cancellationToken;
    }

    internal override bool MoveNext(ref TOutput currentElement, ref TLeftKey currentKey)
    {
      HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, THashKey, TOutput>.Mutables mutables = this.m_mutables;
      if (mutables == null)
      {
        mutables = this.m_mutables = new HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, THashKey, TOutput>.Mutables();
        mutables.m_rightHashLookup = new HashLookup<THashKey, Pair<TRightInput, ListChunk<TRightInput>>>(this.m_keyComparer);
        Pair<TRightInput, THashKey> currentElement1 = new Pair<TRightInput, THashKey>();
        int currentKey1 = 0;
        int num = 0;
        while (this.m_rightSource.MoveNext(ref currentElement1, ref currentKey1))
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          TRightInput first = currentElement1.First;
          THashKey second = currentElement1.Second;
          if ((object) second != null)
          {
            Pair<TRightInput, ListChunk<TRightInput>> pair = new Pair<TRightInput, ListChunk<TRightInput>>();
            if (!mutables.m_rightHashLookup.TryGetValue(second, ref pair))
            {
              pair = new Pair<TRightInput, ListChunk<TRightInput>>(first, (ListChunk<TRightInput>) null);
              if (this.m_groupResultSelector != null)
              {
                pair.Second = new ListChunk<TRightInput>(2);
                pair.Second.Add(first);
              }
              mutables.m_rightHashLookup.Add(second, pair);
            }
            else
            {
              if (pair.Second == null)
              {
                pair.Second = new ListChunk<TRightInput>(2);
                mutables.m_rightHashLookup[second] = pair;
              }
              pair.Second.Add(first);
            }
          }
        }
      }
      ListChunk<TRightInput> listChunk1 = mutables.m_currentRightMatches;
      if (listChunk1 != null && mutables.m_currentRightMatchesIndex == listChunk1.Count)
      {
        ListChunk<TRightInput> listChunk2 = mutables.m_currentRightMatches = listChunk1.Next;
        mutables.m_currentRightMatchesIndex = 0;
      }
      if (mutables.m_currentRightMatches == null)
      {
        Pair<TLeftInput, THashKey> currentElement1 = new Pair<TLeftInput, THashKey>();
        TLeftKey currentKey1 = default (TLeftKey);
        while (this.m_leftSource.MoveNext(ref currentElement1, ref currentKey1))
        {
          if ((mutables.m_outputLoopCount++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          Pair<TRightInput, ListChunk<TRightInput>> pair = new Pair<TRightInput, ListChunk<TRightInput>>();
          TLeftInput first = currentElement1.First;
          THashKey second = currentElement1.Second;
          if ((object) second != null && mutables.m_rightHashLookup.TryGetValue(second, ref pair) && this.m_singleResultSelector != null)
          {
            mutables.m_currentRightMatches = pair.Second;
            mutables.m_currentRightMatchesIndex = 0;
            currentElement = this.m_singleResultSelector(first, pair.First);
            currentKey = currentKey1;
            if (pair.Second != null)
            {
              mutables.m_currentLeft = first;
              mutables.m_currentLeftKey = currentKey1;
            }
            return true;
          }
          else if (this.m_groupResultSelector != null)
          {
            IEnumerable<TRightInput> enumerable = (IEnumerable<TRightInput>) pair.Second ?? (IEnumerable<TRightInput>) ParallelEnumerable.Empty<TRightInput>();
            currentElement = this.m_groupResultSelector(first, enumerable);
            currentKey = currentKey1;
            return true;
          }
        }
        return false;
      }
      else
      {
        currentElement = this.m_singleResultSelector(mutables.m_currentLeft, mutables.m_currentRightMatches.m_chunk[mutables.m_currentRightMatchesIndex]);
        currentKey = mutables.m_currentLeftKey;
        ++mutables.m_currentRightMatchesIndex;
        return true;
      }
    }

    protected override void Dispose(bool disposing)
    {
      this.m_leftSource.Dispose();
      this.m_rightSource.Dispose();
    }

    private class Mutables
    {
      internal TLeftInput m_currentLeft;
      internal TLeftKey m_currentLeftKey;
      internal HashLookup<THashKey, Pair<TRightInput, ListChunk<TRightInput>>> m_rightHashLookup;
      internal ListChunk<TRightInput> m_currentRightMatches;
      internal int m_currentRightMatchesIndex;
      internal int m_outputLoopCount;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public Mutables()
      {
      }
    }
  }
}
