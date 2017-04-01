// Type: System.Security.Cryptography.CngKeyBlobFormat
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CngKeyBlobFormat : IEquatable<CngKeyBlobFormat>
  {
    private static volatile CngKeyBlobFormat s_eccPrivate;
    private static volatile CngKeyBlobFormat s_eccPublic;
    private static volatile CngKeyBlobFormat s_genericPrivate;
    private static volatile CngKeyBlobFormat s_genericPublic;
    private static volatile CngKeyBlobFormat s_opaqueTransport;
    private static volatile CngKeyBlobFormat s_pkcs8Private;
    private string m_format;

    public string Format
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_format;
      }
    }

    public static CngKeyBlobFormat EccPrivateBlob
    {
      get
      {
        if (CngKeyBlobFormat.s_eccPrivate == (CngKeyBlobFormat) null)
          CngKeyBlobFormat.s_eccPrivate = new CngKeyBlobFormat("ECCPRIVATEBLOB");
        return CngKeyBlobFormat.s_eccPrivate;
      }
    }

    public static CngKeyBlobFormat EccPublicBlob
    {
      get
      {
        if (CngKeyBlobFormat.s_eccPublic == (CngKeyBlobFormat) null)
          CngKeyBlobFormat.s_eccPublic = new CngKeyBlobFormat("ECCPUBLICBLOB");
        return CngKeyBlobFormat.s_eccPublic;
      }
    }

    public static CngKeyBlobFormat GenericPrivateBlob
    {
      get
      {
        if (CngKeyBlobFormat.s_genericPrivate == (CngKeyBlobFormat) null)
          CngKeyBlobFormat.s_genericPrivate = new CngKeyBlobFormat("PRIVATEBLOB");
        return CngKeyBlobFormat.s_genericPrivate;
      }
    }

    public static CngKeyBlobFormat GenericPublicBlob
    {
      get
      {
        if (CngKeyBlobFormat.s_genericPublic == (CngKeyBlobFormat) null)
          CngKeyBlobFormat.s_genericPublic = new CngKeyBlobFormat("PUBLICBLOB");
        return CngKeyBlobFormat.s_genericPublic;
      }
    }

    public static CngKeyBlobFormat OpaqueTransportBlob
    {
      get
      {
        if (CngKeyBlobFormat.s_opaqueTransport == (CngKeyBlobFormat) null)
          CngKeyBlobFormat.s_opaqueTransport = new CngKeyBlobFormat("OpaqueTransport");
        return CngKeyBlobFormat.s_opaqueTransport;
      }
    }

    public static CngKeyBlobFormat Pkcs8PrivateBlob
    {
      get
      {
        if (CngKeyBlobFormat.s_pkcs8Private == (CngKeyBlobFormat) null)
          CngKeyBlobFormat.s_pkcs8Private = new CngKeyBlobFormat("PKCS8_PRIVATEKEY");
        return CngKeyBlobFormat.s_pkcs8Private;
      }
    }

    public CngKeyBlobFormat(string format)
    {
      if (format == null)
        throw new ArgumentNullException("format");
      if (format.Length == 0)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidKeyBlobFormat", new object[1]
        {
          (object) format
        }), "format");
      else
        this.m_format = format;
    }

    public static bool operator ==(CngKeyBlobFormat left, CngKeyBlobFormat right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return object.ReferenceEquals((object) right, (object) null);
      else
        return left.Equals(right);
    }

    public static bool operator !=(CngKeyBlobFormat left, CngKeyBlobFormat right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return !object.ReferenceEquals((object) right, (object) null);
      else
        return !left.Equals(right);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as CngKeyBlobFormat);
    }

    public bool Equals(CngKeyBlobFormat other)
    {
      if (object.ReferenceEquals((object) other, (object) null))
        return false;
      else
        return this.m_format.Equals(other.Format);
    }

    public override int GetHashCode()
    {
      return this.m_format.GetHashCode();
    }

    public override string ToString()
    {
      return this.m_format;
    }
  }
}
