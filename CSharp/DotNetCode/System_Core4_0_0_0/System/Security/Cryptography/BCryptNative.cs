// Type: System.Security.Cryptography.BCryptNative
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Security.Cryptography
{
  internal static class BCryptNative
  {
    private static volatile bool s_haveBcryptSupported;
    private static volatile bool s_bcryptSupported;

    internal static bool BCryptSupported
    {
      [SecuritySafeCritical] get
      {
        if (!BCryptNative.s_haveBcryptSupported)
        {
          using (Microsoft.Win32.SafeLibraryHandle safeLibraryHandle = Microsoft.Win32.UnsafeNativeMethods.LoadLibraryEx("bcrypt", IntPtr.Zero, 0))
          {
            BCryptNative.s_bcryptSupported = !safeLibraryHandle.IsInvalid;
            BCryptNative.s_haveBcryptSupported = true;
          }
        }
        return BCryptNative.s_bcryptSupported;
      }
    }

    [SecurityCritical]
    internal static int GetInt32Property<T>(T algorithm, string property) where T : SafeHandle
    {
      return BitConverter.ToInt32(BCryptNative.GetProperty<T>(algorithm, property), 0);
    }

    [SecurityCritical]
    internal static byte[] GetProperty<T>(T algorithm, string property) where T : SafeHandle
    {
      BCryptNative.BCryptPropertyGetter<T> bcryptPropertyGetter = (BCryptNative.BCryptPropertyGetter<T>) null;
      if (typeof (T) == typeof (SafeBCryptAlgorithmHandle))
        bcryptPropertyGetter = new BCryptNative.BCryptPropertyGetter<SafeBCryptAlgorithmHandle>(BCryptNative.UnsafeNativeMethods.BCryptGetAlgorithmProperty) as BCryptNative.BCryptPropertyGetter<T>;
      else if (typeof (T) == typeof (SafeBCryptHashHandle))
        bcryptPropertyGetter = new BCryptNative.BCryptPropertyGetter<SafeBCryptHashHandle>(BCryptNative.UnsafeNativeMethods.BCryptGetHashProperty) as BCryptNative.BCryptPropertyGetter<T>;
      int pcbResult = 0;
      BCryptNative.ErrorCode errorCode1 = bcryptPropertyGetter(algorithm, property, (byte[]) null, 0, ref pcbResult, 0);
      switch (errorCode1)
      {
        case BCryptNative.ErrorCode.BufferToSmall:
        case BCryptNative.ErrorCode.Success:
          byte[] pbOutput = new byte[pcbResult];
          BCryptNative.ErrorCode errorCode2 = bcryptPropertyGetter(algorithm, property, pbOutput, pbOutput.Length, ref pcbResult, 0);
          if (errorCode2 != BCryptNative.ErrorCode.Success)
            throw new CryptographicException((int) errorCode2);
          else
            return pbOutput;
        default:
          throw new CryptographicException((int) errorCode1);
      }
    }

    internal static void MapAlgorithmIdToMagic(string algorithm, out BCryptNative.KeyBlobMagicNumber algorithmMagic, out int keySize)
    {
      switch (algorithm)
      {
        case "ECDH_P256":
          algorithmMagic = BCryptNative.KeyBlobMagicNumber.ECDHPublicP256;
          keySize = 256;
          break;
        case "ECDH_P384":
          algorithmMagic = BCryptNative.KeyBlobMagicNumber.ECDHPublicP384;
          keySize = 384;
          break;
        case "ECDH_P521":
          algorithmMagic = BCryptNative.KeyBlobMagicNumber.ECDHPublicP521;
          keySize = 521;
          break;
        case "ECDSA_P256":
          algorithmMagic = BCryptNative.KeyBlobMagicNumber.ECDsaPublicP256;
          keySize = 256;
          break;
        case "ECDSA_P384":
          algorithmMagic = BCryptNative.KeyBlobMagicNumber.ECDsaPublicP384;
          keySize = 384;
          break;
        case "ECDSA_P521":
          algorithmMagic = BCryptNative.KeyBlobMagicNumber.ECDsaPublicP521;
          keySize = 521;
          break;
        default:
          throw new ArgumentException(SR.GetString("Cryptography_UnknownEllipticCurveAlgorithm"));
      }
    }

    [SecurityCritical]
    internal static SafeBCryptAlgorithmHandle OpenAlgorithm(string algorithm, string implementation)
    {
      SafeBCryptAlgorithmHandle phAlgorithm = (SafeBCryptAlgorithmHandle) null;
      BCryptNative.ErrorCode errorCode = BCryptNative.UnsafeNativeMethods.BCryptOpenAlgorithmProvider(out phAlgorithm, algorithm, implementation, 0);
      if (errorCode != BCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      else
        return phAlgorithm;
    }

    internal enum ErrorCode
    {
      BufferToSmall = -1073741789,
      ObjectNameNotFound = -1073741772,
      Success = 0,
    }

    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical(SecurityCriticalScope.Everything)]
    internal static class UnsafeNativeMethods
    {
      [DllImport("bcrypt.dll", EntryPoint = "BCryptGetProperty", CharSet = CharSet.Unicode)]
      internal static BCryptNative.ErrorCode BCryptGetAlgorithmProperty(SafeBCryptAlgorithmHandle hObject, string pszProperty, [MarshalAs(UnmanagedType.LPArray), In, Out] byte[] pbOutput, int cbOutput, [In, Out] ref int pcbResult, int flags);

      [DllImport("bcrypt.dll", EntryPoint = "BCryptGetProperty", CharSet = CharSet.Unicode)]
      internal static BCryptNative.ErrorCode BCryptGetHashProperty(SafeBCryptHashHandle hObject, string pszProperty, [MarshalAs(UnmanagedType.LPArray), In, Out] byte[] pbOutput, int cbOutput, [In, Out] ref int pcbResult, int flags);

      [DllImport("bcrypt.dll", CharSet = CharSet.Unicode)]
      internal static BCryptNative.ErrorCode BCryptCreateHash(SafeBCryptAlgorithmHandle hAlgorithm, out SafeBCryptHashHandle phHash, IntPtr pbHashObject, int cbHashObject, IntPtr pbSecret, int cbSecret, int dwFlags);

      [DllImport("bcrypt.dll", CharSet = CharSet.Unicode)]
      internal static BCryptNative.ErrorCode BCryptGetProperty(SafeBCryptAlgorithmHandle hObject, string pszProperty, [MarshalAs(UnmanagedType.LPArray), In, Out] byte[] pbOutput, int cbOutput, [In, Out] ref int pcbResult, int flags);

      [DllImport("bcrypt.dll")]
      internal static BCryptNative.ErrorCode BCryptFinishHash(SafeBCryptHashHandle hHash, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbInput, int cbInput, int dwFlags);

      [DllImport("bcrypt.dll")]
      internal static BCryptNative.ErrorCode BCryptHashData(SafeBCryptHashHandle hHash, [MarshalAs(UnmanagedType.LPArray), In] byte[] pbInput, int cbInput, int dwFlags);

      [DllImport("bcrypt.dll", CharSet = CharSet.Unicode)]
      internal static BCryptNative.ErrorCode BCryptOpenAlgorithmProvider(out SafeBCryptAlgorithmHandle phAlgorithm, string pszAlgId, string pszImplementation, int dwFlags);
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    private delegate BCryptNative.ErrorCode BCryptPropertyGetter<T>(T hObject, string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags) where T : SafeHandle;

    internal static class AlgorithmName
    {
      public const string ECDHP256 = "ECDH_P256";
      public const string ECDHP384 = "ECDH_P384";
      public const string ECDHP521 = "ECDH_P521";
      public const string ECDsaP256 = "ECDSA_P256";
      public const string ECDsaP384 = "ECDSA_P384";
      public const string ECDsaP521 = "ECDSA_P521";
      public const string MD5 = "MD5";
      public const string Sha1 = "SHA1";
      public const string Sha256 = "SHA256";
      public const string Sha384 = "SHA384";
      public const string Sha512 = "SHA512";
    }

    internal static class HashPropertyName
    {
      public const string HashLength = "HashDigestLength";
    }

    internal enum KeyBlobMagicNumber
    {
      ECDHPublicP256 = 827016005,
      ECDsaPublicP256 = 827540293,
      ECDHPublicP384 = 860570437,
      ECDsaPublicP384 = 861094725,
      ECDHPublicP521 = 894124869,
      ECDsaPublicP521 = 894649157,
    }

    internal static class KeyDerivationFunction
    {
      public const string Hash = "HASH";
      public const string Hmac = "HMAC";
      public const string Tls = "TLS_PRF";
    }

    internal static class ProviderName
    {
      public const string MicrosoftPrimitiveProvider = "Microsoft Primitive Provider";
    }

    internal static class ObjectPropertyName
    {
      public const string ObjectLength = "ObjectLength";
    }
  }
}
