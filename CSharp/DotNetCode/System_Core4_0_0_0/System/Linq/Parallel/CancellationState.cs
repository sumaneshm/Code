// Type: System.Linq.Parallel.CancellationState
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class CancellationState
  {
    internal CancellationTokenSource InternalCancellationTokenSource;
    internal CancellationToken ExternalCancellationToken;
    internal CancellationTokenSource MergedCancellationTokenSource;
    internal Shared<bool> TopLevelDisposedFlag;
    internal const int POLL_INTERVAL = 63;

    internal CancellationToken MergedCancellationToken
    {
      get
      {
        if (this.MergedCancellationTokenSource != null)
          return this.MergedCancellationTokenSource.Token;
        else
          return new CancellationToken(false);
      }
    }

    internal CancellationState(CancellationToken externalCancellationToken)
    {
      this.ExternalCancellationToken = externalCancellationToken;
      this.TopLevelDisposedFlag = new Shared<bool>(false);
    }

    internal static void ThrowIfCanceled(CancellationToken token)
    {
      if (token.IsCancellationRequested)
        throw new OperationCanceledException(token);
    }

    internal static void ThrowWithStandardMessageIfCanceled(CancellationToken externalCancellationToken)
    {
      if (externalCancellationToken.IsCancellationRequested)
        throw new OperationCanceledException(System.Linq.SR.GetString("PLINQ_ExternalCancellationRequested"), externalCancellationToken);
    }
  }
}
