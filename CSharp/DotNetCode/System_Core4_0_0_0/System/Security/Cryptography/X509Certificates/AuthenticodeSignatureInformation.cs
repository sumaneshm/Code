// Type: System.Security.Cryptography.X509Certificates.AuthenticodeSignatureInformation
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class AuthenticodeSignatureInformation
  {
    private string m_description;
    private Uri m_descriptionUrl;
    private CapiNative.AlgorithmId m_hashAlgorithmId;
    private X509Chain m_signatureChain;
    private TimestampInformation m_timestamp;
    private SignatureVerificationResult m_verificationResult;
    private X509Certificate2 m_signingCertificate;

    public string Description
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_description;
      }
    }

    public Uri DescriptionUrl
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_descriptionUrl;
      }
    }

    public string HashAlgorithm
    {
      get
      {
        return CapiNative.GetAlgorithmName(this.m_hashAlgorithmId);
      }
    }

    public int HResult
    {
      get
      {
        return CapiNative.HResultForVerificationResult(this.m_verificationResult);
      }
    }

    public X509Chain SignatureChain
    {
      [SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), StorePermission(SecurityAction.Demand, EnumerateCertificates = true, OpenStore = true)] get
      {
        return this.m_signatureChain;
      }
    }

    public X509Certificate2 SigningCertificate
    {
      [SecuritySafeCritical, StorePermission(SecurityAction.Demand, EnumerateCertificates = true, OpenStore = true)] get
      {
        if (this.m_signingCertificate == null && this.SignatureChain != null)
          this.m_signingCertificate = this.SignatureChain.ChainElements[0].Certificate;
        return this.m_signingCertificate;
      }
    }

    public TimestampInformation Timestamp
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_timestamp;
      }
    }

    public TrustStatus TrustStatus
    {
      get
      {
        switch (this.VerificationResult)
        {
          case SignatureVerificationResult.CertificateNotExplicitlyTrusted:
            return TrustStatus.KnownIdentity;
          case SignatureVerificationResult.CertificateExplicitlyDistrusted:
            return TrustStatus.Untrusted;
          case SignatureVerificationResult.Valid:
            return TrustStatus.Trusted;
          default:
            return TrustStatus.UnknownIdentity;
        }
      }
    }

    public SignatureVerificationResult VerificationResult
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_verificationResult;
      }
    }

    [SecurityCritical]
    internal AuthenticodeSignatureInformation(X509Native.AXL_AUTHENTICODE_SIGNER_INFO signer, X509Chain signatureChain, TimestampInformation timestamp)
    {
      this.m_verificationResult = (SignatureVerificationResult) signer.dwError;
      this.m_hashAlgorithmId = signer.algHash;
      if (signer.pwszDescription != IntPtr.Zero)
        this.m_description = Marshal.PtrToStringUni(signer.pwszDescription);
      if (signer.pwszDescriptionUrl != IntPtr.Zero)
        Uri.TryCreate(Marshal.PtrToStringUni(signer.pwszDescriptionUrl), UriKind.RelativeOrAbsolute, out this.m_descriptionUrl);
      this.m_signatureChain = signatureChain;
      if (timestamp != null && timestamp.VerificationResult != SignatureVerificationResult.MissingSignature)
      {
        if (timestamp.IsValid)
          this.m_timestamp = timestamp;
        else
          this.m_verificationResult = SignatureVerificationResult.InvalidTimestamp;
      }
      else
        this.m_timestamp = (TimestampInformation) null;
    }

    internal AuthenticodeSignatureInformation(SignatureVerificationResult error)
    {
      this.m_verificationResult = error;
    }
  }
}
