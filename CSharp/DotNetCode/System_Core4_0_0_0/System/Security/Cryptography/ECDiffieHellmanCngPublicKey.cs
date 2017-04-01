// Type: System.Security.Cryptography.ECDiffieHellmanCngPublicKey
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
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class ECDiffieHellmanCngPublicKey : ECDiffieHellmanPublicKey
  {
    [NonSerialized]
    private CngKey m_key;
    private CngKeyBlobFormat m_format;

    public CngKeyBlobFormat BlobFormat
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_format;
      }
    }

    [SecuritySafeCritical]
    internal ECDiffieHellmanCngPublicKey(CngKey key)
      : base(key.Export(CngKeyBlobFormat.EccPublicBlob))
    {
      this.m_format = CngKeyBlobFormat.EccPublicBlob;
      new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
      using (SafeNCryptKeyHandle handle = key.Handle)
        this.m_key = CngKey.Open(handle, key.IsEphemeral ? CngKeyHandleOpenOptions.EphemeralKey : CngKeyHandleOpenOptions.None);
      CodeAccessPermission.RevertAssert();
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

    [SecuritySafeCritical]
    public static ECDiffieHellmanPublicKey FromByteArray(byte[] publicKeyBlob, CngKeyBlobFormat format)
    {
      if (publicKeyBlob == null)
        throw new ArgumentNullException("publicKeyBlob");
      if (format == (CngKeyBlobFormat) null)
        throw new ArgumentNullException("format");
      using (CngKey key = CngKey.Import(publicKeyBlob, format))
      {
        if (key.AlgorithmGroup != CngAlgorithmGroup.ECDiffieHellman)
          throw new ArgumentException(SR.GetString("Cryptography_ArgECDHRequiresECDHKey"));
        else
          return (ECDiffieHellmanPublicKey) new ECDiffieHellmanCngPublicKey(key);
      }
    }

    [SecuritySafeCritical]
    public static ECDiffieHellmanCngPublicKey FromXmlString(string xml)
    {
      if (xml == null)
        throw new ArgumentNullException("xml");
      using (CngKey key = Rfc4050KeyFormatter.FromXml(xml))
      {
        if (key.AlgorithmGroup != CngAlgorithmGroup.ECDiffieHellman)
          throw new ArgumentException(SR.GetString("Cryptography_ArgECDHRequiresECDHKey"), "xml");
        else
          return new ECDiffieHellmanCngPublicKey(key);
      }
    }

    public CngKey Import()
    {
      return CngKey.Import(this.ToByteArray(), this.BlobFormat);
    }

    public override string ToXmlString()
    {
      if (this.m_key == null)
        this.m_key = this.Import();
      return Rfc4050KeyFormatter.ToXml(this.m_key);
    }
  }
}
