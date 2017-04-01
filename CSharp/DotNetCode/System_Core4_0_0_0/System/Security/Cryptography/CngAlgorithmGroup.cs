// Type: System.Security.Cryptography.CngAlgorithmGroup
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
  public sealed class CngAlgorithmGroup : IEquatable<CngAlgorithmGroup>
  {
    private static volatile CngAlgorithmGroup s_dh;
    private static volatile CngAlgorithmGroup s_dsa;
    private static volatile CngAlgorithmGroup s_ecdh;
    private static volatile CngAlgorithmGroup s_ecdsa;
    private static volatile CngAlgorithmGroup s_rsa;
    private string m_algorithmGroup;

    public string AlgorithmGroup
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_algorithmGroup;
      }
    }

    public static CngAlgorithmGroup DiffieHellman
    {
      get
      {
        if (CngAlgorithmGroup.s_dh == (CngAlgorithmGroup) null)
          CngAlgorithmGroup.s_dh = new CngAlgorithmGroup("DH");
        return CngAlgorithmGroup.s_dh;
      }
    }

    public static CngAlgorithmGroup Dsa
    {
      get
      {
        if (CngAlgorithmGroup.s_dsa == (CngAlgorithmGroup) null)
          CngAlgorithmGroup.s_dsa = new CngAlgorithmGroup("DSA");
        return CngAlgorithmGroup.s_dsa;
      }
    }

    public static CngAlgorithmGroup ECDiffieHellman
    {
      get
      {
        if (CngAlgorithmGroup.s_ecdh == (CngAlgorithmGroup) null)
          CngAlgorithmGroup.s_ecdh = new CngAlgorithmGroup("ECDH");
        return CngAlgorithmGroup.s_ecdh;
      }
    }

    public static CngAlgorithmGroup ECDsa
    {
      get
      {
        if (CngAlgorithmGroup.s_ecdsa == (CngAlgorithmGroup) null)
          CngAlgorithmGroup.s_ecdsa = new CngAlgorithmGroup("ECDSA");
        return CngAlgorithmGroup.s_ecdsa;
      }
    }

    public static CngAlgorithmGroup Rsa
    {
      get
      {
        if (CngAlgorithmGroup.s_rsa == (CngAlgorithmGroup) null)
          CngAlgorithmGroup.s_rsa = new CngAlgorithmGroup("RSA");
        return CngAlgorithmGroup.s_rsa;
      }
    }

    public CngAlgorithmGroup(string algorithmGroup)
    {
      if (algorithmGroup == null)
        throw new ArgumentNullException("algorithmGroup");
      if (algorithmGroup.Length == 0)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidAlgorithmGroup", new object[1]
        {
          (object) algorithmGroup
        }), "algorithmGroup");
      else
        this.m_algorithmGroup = algorithmGroup;
    }

    public static bool operator ==(CngAlgorithmGroup left, CngAlgorithmGroup right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return object.ReferenceEquals((object) right, (object) null);
      else
        return left.Equals(right);
    }

    public static bool operator !=(CngAlgorithmGroup left, CngAlgorithmGroup right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return !object.ReferenceEquals((object) right, (object) null);
      else
        return !left.Equals(right);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as CngAlgorithmGroup);
    }

    public bool Equals(CngAlgorithmGroup other)
    {
      if (object.ReferenceEquals((object) other, (object) null))
        return false;
      else
        return this.m_algorithmGroup.Equals(other.AlgorithmGroup);
    }

    public override int GetHashCode()
    {
      return this.m_algorithmGroup.GetHashCode();
    }

    public override string ToString()
    {
      return this.m_algorithmGroup;
    }
  }
}
