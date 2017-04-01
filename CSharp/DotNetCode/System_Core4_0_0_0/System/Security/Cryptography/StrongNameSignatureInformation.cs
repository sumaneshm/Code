// Type: System.Security.Cryptography.StrongNameSignatureInformation
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class StrongNameSignatureInformation
  {
    private static readonly string StrongNameHashAlgorithm = CapiNative.GetAlgorithmName(CapiNative.AlgorithmId.Sha1);
    private SignatureVerificationResult m_verificationResult;
    private AsymmetricAlgorithm m_publicKey;

    public string HashAlgorithm
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return StrongNameSignatureInformation.StrongNameHashAlgorithm;
      }
    }

    public int HResult
    {
      get
      {
        return CapiNative.HResultForVerificationResult(this.m_verificationResult);
      }
    }

    public bool IsValid
    {
      get
      {
        return this.m_verificationResult == SignatureVerificationResult.Valid;
      }
    }

    public AsymmetricAlgorithm PublicKey
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_publicKey;
      }
    }

    public SignatureVerificationResult VerificationResult
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_verificationResult;
      }
    }

    static StrongNameSignatureInformation()
    {
    }

    internal StrongNameSignatureInformation(AsymmetricAlgorithm publicKey)
    {
      this.m_verificationResult = SignatureVerificationResult.Valid;
      this.m_publicKey = publicKey;
    }

    internal StrongNameSignatureInformation(SignatureVerificationResult error)
    {
      this.m_verificationResult = error;
    }
  }
}
