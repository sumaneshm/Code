// Type: System.Security.Cryptography.BCryptHashAlgorithm
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Security.Cryptography
{
  internal sealed class BCryptHashAlgorithm : IDisposable
  {
    [ThreadStatic]
    [SecurityCritical]
    private static BCryptAlgorithmHandleCache _algorithmCache;
    [SecurityCritical]
    private SafeBCryptAlgorithmHandle m_algorithmHandle;
    [SecurityCritical]
    private SafeBCryptHashHandle m_hashHandle;

    [SecuritySafeCritical]
    public BCryptHashAlgorithm(CngAlgorithm algorithm, string implementation)
    {
      if (!BCryptNative.BCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      if (BCryptHashAlgorithm._algorithmCache == null)
        BCryptHashAlgorithm._algorithmCache = new BCryptAlgorithmHandleCache();
      this.m_algorithmHandle = BCryptHashAlgorithm._algorithmCache.GetCachedAlgorithmHandle(algorithm.Algorithm, implementation);
      this.Initialize();
    }

    [SecuritySafeCritical]
    public void Dispose()
    {
      if (this.m_hashHandle != null)
        this.m_hashHandle.Dispose();
      if (this.m_algorithmHandle == null)
        return;
      this.m_algorithmHandle = (SafeBCryptAlgorithmHandle) null;
    }

    [SecuritySafeCritical]
    public void Initialize()
    {
      SafeBCryptHashHandle phHash = (SafeBCryptHashHandle) null;
      IntPtr num = IntPtr.Zero;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        int int32Property = BCryptNative.GetInt32Property<SafeBCryptAlgorithmHandle>(this.m_algorithmHandle, "ObjectLength");
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
        }
        finally
        {
          num = Marshal.AllocCoTaskMem(int32Property);
        }
        BCryptNative.ErrorCode hash = BCryptNative.UnsafeNativeMethods.BCryptCreateHash(this.m_algorithmHandle, out phHash, num, int32Property, IntPtr.Zero, 0, 0);
        if (hash != BCryptNative.ErrorCode.Success)
          throw new CryptographicException((int) hash);
      }
      finally
      {
        if (num != IntPtr.Zero)
        {
          if (phHash != null)
            phHash.HashObject = num;
          else
            Marshal.FreeCoTaskMem(num);
        }
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
      byte[] pbInput = new byte[cbSize];
      Buffer.BlockCopy((Array) array, ibStart, (Array) pbInput, 0, cbSize);
      BCryptNative.ErrorCode errorCode = BCryptNative.UnsafeNativeMethods.BCryptHashData(this.m_hashHandle, pbInput, pbInput.Length, 0);
      if (errorCode != BCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
    }

    [SecuritySafeCritical]
    public byte[] HashFinal()
    {
      byte[] pbInput = new byte[BCryptNative.GetInt32Property<SafeBCryptHashHandle>(this.m_hashHandle, "HashDigestLength")];
      BCryptNative.ErrorCode errorCode = BCryptNative.UnsafeNativeMethods.BCryptFinishHash(this.m_hashHandle, pbInput, pbInput.Length, 0);
      if (errorCode != BCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      else
        return pbInput;
    }

    [SecuritySafeCritical]
    public void HashStream(Stream stream)
    {
      byte[] numArray = new byte[4096];
      int cbSize;
      do
      {
        cbSize = stream.Read(numArray, 0, numArray.Length);
        if (cbSize > 0)
          this.HashCore(numArray, 0, cbSize);
      }
      while (cbSize > 0);
    }
  }
}
