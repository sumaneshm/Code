// Type: System.Linq.Parallel.PipelineSpoolingTask`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class PipelineSpoolingTask<TInputOutput, TIgnoreKey> : SpoolingTaskBase
  {
    private QueryOperatorEnumerator<TInputOutput, TIgnoreKey> m_source;
    private AsynchronousChannel<TInputOutput> m_destination;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal PipelineSpoolingTask(int taskIndex, QueryTaskGroupState groupState, QueryOperatorEnumerator<TInputOutput, TIgnoreKey> source, AsynchronousChannel<TInputOutput> destination)
      : base(taskIndex, groupState)
    {
      this.m_source = source;
      this.m_destination = destination;
    }

    protected override void SpoolingWork()
    {
      TInputOutput currentElement = default (TInputOutput);
      TIgnoreKey currentKey = default (TIgnoreKey);
      QueryOperatorEnumerator<TInputOutput, TIgnoreKey> operatorEnumerator = this.m_source;
      AsynchronousChannel<TInputOutput> asynchronousChannel = this.m_destination;
      CancellationToken cancellationToken = this.m_groupState.CancellationState.MergedCancellationToken;
      while (operatorEnumerator.MoveNext(ref currentElement, ref currentKey) && !cancellationToken.IsCancellationRequested)
        asynchronousChannel.Enqueue(currentElement);
      asynchronousChannel.FlushBuffers();
    }

    protected override void SpoolingFinally()
    {
      base.SpoolingFinally();
      if (this.m_destination != null)
        this.m_destination.SetDone();
      this.m_source.Dispose();
    }
  }
}
