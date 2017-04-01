// Type: System.Security.Cryptography.ECDsaCng
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class ECDsaCng : ECDsa
  {
    private static KeySizes[] s_legalKeySizes = new KeySizes[2]
    {
      new KeySizes(256, 384, 128),
      new KeySizes(521, 521, 0)
    };
    private CngAlgorithm m_hashAlgorithm = CngAlgorithm.Sha256;
    private CngKey m_key;

    public CngAlgorithm HashAlgorithm
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_hashAlgorithm;
      }
      set
      {
        if (value == (CngAlgorithm) null)
          throw new ArgumentNullException("value");
        this.m_hashAlgorithm = value;
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
              algorithm = CngAlgorithm.ECDsaP256;
              break;
            case 384:
              algorithm = CngAlgorithm.ECDsaP384;
              break;
            case 521:
              algorithm = CngAlgorithm.ECDsaP521;
              break;
          }
          this.m_key = CngKey.Create(algorithm);
        }
        return this.m_key;
      }
      private set
      {
        if (value.AlgorithmGroup != CngAlgorithmGroup.ECDsa)
          throw new ArgumentException(SR.GetString("Cryptography_ArgECDsaRequiresECDsaKey"));
        if (this.m_key != null)
          this.m_key.Dispose();
        this.m_key = value;
        this.KeySize = this.m_key.KeySize;
      }
    }

    static ECDsaCng()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ECDsaCng()
      : this(521)
    {
    }

    public ECDsaCng(int keySize)
    {
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      this.LegalKeySizesValue = ECDsaCng.s_legalKeySizes;
      this.KeySize = keySize;
    }

    [SecuritySafeCritical]
    public ECDsaCng(CngKey key)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      if (key.AlgorithmGroup != CngAlgorithmGroup.ECDsa)
        throw new ArgumentException(SR.GetString("Cryptography_ArgECDsaRequiresECDsaKey"), "key");
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      this.LegalKeySizesValue = ECDsaCng.s_legalKeySizes;
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
      using (SafeNCryptKeyHandle handle = key.Handle)
        this.Key = CngKey.Open(handle, key.IsEphemeral ? CngKeyHandleOpenOptions.EphemeralKey : CngKeyHandleOpenOptions.None);
      CodeAccessPermission.RevertAssert();
      this.KeySize = this.m_key.KeySize;
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.m_key == null)
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

    public byte[] SignData(byte[] data)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      else
        return this.SignData(data, 0, data.Length);
    }

    [SecuritySafeCritical]
    public byte[] SignData(byte[] data, int offset, int count)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      if (offset < 0 || offset > data.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (count < 0 || count > data.Length - offset)
        throw new ArgumentOutOfRangeException("count");
      using (BCryptHashAlgorithm bcryptHashAlgorithm = new BCryptHashAlgorithm(this.HashAlgorithm, "Microsoft Primitive Provider"))
      {
        bcryptHashAlgorithm.HashCore(data, offset, count);
        return this.SignHash(bcryptHashAlgorithm.HashFinal());
      }
    }

    [SecuritySafeCritical]
    public byte[] SignData(Stream data)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      using (BCryptHashAlgorithm bcryptHashAlgorithm = new BCryptHashAlgorithm(this.HashAlgorithm, "Microsoft Primitive Provider"))
      {
        bcryptHashAlgorithm.HashStream(data);
        return this.SignHash(bcryptHashAlgorithm.HashFinal());
      }
    }

    [SecuritySafeCritical]
    public override byte[] SignHash(byte[] hash)
    {
      if (hash == null)
        throw new ArgumentNullException("hash");
      KeyContainerPermission containerPermission = this.Key.BuildKeyContainerPermission(KeyContainerPermissionFlags.Sign);
      if (containerPermission != null)
        containerPermission.Demand();
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
      using (SafeNCryptKeyHandle handle = this.Key.Handle)
      {
        CodeAccessPermission.RevertAssert();
        return NCryptNative.SignHash(handle, hash);
      }
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

    public bool VerifyData(byte[] data, byte[] signature)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      else
        return this.VerifyData(data, 0, data.Length, signature);
    }

    [SecuritySafeCritical]
    public bool VerifyData(byte[] data, int offset, int count, byte[] signature)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      if (offset < 0 || offset > data.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (count < 0 || count > data.Length - offset)
        throw new ArgumentOutOfRangeException("count");
      if (signature == null)
        throw new ArgumentNullException("signature");
      using (BCryptHashAlgorithm bcryptHashAlgorithm = new BCryptHashAlgorithm(this.HashAlgorithm, "Microsoft Primitive Provider"))
      {
        bcryptHashAlgorithm.HashCore(data, offset, count);
        return this.VerifyHash(bcryptHashAlgorithm.HashFinal(), signature);
      }
    }

    [SecuritySafeCritical]
    public bool VerifyData(Stream data, byte[] signature)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      if (signature == null)
        throw new ArgumentNullException("signature");
      using (BCryptHashAlgorithm bcryptHashAlgorithm = new BCryptHashAlgorithm(this.HashAlgorithm, "Microsoft Primitive Provider"))
      {
        bcryptHashAlgorithm.HashStream(data);
        return this.VerifyHash(bcryptHashAlgorithm.HashFinal(), signature);
      }
    }

    [SecuritySafeCritical]
    public override bool VerifyHash(byte[] hash, byte[] signature)
    {
      if (hash == null)
        throw new ArgumentNullException("hash");
      if (signature == null)
        throw new ArgumentNullException("signature");
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
      using (SafeNCryptKeyHandle handle = this.Key.Handle)
      {
        CodeAccessPermission.RevertAssert();
        return NCryptNative.VerifySignature(handle, hash, signature);
      }
    }
  }
}
