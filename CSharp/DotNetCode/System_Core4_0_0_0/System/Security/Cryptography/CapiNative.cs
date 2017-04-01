// Type: System.Security.Cryptography.CapiNative
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Security.Cryptography
{
  internal static class CapiNative
  {
    [SecurityCritical]
    internal static T GetProviderParameterStruct<T>(SafeCspHandle provider, CapiNative.ProviderParameter parameter, CapiNative.ProviderParameterFlags flags) where T : struct
    {
      int pdwDataLen = 0;
      IntPtr num = IntPtr.Zero;
      if (!CapiNative.UnsafeNativeMethods.CryptGetProvParam(provider, parameter, num, out pdwDataLen, flags))
      {
        int lastWin32Error = Marshal.GetLastWin32Error();
        switch (lastWin32Error)
        {
          case 259:
            return default (T);
          case 234:
            break;
          default:
            throw new CryptographicException(lastWin32Error);
        }
      }
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
        }
        finally
        {
          num = Marshal.AllocCoTaskMem(pdwDataLen);
        }
        if (!CapiNative.UnsafeNativeMethods.CryptGetProvParam(provider, parameter, num, out pdwDataLen, flags))
          throw new CryptographicException(Marshal.GetLastWin32Error());
        else
          return (T) Marshal.PtrToStructure(num, typeof (T));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeCoTaskMem(num);
      }
    }

    [SecurityCritical]
    internal static SafeCspHandle AcquireCsp(string keyContainer, string providerName, CapiNative.ProviderType providerType, CapiNative.CryptAcquireContextFlags flags, bool throwPlatformException)
    {
      SafeCspHandle phProv = (SafeCspHandle) null;
      if (CapiNative.UnsafeNativeMethods.CryptAcquireContext(out phProv, keyContainer, providerName, providerType, flags))
        return phProv;
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (throwPlatformException && (lastWin32Error == -2146893801 || lastWin32Error == -2146893799))
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      else
        throw new CryptographicException(lastWin32Error);
    }

    [SecurityCritical]
    internal static byte[] ExportSymmetricKey(SafeCapiKeyHandle key)
    {
      int pdwDataLen = 0;
      if (!CapiNative.UnsafeNativeMethods.CryptExportKey(key, SafeCapiKeyHandle.InvalidHandle, 8, 0, (byte[]) null, out pdwDataLen))
      {
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (lastWin32Error != 234)
          throw new CryptographicException(lastWin32Error);
      }
      byte[] pbData = new byte[pdwDataLen];
      if (!CapiNative.UnsafeNativeMethods.CryptExportKey(key, SafeCapiKeyHandle.InvalidHandle, 8, 0, pbData, out pdwDataLen))
        throw new CryptographicException(Marshal.GetLastWin32Error());
      int srcOffset = Marshal.SizeOf(typeof (CapiNative.BLOBHEADER)) + Marshal.SizeOf(typeof (int));
      byte[] numArray = new byte[BitConverter.ToInt32(pbData, Marshal.SizeOf(typeof (CapiNative.BLOBHEADER)))];
      Buffer.BlockCopy((Array) pbData, srcOffset, (Array) numArray, 0, numArray.Length);
      return numArray;
    }

    internal static string GetAlgorithmName(CapiNative.AlgorithmId algorithm)
    {
      return ((object) algorithm).ToString().ToUpper(CultureInfo.InvariantCulture);
    }

    [SecurityCritical]
    internal static byte[] GetHashParameter(SafeCapiHashHandle hashHandle, CapiNative.HashParameter parameter)
    {
      int pdwDataLen = 0;
      if (!CapiNative.UnsafeNativeMethods.CryptGetHashParam(hashHandle, parameter, (byte[]) null, out pdwDataLen, 0))
        throw new CryptographicException(Marshal.GetLastWin32Error());
      byte[] pbData = new byte[pdwDataLen];
      if (!CapiNative.UnsafeNativeMethods.CryptGetHashParam(hashHandle, parameter, pbData, out pdwDataLen, 0))
        throw new CryptographicException(Marshal.GetLastWin32Error());
      if (pdwDataLen != pbData.Length)
      {
        byte[] numArray = new byte[pdwDataLen];
        Buffer.BlockCopy((Array) pbData, 0, (Array) numArray, 0, pdwDataLen);
        pbData = numArray;
      }
      return pbData;
    }

    internal static int HResultForVerificationResult(SignatureVerificationResult verificationResult)
    {
      switch (verificationResult)
      {
        case SignatureVerificationResult.AssemblyIdentityMismatch:
        case SignatureVerificationResult.PublicKeyTokenMismatch:
        case SignatureVerificationResult.PublisherMismatch:
          return -2146762749;
        case SignatureVerificationResult.ContainingSignatureInvalid:
          return -2146869232;
        default:
          return (int) verificationResult;
      }
    }

    [SecurityCritical]
    internal static unsafe SafeCapiKeyHandle ImportSymmetricKey(SafeCspHandle provider, CapiNative.AlgorithmId algorithm, byte[] key)
    {
      byte[] pbData = new byte[Marshal.SizeOf(typeof (CapiNative.BLOBHEADER)) + Marshal.SizeOf(typeof (int)) + key.Length];
      fixed (byte* numPtr = pbData)
      {
        ((CapiNative.BLOBHEADER*) numPtr)->bType = CapiNative.KeyBlobType.PlainText;
        ((CapiNative.BLOBHEADER*) numPtr)->bVersion = (byte) 2;
        ((CapiNative.BLOBHEADER*) numPtr)->reserved = (short) 0;
        ((CapiNative.BLOBHEADER*) numPtr)->aiKeyAlg = algorithm;
        *(int*) (numPtr + Marshal.SizeOf<CapiNative.BLOBHEADER>(*(CapiNative.BLOBHEADER*) numPtr)) = key.Length;
      }
      Buffer.BlockCopy((Array) key, 0, (Array) pbData, Marshal.SizeOf(typeof (CapiNative.BLOBHEADER)) + Marshal.SizeOf(typeof (int)), key.Length);
      SafeCapiKeyHandle phKey = (SafeCapiKeyHandle) null;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        if (!CapiNative.UnsafeNativeMethods.CryptImportKey(provider, pbData, pbData.Length, SafeCapiKeyHandle.InvalidHandle, CapiNative.KeyFlags.Exportable, out phKey))
          throw new CryptographicException(Marshal.GetLastWin32Error());
      }
      finally
      {
        if (phKey != null && !phKey.IsInvalid)
          phKey.SetParentCsp(provider);
      }
      return phKey;
    }

    [SecurityCritical]
    internal static void SetKeyParameter(SafeCapiKeyHandle key, CapiNative.KeyParameter parameter, int value)
    {
      CapiNative.SetKeyParameter(key, parameter, BitConverter.GetBytes(value));
    }

    [SecurityCritical]
    internal static void SetKeyParameter(SafeCapiKeyHandle key, CapiNative.KeyParameter parameter, byte[] value)
    {
      if (!CapiNative.UnsafeNativeMethods.CryptSetKeyParam(key, parameter, value, 0))
        throw new CryptographicException(Marshal.GetLastWin32Error());
    }

    internal enum AlgorithmId
    {
      None = 0,
      Aes128 = 26126,
      Aes192 = 26127,
      Aes256 = 26128,
      MD5 = 32771,
      Sha1 = 32772,
      Sha256 = 32780,
      Sha384 = 32781,
      Sha512 = 32782,
    }

    internal enum ProviderParameter
    {
      None,
      EnumerateAlgorithms,
    }

    [Flags]
    internal enum ProviderParameterFlags
    {
      None = 0,
      RestartEnumeration = 1,
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
      [SuppressUnmanagedCodeSecurity]
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("advapi32")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptDuplicateKey(SafeCapiKeyHandle hKey, IntPtr pdwReserved, int dwFlags, out SafeCapiKeyHandle phKey);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptGetProvParam(SafeCspHandle hProv, CapiNative.ProviderParameter dwParam, IntPtr pbData, [In, Out] ref int pdwDataLen, CapiNative.ProviderParameterFlags dwFlags);

      [DllImport("clr")]
      public static int _AxlPublicKeyBlobToPublicKeyToken(ref CapiNative.CRYPTOAPI_BLOB pCspPublicKeyBlob, out SafeAxlBufferHandle ppwszPublicKeyToken);

      [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptAcquireContext(out SafeCspHandle phProv, string pszContainer, string pszProvider, CapiNative.ProviderType dwProvType, CapiNative.CryptAcquireContextFlags dwFlags);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptCreateHash(SafeCspHandle hProv, CapiNative.AlgorithmId Algid, SafeCapiKeyHandle hKey, int dwFlags, out SafeCapiHashHandle phHash);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptDecrypt(SafeCapiKeyHandle hKey, SafeCapiHashHandle hHash, [MarshalAs(UnmanagedType.Bool)] bool Final, int dwFlags, IntPtr pbData, [In, Out] ref int pdwDataLen);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptEncrypt(SafeCapiKeyHandle hKey, SafeCapiHashHandle hHash, [MarshalAs(UnmanagedType.Bool)] bool Final, int dwFlags, IntPtr pbData, [In, Out] ref int pdwDataLen, int dwBufLen);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptExportKey(SafeCapiKeyHandle hKey, SafeCapiKeyHandle hExpKey, int dwBlobType, int dwExportFlags, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbData, [In, Out] ref int pdwDataLen);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptGenKey(SafeCspHandle hProv, CapiNative.AlgorithmId Algid, CapiNative.KeyFlags dwFlags, out SafeCapiKeyHandle phKey);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptGenRandom(SafeCspHandle hProv, int dwLen, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbBuffer);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptGetHashParam(SafeCapiHashHandle hHash, CapiNative.HashParameter dwParam, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbData, [In, Out] ref int pdwDataLen, int dwFlags);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptHashData(SafeCapiHashHandle hHash, [MarshalAs(UnmanagedType.LPArray)] byte[] pbData, int dwDataLen, int dwFlags);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptImportKey(SafeCspHandle hProv, [MarshalAs(UnmanagedType.LPArray)] byte[] pbData, int dwDataLen, SafeCapiKeyHandle hPubKey, CapiNative.KeyFlags dwFlags, out SafeCapiKeyHandle phKey);

      [DllImport("advapi32", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static bool CryptSetKeyParam(SafeCapiKeyHandle hKey, CapiNative.KeyParameter dwParam, [MarshalAs(UnmanagedType.LPArray)] byte[] pbData, int dwFlags);
    }

    internal enum AlgorithmClass
    {
      DataEncryption = 24576,
      Hash = 32768,
    }

    internal enum AlgorithmType
    {
      Any = 0,
      Block = 1536,
    }

    internal enum AlgorithmSubId
    {
      MD5 = 3,
      Sha1 = 4,
      Sha256 = 12,
      Sha384 = 13,
      Aes128 = 14,
      Sha512 = 14,
      Aes192 = 15,
      Aes256 = 16,
    }

    [Flags]
    internal enum CryptAcquireContextFlags
    {
      None = 0,
      VerifyContext = -268435456,
    }

    internal enum ErrorCode
    {
      BadData = -2146893819,
      BadAlgorithmId = -2146893816,
      ProviderTypeNotDefined = -2146893801,
      KeysetNotDefined = -2146893799,
      Success = 0,
      MoreData = 234,
      NoMoreItems = 259,
    }

    internal enum HashParameter
    {
      None = 0,
      AlgorithmId = 1,
      HashValue = 2,
      HashSize = 4,
    }

    internal enum KeyBlobType : byte
    {
      PlainText = (byte) 8,
    }

    [Flags]
    internal enum KeyFlags
    {
      None = 0,
      Exportable = 1,
    }

    internal enum KeyParameter
    {
      None = 0,
      IV = 1,
      Mode = 4,
      ModeBits = 5,
    }

    internal static class ProviderNames
    {
      public const string MicrosoftEnhancedRsaAes = "Microsoft Enhanced RSA and AES Cryptographic Provider";
      public const string MicrosoftEnhancedRsaAesPrototype = "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)";
    }

    internal enum ProviderType
    {
      None = 0,
      RsaAes = 24,
    }

    internal struct BLOBHEADER
    {
      public CapiNative.KeyBlobType bType;
      public byte bVersion;
      public short reserved;
      public CapiNative.AlgorithmId aiKeyAlg;
    }

    internal struct CRYPTOAPI_BLOB
    {
      public int cbData;
      public IntPtr pbData;
    }

    internal struct PROV_ENUMALGS
    {
      public CapiNative.AlgorithmId aiAlgId;
      public int dwBitLen;
      public int dwNameLen;
      public unsafe fixed byte szName[20];
    }
  }
}
