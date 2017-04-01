// Type: System.Linq.Parallel.Scheduling
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.InteropServices;

namespace System.Linq.Parallel
{
  internal static class Scheduling
  {
    internal static int DefaultDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 512);
    internal const bool DefaultPreserveOrder = false;
    internal const int DEFAULT_BOUNDED_BUFFER_CAPACITY = 512;
    internal const int DEFAULT_BYTES_PER_CHUNK = 512;
    internal const int ZOMBIED_PRODUCER_TIMEOUT = -1;
    internal const int MAX_SUPPORTED_DOP = 512;

    static Scheduling()
    {
    }

    internal static int GetDefaultChunkSize<T>()
    {
      return !typeof (T).IsValueType ? 512 / IntPtr.Size : (typeof (T).StructLayoutAttribute.Value != LayoutKind.Explicit ? 128 : Math.Max(1, 512 / Marshal.SizeOf(typeof (T))));
    }

    internal static int GetDefaultDegreeOfParallelism()
    {
      return Scheduling.DefaultDegreeOfParallelism;
    }
  }
}
