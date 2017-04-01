// Type: System.Security.Cryptography.ECDiffieHellmanCng
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class ECDiffieHellmanCng : ECDiffieHellman
  {
    private static KeySizes[] s_legalKeySizes = new KeySizes[2]
    {
      new KeySizes(256, 384, 128),
      new KeySizes(521, 521, 0)
    };
    private CngAlgorithm m_hashAlgorithm = CngAlgorithm.Sha256;
    private byte[] m_hmacKey;
    private CngKey m_key;
    private ECDiffieHellmanKeyDerivationFunction m_kdf;
    private byte[] m_label;
    private byte[] m_secretAppend;
    private byte[] m_secretPrepend;
    private byte[] m_seed;

    public CngAlgorithm HashAlgorithm
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_hashAlgorithm;
      }
      set
      {
        if (this.m_hashAlgorithm == (CngAlgorithm) null)
          throw new ArgumentNullException("value");
        this.m_hashAlgorithm = value;
      }
    }

    public byte[] HmacKey
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_hmacKey;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_hmacKey = value;
      }
    }

    public ECDiffieHellmanKeyDerivationFunction KeyDerivationFunction
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_kdf;
      }
      set
      {
        if (value < ECDiffieHellmanKeyDerivationFunction.Hash || value > ECDiffieHellmanKeyDerivationFunction.Tls)
          throw new ArgumentOutOfRangeException("value");
        this.m_kdf = value;
      }
    }

    public byte[] Label
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_label;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_label = value;
      }
    }

    public byte[] SecretAppend
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_secretAppend;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_secretAppend = value;
      }
    }

    public byte[] SecretPrepend
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_secretPrepend;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_secretPrepend = value;
      }
    }

    public byte[] Seed
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_seed;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_seed = value;
      }
    }

    public CngKey Key
    {
      get
      {
        if (this.m_key != null && this.m_key.KeySize != this.KeySize)
        {
          this.m_key.Dispose();
          this.m_key = (CngKey) null;
        }
        if (this.m_key == null)
        {
          CngAlgorithm algorithm = (CngAlgorithm) null;
          switch (this.KeySize)
          {
            case 256:
              algorithm = CngAlgorithm.ECDiffieHellmanP256;
              break;
            case 384:
              algorithm = CngAlgorithm.ECDiffieHellmanP384;
              break;
            case 521:
              algorithm = CngAlgorithm.ECDiffieHellmanP521;
              break;
          }
          this.m_key = CngKey.Create(algorithm);
        }
        return this.m_key;
      }
      private set
      {
        if (value.AlgorithmGroup != CngAlgorithmGroup.ECDiffieHellman)
          throw new ArgumentException(SR.GetString("Cryptography_ArgECDHRequiresECDHKey"));
        if (this.m_key != null)
          this.m_key.Dispose();
        this.m_key = value;
        this.KeySize = this.m_key.KeySize;
      }
    }

    public override ECDiffieHellmanPublicKey PublicKey
    {
      get
      {
        return (ECDiffieHellmanPublicKey) new ECDiffieHellmanCngPublicKey(this.Key);
      }
    }

    public bool UseSecretAgreementAsHmacKey
    {
      get
      {
        return this.HmacKey == null;
      }
    }

    static ECDiffieHellmanCng()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ECDiffieHellmanCng()
      : this(521)
    {
    }

    public ECDiffieHellmanCng(int keySize)
    {
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      this.LegalKeySizesValue = ECDiffieHellmanCng.s_legalKeySizes;
      this.KeySize = keySize;
    }

    [SecuritySafeCritical]
    public ECDiffieHellmanCng(CngKey key)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      if (key.AlgorithmGroup != CngAlgorithmGroup.ECDiffieHellman)
        throw new ArgumentException(SR.GetString("Cryptography_ArgECDHRequiresECDHKey"), "key");
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      this.LegalKeySizesValue = ECDiffieHellmanCng.s_legalKeySizes;
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
      using (SafeNCryptKeyHandle handle = key.Handle)
        this.Key = CngKey.Open(handle, key.IsEphemeral ? CngKeyHandleOpenOptions.EphemeralKey : CngKeyHandleOpenOptions.None);
      CodeAccessPermission.RevertAssert();
      this.KeySize = this.m_key.KeySize;
    }

    public override byte[] DeriveKeyMaterial(ECDiffieHellmanPublicKey otherPartyPublicKey)
    {
      if (otherPartyPublicKey == null)
        throw new ArgumentNullException("otherPartyPublicKey");
      ECDiffieHellmanCngPublicKey hellmanCngPublicKey = otherPartyPublicKey as ECDiffieHellmanCngPublicKey;
      if (otherPartyPublicKey == null)
        throw new ArgumentException(SR.GetString("Cryptography_ArgExpectedECDiffieHellmanCngPublicKey"));
      using (CngKey otherPartyPublicKey1 = hellmanCngPublicKey.Import())
        return this.DeriveKeyMaterial(otherPartyPublicKey1);
    }

    [SecuritySafeCritical]
    public byte[] DeriveKeyMaterial(CngKey otherPartyPublicKey)
    {
      if (otherPartyPublicKey == null)
        throw new ArgumentNullException("otherPartyPublicKey");
      if (otherPartyPublicKey.AlgorithmGroup != CngAlgorithmGroup.ECDiffieHellman)
        throw new ArgumentException(SR.GetString("Cryptography_ArgECDHRequiresECDHKey"), "otherPartyPublicKey");
      if (otherPartyPublicKey.KeySize != this.KeySize)
        throw new ArgumentException(SR.GetString("Cryptography_ArgECDHKeySizeMismatch"), "otherPartyPublicKey");
      NCryptNative.SecretAgreementFlags flags = this.UseSecretAgreementAsHmacKey ? NCryptNative.SecretAgreementFlags.UseSecretAsHmacKey : NCryptNative.SecretAgreementFlags.None;
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
      using (SafeNCryptKeyHandle handle1 = this.Key.Handle)
      {
        using (SafeNCryptKeyHandle handle2 = otherPartyPublicKey.Handle)
        {
          CodeAccessPermission.RevertAssert();
          using (SafeNCryptSecretHandle secretAgreement = NCryptNative.DeriveSecretAgreement(handle1, handle2))
          {
            if (this.KeyDerivationFunction == ECDiffieHellmanKeyDerivationFunction.Hash)
            {
              byte[] secretAppend = this.SecretAppend == null ? (byte[]) null : this.SecretAppend.Clone() as byte[];
              byte[] secretPrepend = this.SecretPrepend == null ? (byte[]) null : this.SecretPrepend.Clone() as byte[];
              return NCryptNative.DeriveKeyMaterialHash(secretAgreement, this.HashAlgorithm.Algorithm, secretPrepend, secretAppend, flags);
            }
            else if (this.KeyDerivationFunction == ECDiffieHellmanKeyDerivationFunction.Hmac)
            {
              byte[] hmacKey = this.HmacKey == null ? (byte[]) null : this.HmacKey.Clone() as byte[];
              byte[] secretAppend = this.SecretAppend == null ? (byte[]) null : this.SecretAppend.Clone() as byte[];
              byte[] secretPrepend = this.SecretPrepend == null ? (byte[]) null : this.SecretPrepend.Clone() as byte[];
              return NCryptNative.DeriveKeyMaterialHmac(secretAgreement, this.HashAlgorithm.Algorithm, hmacKey, secretPrepend, secretAppend, flags);
            }
            else
            {
              byte[] label = this.Label == null ? (byte[]) null : this.Label.Clone() as byte[];
              byte[] seed = this.Seed == null ? (byte[]) null : this.Seed.Clone() as byte[];
              if (label == null || seed == null)
                throw new InvalidOperationException(SR.GetString("Cryptography_TlsRequiresLabelAndSeed"));
              else
                return NCryptNative.DeriveKeyMaterialTls(secretAgreement, label, seed, flags);
            }
          }
        }
      }
    }

    public SafeNCryptSecretHandle DeriveSecretAgreementHandle(ECDiffieHellmanPublicKey otherPartyPublicKey)
    {
      if (otherPartyPublicKey == null)
        throw new ArgumentNullException("otherPartyPublicKey");
      ECDiffieHellmanCngPublicKey hellmanCngPublicKey = otherPartyPublicKey as ECDiffieHellmanCngPublicKey;
      if (otherPartyPublicKey == null)
        throw new ArgumentException(SR.GetString("Cryptography_ArgExpectedECDiffieHellmanCngPublicKey"));
      using (CngKey otherPartyPublicKey1 = hellmanCngPublicKey.Import())
        return this.DeriveSecretAgreementHandle(otherPartyPublicKey1);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public SafeNCryptSecretHandle DeriveSecretAgreementHandle(CngKey otherPartyPublicKey)
    {
      if (otherPartyPublicKey == null)
        throw new ArgumentNullException("otherPartyPublicKey");
      if (otherPartyPublicKey.AlgorithmGroup != CngAlgorithmGroup.ECDiffieHellman)
        throw new ArgumentException(SR.GetString("Cryptography_ArgECDHRequiresECDHKey"), "otherPartyPublicKey");
      if (otherPartyPublicKey.KeySize != this.KeySize)
        throw new ArgumentException(SR.GetString("Cryptography_ArgECDHKeySizeMismatch"), "otherPartyPublicKey");
      using (SafeNCryptKeyHandle handle1 = this.Key.Handle)
      {
        using (SafeNCryptKeyHandle handle2 = otherPartyPublicKey.Handle)
          return NCryptNative.DeriveSecretAgreement(handle1, handle2);
      }
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this.m_key == null)
          return;
        this.m_key.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public override void FromXmlString(string xmlString)
    {
      throw new NotImplementedException(SR.GetString("Cryptography_ECXmlSerializationFormatRequired"));
    }

    public void FromXmlString(string xml, ECKeyXmlFormat format)
    {
      if (xml == null)
        throw new ArgumentNullException("xml");
      if (format != ECKeyXmlFormat.Rfc4050)
        throw new ArgumentOutOfRangeException("format");
      this.Key = Rfc4050KeyFormatter.FromXml(xml);
    }

    public override string ToXmlString(bool includePrivateParameters)
    {
      throw new NotImplementedException(SR.GetString("Cryptography_ECXmlSerializationFormatRequired"));
    }

    public string ToXmlString(ECKeyXmlFormat format)
    {
      if (format != ECKeyXmlFormat.Rfc4050)
        throw new ArgumentOutOfRangeException("format");
      else
        return Rfc4050KeyFormatter.ToXml(this.Key);
    }
  }
}
