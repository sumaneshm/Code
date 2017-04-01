// Type: System.Linq.Parallel.ExceptionAggregator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
  internal static class ExceptionAggregator
  {
    internal static IEnumerable<TElement> WrapEnumerable<TElement>(IEnumerable<TElement> source, CancellationState cancellationState)
    {
      using (IEnumerator<TElement> enumerator = source.GetEnumerator())
      {
        while (true)
        {
          TElement elem = default (TElement);
          try
          {
            if (!enumerator.MoveNext())
              break;
            else
              elem = enumerator.Current;
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            ExceptionAggregator.ThrowOCEorAggregateException(ex, cancellationState);
          }
          yield return elem;
        }
      }
    }

    internal static IEnumerable<TElement> WrapQueryEnumerator<TElement, TIgnoreKey>(QueryOperatorEnumerator<TElement, TIgnoreKey> source, CancellationState cancellationState)
    {
      TElement elem = default (TElement);
      TIgnoreKey ignoreKey = default (TIgnoreKey);
      try
      {
        while (true)
        {
          try
          {
            if (!source.MoveNext(ref elem, ref ignoreKey))
              break;
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            ExceptionAggregator.ThrowOCEorAggregateException(ex, cancellationState);
          }
          yield return elem;
        }
      }
      finally
      {
        source.Dispose();
      }
    }

    internal static void ThrowOCEorAggregateException(Exception ex, CancellationState cancellationState)
    {
      if (ExceptionAggregator.ThrowAnOCE(ex, cancellationState))
        CancellationState.ThrowWithStandardMessageIfCanceled(cancellationState.ExternalCancellationToken);
      else
        throw new AggregateException(new Exception[1]
        {
          ex
        });
    }

    internal static Func<T, U> WrapFunc<T, U>(Func<T, U> f, CancellationState cancellationState)
    {
      return (Func<T, U>) (t =>
      {
        U local_0 = default (U);
        try
        {
          local_0 = f(t);
        }
        catch (ThreadAbortException exception_0)
        {
          throw;
        }
        catch (Exception exception_1)
        {
          ExceptionAggregator.ThrowOCEorAggregateException(exception_1, cancellationState);
        }
        return local_0;
      });
    }

    private static bool ThrowAnOCE(Exception ex, CancellationState cancellationState)
    {
      OperationCanceledException canceledException = ex as OperationCanceledException;
      return canceledException != null && canceledException.CancellationToken == cancellationState.ExternalCancellationToken && cancellationState.ExternalCancellationToken.IsCancellationRequested || canceledException != null && canceledException.CancellationToken == cancellationState.MergedCancellationToken && (cancellationState.MergedCancellationToken.IsCancellationRequested && cancellationState.ExternalCancellationToken.IsCancellationRequested);
    }
  }
}
