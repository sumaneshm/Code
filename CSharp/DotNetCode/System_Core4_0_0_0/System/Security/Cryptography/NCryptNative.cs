// Type: System.Security.Cryptography.NCryptNative
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Security.Cryptography
{
  internal static class NCryptNative
  {
    private static volatile bool s_haveNcryptSupported;
    private static volatile bool s_ncryptSupported;

    internal static bool NCryptSupported
    {
      [SecuritySafeCritical] get
      {
        if (!NCryptNative.s_haveNcryptSupported)
        {
          using (Microsoft.Win32.SafeLibraryHandle safeLibraryHandle = Microsoft.Win32.UnsafeNativeMethods.LoadLibraryEx("ncrypt", IntPtr.Zero, 0))
          {
            NCryptNative.s_ncryptSupported = !safeLibraryHandle.IsInvalid;
            NCryptNative.s_haveNcryptSupported = true;
          }
        }
        return NCryptNative.s_ncryptSupported;
      }
    }

    [SecurityCritical]
    internal static byte[] GetProperty(SafeNCryptHandle ncryptObject, string propertyName, CngPropertyOptions propertyOptions, out bool foundProperty)
    {
      int pcbResult = 0;
      NCryptNative.ErrorCode property1 = NCryptNative.UnsafeNativeMethods.NCryptGetProperty(ncryptObject, propertyName, (byte[]) null, 0, out pcbResult, propertyOptions);
      switch (property1)
      {
        case NCryptNative.ErrorCode.Success:
        case NCryptNative.ErrorCode.BufferTooSmall:
        case NCryptNative.ErrorCode.NotFound:
          foundProperty = property1 != NCryptNative.ErrorCode.NotFound;
          byte[] pbOutput = (byte[]) null;
          if (property1 != NCryptNative.ErrorCode.NotFound && pcbResult > 0)
          {
            pbOutput = new byte[pcbResult];
            NCryptNative.ErrorCode property2 = NCryptNative.UnsafeNativeMethods.NCryptGetProperty(ncryptObject, propertyName, pbOutput, pbOutput.Length, out pcbResult, propertyOptions);
            if (property2 != NCryptNative.ErrorCode.Success)
              throw new CryptographicException((int) property2);
            foundProperty = true;
          }
          return pbOutput;
        default:
          throw new CryptographicException((int) property1);
      }
    }

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    internal static IntPtr GetPropertyAsIntPtr(SafeNCryptHandle ncryptObject, string propertyName, CngPropertyOptions propertyOptions)
    {
      int pcbResult = IntPtr.Size;
      IntPtr pbOutput = IntPtr.Zero;
      NCryptNative.ErrorCode property = NCryptNative.UnsafeNativeMethods.NCryptGetProperty(ncryptObject, propertyName, out pbOutput, IntPtr.Size, out pcbResult, propertyOptions);
      switch (property)
      {
        case NCryptNative.ErrorCode.NotFound:
          return IntPtr.Zero;
        case NCryptNative.ErrorCode.Success:
          return pbOutput;
        default:
          throw new CryptographicException((int) property);
      }
    }

    [SecurityCritical]
    internal static unsafe T GetPropertyAsStruct<T>(SafeNCryptHandle ncryptObject, string propertyName, CngPropertyOptions propertyOptions) where T : struct
    {
      bool foundProperty;
      byte[] property = NCryptNative.GetProperty(ncryptObject, propertyName, propertyOptions, out foundProperty);
      if (!foundProperty || property == null)
        return default (T);
      fixed (byte* numPtr = property)
        return (T) Marshal.PtrToStructure(new IntPtr((void*) numPtr), typeof (T));
    }

    [SecurityCritical]
    internal static unsafe void SetProperty<T>(SafeNCryptHandle ncryptObject, string propertyName, T value, CngPropertyOptions propertyOptions) where T : struct
    {
      byte[] numArray = new byte[Marshal.SizeOf(typeof (T))];
      fixed (byte* numPtr = numArray)
      {
        bool flag = false;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          RuntimeHelpers.PrepareConstrainedRegions();
          try
          {
          }
          finally
          {
            Marshal.StructureToPtr<T>(value, new IntPtr((void*) numPtr), false);
            flag = true;
          }
          NCryptNative.SetProperty(ncryptObject, propertyName, numArray, propertyOptions);
        }
        finally
        {
          if (flag)
            Marshal.DestroyStructure(new IntPtr((void*) numPtr), typeof (T));
        }
      }
    }

    [SecurityCritical]
    internal static void SetProperty(SafeNCryptHandle ncryptObject, string propertyName, byte[] value, CngPropertyOptions propertyOptions)
    {
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptSetProperty(ncryptObject, propertyName, value, value != null ? value.Length : 0, propertyOptions);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
    }

    internal static byte[] BuildEccPublicBlob(string algorithm, BigInteger x, BigInteger y)
    {
      BCryptNative.KeyBlobMagicNumber algorithmMagic;
      int keySize;
      BCryptNative.MapAlgorithmIdToMagic(algorithm, out algorithmMagic, out keySize);
      byte[] numArray1 = NCryptNative.ReverseBytes(NCryptNative.FillKeyParameter(x.ToByteArray(), keySize));
      byte[] numArray2 = NCryptNative.ReverseBytes(NCryptNative.FillKeyParameter(y.ToByteArray(), keySize));
      byte[] numArray3 = new byte[8 + numArray1.Length + numArray2.Length];
      Buffer.BlockCopy((Array) BitConverter.GetBytes((int) algorithmMagic), 0, (Array) numArray3, 0, 4);
      Buffer.BlockCopy((Array) BitConverter.GetBytes(numArray1.Length), 0, (Array) numArray3, 4, 4);
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray3, 8, numArray1.Length);
      Buffer.BlockCopy((Array) numArray2, 0, (Array) numArray3, 8 + numArray1.Length, numArray2.Length);
      return numArray3;
    }

    [SecurityCritical]
    internal static SafeNCryptKeyHandle CreatePersistedKey(SafeNCryptProviderHandle provider, string algorithm, string name, CngKeyCreationOptions options)
    {
      SafeNCryptKeyHandle phKey = (SafeNCryptKeyHandle) null;
      NCryptNative.ErrorCode persistedKey = NCryptNative.UnsafeNativeMethods.NCryptCreatePersistedKey(provider, out phKey, algorithm, name, 0, options);
      if (persistedKey != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) persistedKey);
      else
        return phKey;
    }

    [SecurityCritical]
    internal static void DeleteKey(SafeNCryptKeyHandle key)
    {
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptDeleteKey(key, 0);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      key.Dispose();
    }

    [SecurityCritical]
    private static unsafe byte[] DeriveKeyMaterial(SafeNCryptSecretHandle secretAgreement, string kdf, string hashAlgorithm, byte[] hmacKey, byte[] secretPrepend, byte[] secretAppend, NCryptNative.SecretAgreementFlags flags)
    {
      List<NCryptNative.NCryptBuffer> list = new List<NCryptNative.NCryptBuffer>();
      IntPtr ptr = IntPtr.Zero;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
        }
        finally
        {
          ptr = Marshal.StringToCoTaskMemUni(hashAlgorithm);
        }
        list.Add(new NCryptNative.NCryptBuffer()
        {
          cbBuffer = (hashAlgorithm.Length + 1) * 2,
          BufferType = NCryptNative.BufferType.KdfHashAlgorithm,
          pvBuffer = ptr
        });
        fixed (byte* numPtr1 = hmacKey)
          fixed (byte* numPtr2 = secretPrepend)
            fixed (byte* numPtr3 = secretAppend)
            {
              if ((IntPtr) numPtr1 != IntPtr.Zero)
                list.Add(new NCryptNative.NCryptBuffer()
                {
                  cbBuffer = hmacKey.Length,
                  BufferType = NCryptNative.BufferType.KdfHmacKey,
                  pvBuffer = new IntPtr((void*) numPtr1)
                });
              if ((IntPtr) numPtr2 != IntPtr.Zero)
                list.Add(new NCryptNative.NCryptBuffer()
                {
                  cbBuffer = secretPrepend.Length,
                  BufferType = NCryptNative.BufferType.KdfSecretPrepend,
                  pvBuffer = new IntPtr((void*) numPtr2)
                });
              if ((IntPtr) numPtr3 != IntPtr.Zero)
                list.Add(new NCryptNative.NCryptBuffer()
                {
                  cbBuffer = secretAppend.Length,
                  BufferType = NCryptNative.BufferType.KdfSecretAppend,
                  pvBuffer = new IntPtr((void*) numPtr3)
                });
              return NCryptNative.DeriveKeyMaterial(secretAgreement, kdf, list.ToArray(), flags);
            }
      }
      finally
      {
        if (ptr != IntPtr.Zero)
          Marshal.FreeCoTaskMem(ptr);
      }
    }

    [SecurityCritical]
    private static unsafe byte[] DeriveKeyMaterial(SafeNCryptSecretHandle secretAgreement, string kdf, NCryptNative.NCryptBuffer[] parameters, NCryptNative.SecretAgreementFlags flags)
    {
      fixed (NCryptNative.NCryptBuffer* ncryptBufferPtr = parameters)
      {
        NCryptNative.NCryptBufferDesc pParameterList = new NCryptNative.NCryptBufferDesc();
        pParameterList.ulVersion = 0;
        pParameterList.cBuffers = parameters.Length;
        pParameterList.pBuffers = new IntPtr((void*) ncryptBufferPtr);
        int pcbResult = 0;
        NCryptNative.ErrorCode errorCode1 = NCryptNative.UnsafeNativeMethods.NCryptDeriveKey(secretAgreement, kdf, ref pParameterList, (byte[]) null, 0, out pcbResult, flags);
        switch (errorCode1)
        {
          case NCryptNative.ErrorCode.Success:
          case NCryptNative.ErrorCode.BufferTooSmall:
            byte[] pbDerivedKey = new byte[pcbResult];
            NCryptNative.ErrorCode errorCode2 = NCryptNative.UnsafeNativeMethods.NCryptDeriveKey(secretAgreement, kdf, ref pParameterList, pbDerivedKey, pbDerivedKey.Length, out pcbResult, flags);
            if (errorCode2 != NCryptNative.ErrorCode.Success)
              throw new CryptographicException((int) errorCode2);
            else
              return pbDerivedKey;
          default:
            throw new CryptographicException((int) errorCode1);
        }
      }
    }

    [SecurityCritical]
    internal static byte[] DeriveKeyMaterialHash(SafeNCryptSecretHandle secretAgreement, string hashAlgorithm, byte[] secretPrepend, byte[] secretAppend, NCryptNative.SecretAgreementFlags flags)
    {
      return NCryptNative.DeriveKeyMaterial(secretAgreement, "HASH", hashAlgorithm, (byte[]) null, secretPrepend, secretAppend, flags);
    }

    [SecurityCritical]
    internal static byte[] DeriveKeyMaterialHmac(SafeNCryptSecretHandle secretAgreement, string hashAlgorithm, byte[] hmacKey, byte[] secretPrepend, byte[] secretAppend, NCryptNative.SecretAgreementFlags flags)
    {
      return NCryptNative.DeriveKeyMaterial(secretAgreement, "HMAC", hashAlgorithm, hmacKey, secretPrepend, secretAppend, flags);
    }

    [SecurityCritical]
    internal static unsafe byte[] DeriveKeyMaterialTls(SafeNCryptSecretHandle secretAgreement, byte[] label, byte[] seed, NCryptNative.SecretAgreementFlags flags)
    {
      NCryptNative.NCryptBuffer[] parameters = new NCryptNative.NCryptBuffer[2];
      fixed (byte* numPtr1 = label)
        fixed (byte* numPtr2 = seed)
        {
          parameters[0] = new NCryptNative.NCryptBuffer()
          {
            cbBuffer = label.Length,
            BufferType = NCryptNative.BufferType.KdfTlsLabel,
            pvBuffer = new IntPtr((void*) numPtr1)
          };
          parameters[1] = new NCryptNative.NCryptBuffer()
          {
            cbBuffer = seed.Length,
            BufferType = NCryptNative.BufferType.KdfTlsSeed,
            pvBuffer = new IntPtr((void*) numPtr2)
          };
          return NCryptNative.DeriveKeyMaterial(secretAgreement, "TLS_PRF", parameters, flags);
        }
    }

    [SecurityCritical]
    internal static SafeNCryptSecretHandle DeriveSecretAgreement(SafeNCryptKeyHandle privateKey, SafeNCryptKeyHandle otherPartyPublicKey)
    {
      SafeNCryptSecretHandle phSecret;
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptSecretAgreement(privateKey, otherPartyPublicKey, out phSecret, 0);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      else
        return phSecret;
    }

    [SecurityCritical]
    internal static byte[] ExportKey(SafeNCryptKeyHandle key, string format)
    {
      int pcbResult = 0;
      NCryptNative.ErrorCode errorCode1 = NCryptNative.UnsafeNativeMethods.NCryptExportKey(key, IntPtr.Zero, format, IntPtr.Zero, (byte[]) null, 0, out pcbResult, 0);
      switch (errorCode1)
      {
        case NCryptNative.ErrorCode.Success:
        case NCryptNative.ErrorCode.BufferTooSmall:
          byte[] pbOutput = new byte[pcbResult];
          NCryptNative.ErrorCode errorCode2 = NCryptNative.UnsafeNativeMethods.NCryptExportKey(key, IntPtr.Zero, format, IntPtr.Zero, pbOutput, pbOutput.Length, out pcbResult, 0);
          if (errorCode2 != NCryptNative.ErrorCode.Success)
            throw new CryptographicException((int) errorCode2);
          else
            return pbOutput;
        default:
          throw new CryptographicException((int) errorCode1);
      }
    }

    private static byte[] FillKeyParameter(byte[] key, int keySize)
    {
      int length = keySize / 8 + (keySize % 8 == 0 ? 0 : 1);
      if (key.Length == length)
        return key;
      byte[] numArray = new byte[length];
      Buffer.BlockCopy((Array) key, 0, (Array) numArray, 0, Math.Min(key.Length, numArray.Length));
      return numArray;
    }

    [SecurityCritical]
    internal static void FinalizeKey(SafeNCryptKeyHandle key)
    {
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptFinalizeKey(key, 0);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
    }

    [SecurityCritical]
    internal static int GetPropertyAsDWord(SafeNCryptHandle ncryptObject, string propertyName, CngPropertyOptions propertyOptions)
    {
      bool foundProperty;
      byte[] property = NCryptNative.GetProperty(ncryptObject, propertyName, propertyOptions, out foundProperty);
      if (!foundProperty || property == null)
        return 0;
      else
        return BitConverter.ToInt32(property, 0);
    }

    [SecurityCritical]
    internal static unsafe string GetPropertyAsString(SafeNCryptHandle ncryptObject, string propertyName, CngPropertyOptions propertyOptions)
    {
      bool foundProperty;
      byte[] property = NCryptNative.GetProperty(ncryptObject, propertyName, propertyOptions, out foundProperty);
      if (!foundProperty || property == null)
        return (string) null;
      if (property.Length == 0)
        return string.Empty;
      fixed (byte* numPtr = property)
        return Marshal.PtrToStringUni(new IntPtr((void*) numPtr));
    }

    [SecurityCritical]
    internal static SafeNCryptKeyHandle ImportKey(SafeNCryptProviderHandle provider, byte[] keyBlob, string format)
    {
      SafeNCryptKeyHandle phKey = (SafeNCryptKeyHandle) null;
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptImportKey(provider, IntPtr.Zero, format, IntPtr.Zero, out phKey, keyBlob, keyBlob.Length, 0);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      else
        return phKey;
    }

    [SecurityCritical]
    internal static SafeNCryptKeyHandle OpenKey(SafeNCryptProviderHandle provider, string name, CngKeyOpenOptions options)
    {
      SafeNCryptKeyHandle phKey = (SafeNCryptKeyHandle) null;
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptOpenKey(provider, out phKey, name, 0, options);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      else
        return phKey;
    }

    [SecurityCritical]
    internal static SafeNCryptProviderHandle OpenStorageProvider(string providerName)
    {
      SafeNCryptProviderHandle phProvider = (SafeNCryptProviderHandle) null;
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptOpenStorageProvider(out phProvider, providerName, 0);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
      else
        return phProvider;
    }

    private static byte[] ReverseBytes(byte[] buffer)
    {
      return NCryptNative.ReverseBytes(buffer, 0, buffer.Length, false);
    }

    private static byte[] ReverseBytes(byte[] buffer, int offset, int count)
    {
      return NCryptNative.ReverseBytes(buffer, offset, count, false);
    }

    private static byte[] ReverseBytes(byte[] buffer, int offset, int count, bool padWithZeroByte)
    {
      byte[] numArray = !padWithZeroByte ? new byte[count] : new byte[count + 1];
      int num = offset + count - 1;
      for (int index = 0; index < count; ++index)
        numArray[index] = buffer[num - index];
      return numArray;
    }

    [SecurityCritical]
    internal static void SetProperty(SafeNCryptHandle ncryptObject, string propertyName, int value, CngPropertyOptions propertyOptions)
    {
      NCryptNative.SetProperty(ncryptObject, propertyName, BitConverter.GetBytes(value), propertyOptions);
    }

    [SecurityCritical]
    internal static void SetProperty(SafeNCryptHandle ncryptObject, string propertyName, string value, CngPropertyOptions propertyOptions)
    {
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptSetProperty(ncryptObject, propertyName, value, (value.Length + 1) * 2, propertyOptions);
      if (errorCode != NCryptNative.ErrorCode.Success)
        throw new CryptographicException((int) errorCode);
    }

    [SecurityCritical]
    internal static byte[] SignHash(SafeNCryptKeyHandle key, byte[] hash)
    {
      int pcbResult = 0;
      NCryptNative.ErrorCode errorCode1 = NCryptNative.UnsafeNativeMethods.NCryptSignHash(key, IntPtr.Zero, hash, hash.Length, (byte[]) null, 0, out pcbResult, 0);
      switch (errorCode1)
      {
        case NCryptNative.ErrorCode.Success:
        case NCryptNative.ErrorCode.BufferTooSmall:
          byte[] pbSignature = new byte[pcbResult];
          NCryptNative.ErrorCode errorCode2 = NCryptNative.UnsafeNativeMethods.NCryptSignHash(key, IntPtr.Zero, hash, hash.Length, pbSignature, pbSignature.Length, out pcbResult, 0);
          if (errorCode2 != NCryptNative.ErrorCode.Success)
            throw new CryptographicException((int) errorCode2);
          else
            return pbSignature;
        default:
          throw new CryptographicException((int) errorCode1);
      }
    }

    internal static void UnpackEccPublicBlob(byte[] blob, out BigInteger x, out BigInteger y)
    {
      int count = BitConverter.ToInt32(blob, 4);
      x = new BigInteger(NCryptNative.ReverseBytes(blob, 8, count, true));
      y = new BigInteger(NCryptNative.ReverseBytes(blob, 8 + count, count, true));
    }

    [SecurityCritical]
    internal static bool VerifySignature(SafeNCryptKeyHandle key, byte[] hash, byte[] signature)
    {
      NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptVerifySignature(key, IntPtr.Zero, hash, hash.Length, signature, signature.Length, 0);
      switch (errorCode)
      {
        case NCryptNative.ErrorCode.Success:
        case NCryptNative.ErrorCode.BadSignature:
          return errorCode == NCryptNative.ErrorCode.Success;
        default:
          throw new CryptographicException((int) errorCode);
      }
    }

    internal enum ErrorCode
    {
      BadSignature = -2146893818,
      NotFound = -2146893807,
      KeyDoesNotExist = -2146893802,
      BufferTooSmall = -2146893784,
      Success = 0,
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptGetProperty(SafeNCryptHandle hObject, string pszProperty, out IntPtr pbOutput, int cbOutput, out int pcbResult, CngPropertyOptions dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptCreatePersistedKey(SafeNCryptProviderHandle hProvider, out SafeNCryptKeyHandle phKey, string pszAlgId, string pszKeyName, int dwLegacyKeySpec, CngKeyCreationOptions dwFlags);

      [DllImport("ncrypt.dll")]
      internal static NCryptNative.ErrorCode NCryptDeleteKey(SafeNCryptKeyHandle hKey, int flags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptDeriveKey(SafeNCryptSecretHandle hSharedSecret, string pwszKDF, [In] ref NCryptNative.NCryptBufferDesc pParameterList, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbDerivedKey, int cbDerivedKey, out int pcbResult, NCryptNative.SecretAgreementFlags dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptExportKey(SafeNCryptKeyHandle hKey, IntPtr hExportKey, string pszBlobType, IntPtr pParameterList, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbOutput, int cbOutput, out int pcbResult, int dwFlags);

      [DllImport("ncrypt.dll")]
      internal static NCryptNative.ErrorCode NCryptFinalizeKey(SafeNCryptKeyHandle hKey, int dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptGetProperty(SafeNCryptHandle hObject, string pszProperty, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pbOutput, int cbOutput, out int pcbResult, CngPropertyOptions dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptImportKey(SafeNCryptProviderHandle hProvider, IntPtr hImportKey, string pszBlobType, IntPtr pParameterList, out SafeNCryptKeyHandle phKey, [MarshalAs(UnmanagedType.LPArray)] byte[] pbData, int cbData, int dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptOpenKey(SafeNCryptProviderHandle hProvider, out SafeNCryptKeyHandle phKey, string pszKeyName, int dwLegacyKeySpec, CngKeyOpenOptions dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptOpenStorageProvider(out SafeNCryptProviderHandle phProvider, string pszProviderName, int dwFlags);

      [DllImport("ncrypt.dll")]
      internal static NCryptNative.ErrorCode NCryptSecretAgreement(SafeNCryptKeyHandle hPrivKey, SafeNCryptKeyHandle hPubKey, out SafeNCryptSecretHandle phSecret, int dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptSetProperty(SafeNCryptHandle hObject, string pszProperty, [MarshalAs(UnmanagedType.LPArray)] byte[] pbInput, int cbInput, CngPropertyOptions dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptSetProperty(SafeNCryptHandle hObject, string pszProperty, string pbInput, int cbInput, CngPropertyOptions dwFlags);

      [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
      internal static NCryptNative.ErrorCode NCryptSetProperty(SafeNCryptHandle hObject, string pszProperty, IntPtr pbInput, int cbInput, CngPropertyOptions dwFlags);

      [DllImport("ncrypt.dll")]
      internal static NCryptNative.ErrorCode NCryptSignHash(SafeNCryptKeyHandle hKey, IntPtr pPaddingInfo, [MarshalAs(UnmanagedType.LPArray)] byte[] pbHashValue, int cbHashValue, [MarshalAs(UnmanagedType.LPArray)] byte[] pbSignature, int cbSignature, out int pcbResult, int dwFlags);

      [DllImport("ncrypt.dll")]
      internal static NCryptNative.ErrorCode NCryptVerifySignature(SafeNCryptKeyHandle hKey, IntPtr pPaddingInfo, [MarshalAs(UnmanagedType.LPArray)] byte[] pbHashValue, int cbHashValue, [MarshalAs(UnmanagedType.LPArray)] byte[] pbSignature, int cbSignature, int dwFlags);
    }

    internal enum BufferType
    {
      KdfHashAlgorithm,
      KdfSecretPrepend,
      KdfSecretAppend,
      KdfHmacKey,
      KdfTlsLabel,
      KdfTlsSeed,
    }

    internal static class KeyPropertyName
    {
      internal const string Algorithm = "Algorithm Name";
      internal const string AlgorithmGroup = "Algorithm Group";
      internal const string ExportPolicy = "Export Policy";
      internal const string KeyType = "Key Type";
      internal const string KeyUsage = "Key Usage";
      internal const string Length = "Length";
      internal const string Name = "Name";
      internal const string ParentWindowHandle = "HWND Handle";
      internal const string ProviderHandle = "Provider Handle";
      internal const string UIPolicy = "UI Policy";
      internal const string UniqueName = "Unique Name";
      internal const string UseContext = "Use Context";
      internal const string ClrIsEphemeral = "CLR IsEphemeral";
    }

    internal static class ProviderPropertyName
    {
      internal const string Name = "Name";
    }

    [Flags]
    internal enum SecretAgreementFlags
    {
      None = 0,
      UseSecretAsHmacKey = 1,
    }

    internal struct NCRYPT_UI_POLICY
    {
      public int dwVersion;
      public CngUIProtectionLevels dwFlags;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pszCreationTitle;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pszFriendlyName;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pszDescription;
    }

    internal struct NCryptBuffer
    {
      public int cbBuffer;
      public NCryptNative.BufferType BufferType;
      public IntPtr pvBuffer;
    }

    internal struct NCryptBufferDesc
    {
      public int ulVersion;
      public int cBuffers;
      public IntPtr pBuffers;
    }
  }
}
