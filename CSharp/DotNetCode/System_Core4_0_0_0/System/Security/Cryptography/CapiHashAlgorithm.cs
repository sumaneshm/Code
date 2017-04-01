// Type: System.Security.Cryptography.CapiHashAlgorithm
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Security.Cryptography
{
  internal sealed class CapiHashAlgorithm : IDisposable
  {
    private CapiNative.AlgorithmId m_algorithmId;
    [SecurityCritical]
    private SafeCspHandle m_cspHandle;
    [SecurityCritical]
    private SafeCapiHashHandle m_hashHandle;

    [SecuritySafeCritical]
    public CapiHashAlgorithm(string provider, CapiNative.ProviderType providerType, CapiNative.AlgorithmId algorithm)
    {
      this.m_algorithmId = algorithm;
      this.m_cspHandle = CapiNative.AcquireCsp((string) null, provider, providerType, CapiNative.CryptAcquireContextFlags.VerifyContext, true);
      this.Initialize();
    }

    [SecuritySafeCritical]
    public void Dispose()
    {
      if (this.m_hashHandle != null)
        this.m_hashHandle.Dispose();
      if (this.m_cspHandle == null)
        return;
      this.m_cspHandle.Dispose();
    }

    [SecuritySafeCritical]
    public void Initialize()
    {
      SafeCapiHashHandle phHash = (SafeCapiHashHandle) null;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        if (!CapiNative.UnsafeNativeMethods.CryptCreateHash(this.m_cspHandle, this.m_algorithmId, SafeCapiKeyHandle.InvalidHandle, 0, out phHash))
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          if (lastWin32Error == -2146893816)
            throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
          else
            throw new CryptographicException(lastWin32Error);
        }
      }
      finally
      {
        if (phHash != null && !phHash.IsInvalid)
          phHash.SetParentCsp(this.m_cspHandle);
      }
      if (this.m_hashHandle != null)
        this.m_hashHandle.Dispose();
      this.m_hashHandle = phHash;
    }

    [SecuritySafeCritical]
    public void HashCore(byte[] array, int ibStart, int cbSize)
    {
      if (array == null)
        throw new ArgumentNullException("array");
      if (ibStart < 0 || ibStart > array.Length - cbSize)
        throw new ArgumentOutOfRangeException("ibStart");
      if (cbSize < 0 || cbSize > array.Length)
        throw new ArgumentOutOfRangeException("cbSize");
      byte[] pbData = new byte[cbSize];
      Buffer.BlockCopy((Array) array, ibStart, (Array) pbData, 0, cbSize);
      if (!CapiNative.UnsafeNativeMethods.CryptHashData(this.m_hashHandle, pbData, cbSize, 0))
        throw new CryptographicException(Marshal.GetLastWin32Error());
    }

    [SecuritySafeCritical]
    public byte[] HashFinal()
    {
      return CapiNative.GetHashParameter(this.m_hashHandle, CapiNative.HashParameter.HashValue);
    }
  }
}
