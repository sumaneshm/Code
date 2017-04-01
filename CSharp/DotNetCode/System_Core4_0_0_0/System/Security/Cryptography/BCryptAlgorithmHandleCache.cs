// Type: System.Security.Cryptography.BCryptAlgorithmHandleCache
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Security;

namespace System.Security.Cryptography
{
  internal sealed class BCryptAlgorithmHandleCache
  {
    [SecurityCritical]
    private Dictionary<string, WeakReference> m_algorithmHandles;

    [SecurityCritical]
    public BCryptAlgorithmHandleCache()
    {
      this.m_algorithmHandles = new Dictionary<string, WeakReference>();
    }

    [SecuritySafeCritical]
    public SafeBCryptAlgorithmHandle GetCachedAlgorithmHandle(string algorithm, string implementation)
    {
      string key = algorithm + implementation;
      if (this.m_algorithmHandles.ContainsKey(key))
      {
        SafeBCryptAlgorithmHandle bcryptAlgorithmHandle = this.m_algorithmHandles[key].Target as SafeBCryptAlgorithmHandle;
        if (bcryptAlgorithmHandle != null)
          return bcryptAlgorithmHandle;
      }
      SafeBCryptAlgorithmHandle bcryptAlgorithmHandle1 = BCryptNative.OpenAlgorithm(algorithm, implementation);
      this.m_algorithmHandles[key] = new WeakReference((object) bcryptAlgorithmHandle1);
      return bcryptAlgorithmHandle1;
    }
  }
}
