// Type: System.Security.Cryptography.CngAlgorithm
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
  public sealed class CngAlgorithm : IEquatable<CngAlgorithm>
  {
    private static volatile CngAlgorithm s_ecdhp256;
    private static volatile CngAlgorithm s_ecdhp384;
    private static volatile CngAlgorithm s_ecdhp521;
    private static volatile CngAlgorithm s_ecdsap256;
    private static volatile CngAlgorithm s_ecdsap384;
    private static volatile CngAlgorithm s_ecdsap521;
    private static volatile CngAlgorithm s_md5;
    private static volatile CngAlgorithm s_sha1;
    private static volatile CngAlgorithm s_sha256;
    private static volatile CngAlgorithm s_sha384;
    private static volatile CngAlgorithm s_sha512;
    private string m_algorithm;

    public string Algorithm
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_algorithm;
      }
    }

    public static CngAlgorithm ECDiffieHellmanP256
    {
      get
      {
        if (CngAlgorithm.s_ecdhp256 == (CngAlgorithm) null)
          CngAlgorithm.s_ecdhp256 = new CngAlgorithm("ECDH_P256");
        return CngAlgorithm.s_ecdhp256;
      }
    }

    public static CngAlgorithm ECDiffieHellmanP384
    {
      get
      {
        if (CngAlgorithm.s_ecdhp384 == (CngAlgorithm) null)
          CngAlgorithm.s_ecdhp384 = new CngAlgorithm("ECDH_P384");
        return CngAlgorithm.s_ecdhp384;
      }
    }

    public static CngAlgorithm ECDiffieHellmanP521
    {
      get
      {
        if (CngAlgorithm.s_ecdhp521 == (CngAlgorithm) null)
          CngAlgorithm.s_ecdhp521 = new CngAlgorithm("ECDH_P521");
        return CngAlgorithm.s_ecdhp521;
      }
    }

    public static CngAlgorithm ECDsaP256
    {
      get
      {
        if (CngAlgorithm.s_ecdsap256 == (CngAlgorithm) null)
          CngAlgorithm.s_ecdsap256 = new CngAlgorithm("ECDSA_P256");
        return CngAlgorithm.s_ecdsap256;
      }
    }

    public static CngAlgorithm ECDsaP384
    {
      get
      {
        if (CngAlgorithm.s_ecdsap384 == (CngAlgorithm) null)
          CngAlgorithm.s_ecdsap384 = new CngAlgorithm("ECDSA_P384");
        return CngAlgorithm.s_ecdsap384;
      }
    }

    public static CngAlgorithm ECDsaP521
    {
      get
      {
        if (CngAlgorithm.s_ecdsap521 == (CngAlgorithm) null)
          CngAlgorithm.s_ecdsap521 = new CngAlgorithm("ECDSA_P521");
        return CngAlgorithm.s_ecdsap521;
      }
    }

    public static CngAlgorithm MD5
    {
      get
      {
        if (CngAlgorithm.s_md5 == (CngAlgorithm) null)
          CngAlgorithm.s_md5 = new CngAlgorithm("MD5");
        return CngAlgorithm.s_md5;
      }
    }

    public static CngAlgorithm Sha1
    {
      get
      {
        if (CngAlgorithm.s_sha1 == (CngAlgorithm) null)
          CngAlgorithm.s_sha1 = new CngAlgorithm("SHA1");
        return CngAlgorithm.s_sha1;
      }
    }

    public static CngAlgorithm Sha256
    {
      get
      {
        if (CngAlgorithm.s_sha256 == (CngAlgorithm) null)
          CngAlgorithm.s_sha256 = new CngAlgorithm("SHA256");
        return CngAlgorithm.s_sha256;
      }
    }

    public static CngAlgorithm Sha384
    {
      get
      {
        if (CngAlgorithm.s_sha384 == (CngAlgorithm) null)
          CngAlgorithm.s_sha384 = new CngAlgorithm("SHA384");
        return CngAlgorithm.s_sha384;
      }
    }

    public static CngAlgorithm Sha512
    {
      get
      {
        if (CngAlgorithm.s_sha512 == (CngAlgorithm) null)
          CngAlgorithm.s_sha512 = new CngAlgorithm("SHA512");
        return CngAlgorithm.s_sha512;
      }
    }

    public CngAlgorithm(string algorithm)
    {
      if (algorithm == null)
        throw new ArgumentNullException("algorithm");
      if (algorithm.Length == 0)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidAlgorithmName", new object[1]
        {
          (object) algorithm
        }), "algorithm");
      else
        this.m_algorithm = algorithm;
    }

    public static bool operator ==(CngAlgorithm left, CngAlgorithm right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return object.ReferenceEquals((object) right, (object) null);
      else
        return left.Equals(right);
    }

    public static bool operator !=(CngAlgorithm left, CngAlgorithm right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return !object.ReferenceEquals((object) right, (object) null);
      else
        return !left.Equals(right);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as CngAlgorithm);
    }

    public bool Equals(CngAlgorithm other)
    {
      if (object.ReferenceEquals((object) other, (object) null))
        return false;
      else
        return this.m_algorithm.Equals(other.Algorithm);
    }

    public override int GetHashCode()
    {
      return this.m_algorithm.GetHashCode();
    }

    public override string ToString()
    {
      return this.m_algorithm;
    }
  }
}
