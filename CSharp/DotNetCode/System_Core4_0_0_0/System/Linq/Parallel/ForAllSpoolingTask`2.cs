// Type: System.Linq.Parallel.ForAllSpoolingTask`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;

namespace System.Linq.Parallel
{
  internal class ForAllSpoolingTask<TInputOutput, TIgnoreKey> : SpoolingTaskBase
  {
    private QueryOperatorEnumerator<TInputOutput, TIgnoreKey> m_source;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ForAllSpoolingTask(int taskIndex, QueryTaskGroupState groupState, QueryOperatorEnumerator<TInputOutput, TIgnoreKey> source)
      : base(taskIndex, groupState)
    {
      this.m_source = source;
    }

    protected override void SpoolingWork()
    {
      TInputOutput currentElement = default (TInputOutput);
      TIgnoreKey currentKey = default (TIgnoreKey);
      do
        ;
      while (this.m_source.MoveNext(ref currentElement, ref currentKey));
    }

    protected override void SpoolingFinally()
    {
      base.SpoolingFinally();
      this.m_source.Dispose();
    }
  }
}
