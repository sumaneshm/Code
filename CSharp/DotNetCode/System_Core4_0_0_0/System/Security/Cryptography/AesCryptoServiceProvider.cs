// Type: System.Security.Cryptography.AesCryptoServiceProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class AesCryptoServiceProvider : Aes
  {
    private static volatile KeySizes[] s_supportedKeySizes;
    private static volatile int s_defaultKeySize;
    [SecurityCritical]
    private SafeCspHandle m_cspHandle;
    [SecurityCritical]
    private SafeCapiKeyHandle m_key;

    public override byte[] Key
    {
      [SecurityCritical] get
      {
        if (this.m_key == null || this.m_key.IsInvalid || this.m_key.IsClosed)
          this.GenerateKey();
        return CapiNative.ExportSymmetricKey(this.m_key);
      }
      [SecurityCritical] set
      {
        if (value == null)
          throw new ArgumentNullException("value");
        byte[] key = (byte[]) value.Clone();
        if (!this.ValidKeySize(key.Length * 8))
          throw new CryptographicException(SR.GetString("Cryptography_InvalidKeySize"));
        SafeCapiKeyHandle safeCapiKeyHandle = CapiNative.ImportSymmetricKey(this.m_cspHandle, AesCryptoServiceProvider.GetAlgorithmId(key.Length * 8), key);
        if (this.m_key != null)
          this.m_key.Dispose();
        this.m_key = safeCapiKeyHandle;
        this.KeySizeValue = key.Length * 8;
      }
    }

    public override int KeySize
    {
      get
      {
        return base.KeySize;
      }
      [SecurityCritical] set
      {
        base.KeySize = value;
        if (this.m_key == null)
          return;
        this.m_key.Dispose();
      }
    }

    [SecurityCritical]
    public AesCryptoServiceProvider()
    {
      string providerName = "Microsoft Enhanced RSA and AES Cryptographic Provider";
      if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
        providerName = "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)";
      this.m_cspHandle = CapiNative.AcquireCsp((string) null, providerName, CapiNative.ProviderType.RsaAes, CapiNative.CryptAcquireContextFlags.VerifyContext, true);
      this.FeedbackSizeValue = 8;
      int defaultKeySize = 0;
      if (AesCryptoServiceProvider.FindSupportedKeySizes(this.m_cspHandle, out defaultKeySize).Length == 0)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      this.KeySizeValue = defaultKeySize;
    }

    [SecurityCritical]
    public override ICryptoTransform CreateDecryptor()
    {
      if (this.m_key == null || this.m_key.IsInvalid || this.m_key.IsClosed)
        throw new CryptographicException(SR.GetString("Cryptography_DecryptWithNoKey"));
      else
        return this.CreateDecryptor(this.m_key, this.IVValue);
    }

    [SecurityCritical]
    public override ICryptoTransform CreateDecryptor(byte[] key, byte[] iv)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      if (!this.ValidKeySize(key.Length * 8))
        throw new ArgumentException(SR.GetString("Cryptography_InvalidKeySize"), "key");
      if (iv != null && iv.Length * 8 != this.BlockSizeValue)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidIVSize"), "iv");
      byte[] key1 = (byte[]) key.Clone();
      byte[] iv1 = (byte[]) null;
      if (iv != null)
        iv1 = (byte[]) iv.Clone();
      using (SafeCapiKeyHandle key2 = CapiNative.ImportSymmetricKey(this.m_cspHandle, AesCryptoServiceProvider.GetAlgorithmId(key1.Length * 8), key1))
        return this.CreateDecryptor(key2, iv1);
    }

    [SecurityCritical]
    public override ICryptoTransform CreateEncryptor()
    {
      if (this.m_key == null || this.m_key.IsInvalid || this.m_key.IsClosed)
        this.GenerateKey();
      if (this.Mode != CipherMode.ECB && this.IVValue == null)
        this.GenerateIV();
      return this.CreateEncryptor(this.m_key, this.IVValue);
    }

    [SecurityCritical]
    public override ICryptoTransform CreateEncryptor(byte[] key, byte[] iv)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      if (!this.ValidKeySize(key.Length * 8))
        throw new ArgumentException(SR.GetString("Cryptography_InvalidKeySize"), "key");
      if (iv != null && iv.Length * 8 != this.BlockSizeValue)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidIVSize"), "iv");
      byte[] key1 = (byte[]) key.Clone();
      byte[] iv1 = (byte[]) null;
      if (iv != null)
        iv1 = (byte[]) iv.Clone();
      using (SafeCapiKeyHandle key2 = CapiNative.ImportSymmetricKey(this.m_cspHandle, AesCryptoServiceProvider.GetAlgorithmId(key1.Length * 8), key1))
        return this.CreateEncryptor(key2, iv1);
    }

    [SecurityCritical]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        if (this.m_key != null)
          this.m_key.Dispose();
        if (this.m_cspHandle == null)
          return;
        this.m_cspHandle.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    [SecurityCritical]
    public override void GenerateKey()
    {
      SafeCapiKeyHandle phKey = (SafeCapiKeyHandle) null;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        if (!CapiNative.UnsafeNativeMethods.CryptGenKey(this.m_cspHandle, AesCryptoServiceProvider.GetAlgorithmId(this.KeySizeValue), CapiNative.KeyFlags.Exportable, out phKey))
          throw new CryptographicException(Marshal.GetLastWin32Error());
      }
      finally
      {
        if (phKey != null && !phKey.IsInvalid)
          phKey.SetParentCsp(this.m_cspHandle);
      }
      if (this.m_key != null)
        this.m_key.Dispose();
      this.m_key = phKey;
    }

    [SecurityCritical]
    public override void GenerateIV()
    {
      byte[] pbBuffer = new byte[this.BlockSizeValue / 8];
      if (!CapiNative.UnsafeNativeMethods.CryptGenRandom(this.m_cspHandle, pbBuffer.Length, pbBuffer))
        throw new CryptographicException(Marshal.GetLastWin32Error());
      this.IVValue = pbBuffer;
    }

    [SecurityCritical]
    private ICryptoTransform CreateDecryptor(SafeCapiKeyHandle key, byte[] iv)
    {
      return (ICryptoTransform) new CapiSymmetricAlgorithm(this.BlockSizeValue, this.FeedbackSizeValue, this.m_cspHandle, key, iv, this.Mode, this.PaddingValue, EncryptionMode.Decrypt);
    }

    [SecurityCritical]
    private ICryptoTransform CreateEncryptor(SafeCapiKeyHandle key, byte[] iv)
    {
      return (ICryptoTransform) new CapiSymmetricAlgorithm(this.BlockSizeValue, this.FeedbackSizeValue, this.m_cspHandle, key, iv, this.Mode, this.PaddingValue, EncryptionMode.Encrypt);
    }

    [SecurityCritical]
    private static KeySizes[] FindSupportedKeySizes(SafeCspHandle csp, out int defaultKeySize)
    {
      if (AesCryptoServiceProvider.s_supportedKeySizes == null)
      {
        List<KeySizes> list = new List<KeySizes>();
        int num = 0;
        for (CapiNative.PROV_ENUMALGS providerParameterStruct = CapiNative.GetProviderParameterStruct<CapiNative.PROV_ENUMALGS>(csp, CapiNative.ProviderParameter.EnumerateAlgorithms, CapiNative.ProviderParameterFlags.RestartEnumeration); providerParameterStruct.aiAlgId != CapiNative.AlgorithmId.None; providerParameterStruct = CapiNative.GetProviderParameterStruct<CapiNative.PROV_ENUMALGS>(csp, CapiNative.ProviderParameter.EnumerateAlgorithms, CapiNative.ProviderParameterFlags.None))
        {
          switch (providerParameterStruct.aiAlgId)
          {
            case CapiNative.AlgorithmId.Aes128:
              list.Add(new KeySizes(128, 128, 0));
              if (128 > num)
              {
                num = 128;
                break;
              }
              else
                break;
            case CapiNative.AlgorithmId.Aes192:
              list.Add(new KeySizes(192, 192, 0));
              if (192 > num)
              {
                num = 192;
                break;
              }
              else
                break;
            case CapiNative.AlgorithmId.Aes256:
              list.Add(new KeySizes(256, 256, 0));
              if (256 > num)
              {
                num = 256;
                break;
              }
              else
                break;
          }
        }
        AesCryptoServiceProvider.s_supportedKeySizes = list.ToArray();
        AesCryptoServiceProvider.s_defaultKeySize = num;
      }
      defaultKeySize = AesCryptoServiceProvider.s_defaultKeySize;
      return AesCryptoServiceProvider.s_supportedKeySizes;
    }

    private static CapiNative.AlgorithmId GetAlgorithmId(int keySize)
    {
      switch (keySize)
      {
        case 128:
          return CapiNative.AlgorithmId.Aes128;
        case 192:
          return CapiNative.AlgorithmId.Aes192;
        case 256:
          return CapiNative.AlgorithmId.Aes256;
        default:
          return CapiNative.AlgorithmId.None;
      }
    }
  }
}
