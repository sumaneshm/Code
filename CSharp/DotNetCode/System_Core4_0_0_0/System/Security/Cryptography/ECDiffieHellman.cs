// Type: System.Security.Cryptography.ECDiffieHellman
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public abstract class ECDiffieHellman : AsymmetricAlgorithm
  {
    public override string KeyExchangeAlgorithm
    {
      get
      {
        return "ECDiffieHellman";
      }
    }

    public override string SignatureAlgorithm
    {
      get
      {
        return (string) null;
      }
    }

    public abstract ECDiffieHellmanPublicKey PublicKey { get; }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected ECDiffieHellman()
    {
    }

    public static ECDiffieHellman Create()
    {
      return ECDiffieHellman.Create(typeof (ECDiffieHellmanCng).FullName);
    }

    public static ECDiffieHellman Create(string algorithm)
    {
      if (algorithm == null)
        throw new ArgumentNullException("algorithm");
      else
        return CryptoConfig.CreateFromName(algorithm) as ECDiffieHellman;
    }

    public abstract byte[] DeriveKeyMaterial(ECDiffieHellmanPublicKey otherPartyPublicKey);
  }
}
