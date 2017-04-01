// Type: System.Security.Cryptography.X509Certificates.TimestampInformation
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class TimestampInformation
  {
    private CapiNative.AlgorithmId m_hashAlgorithmId;
    private DateTime m_timestamp;
    private X509Chain m_timestampChain;
    private SignatureVerificationResult m_verificationResult;
    private X509Certificate2 m_timestamper;

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

    public bool IsValid
    {
      get
      {
        if (this.VerificationResult != SignatureVerificationResult.Valid)
          return this.VerificationResult == SignatureVerificationResult.CertificateNotExplicitlyTrusted;
        else
          return true;
      }
    }

    public X509Chain SignatureChain
    {
      [SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), StorePermission(SecurityAction.Demand, EnumerateCertificates = true, OpenStore = true)] get
      {
        return this.m_timestampChain;
      }
    }

    public X509Certificate2 SigningCertificate
    {
      [SecuritySafeCritical, StorePermission(SecurityAction.Demand, EnumerateCertificates = true, OpenStore = true)] get
      {
        if (this.m_timestamper == null && this.SignatureChain != null)
          this.m_timestamper = this.SignatureChain.ChainElements[0].Certificate;
        return this.m_timestamper;
      }
    }

    public DateTime Timestamp
    {
      get
      {
        return this.m_timestamp.ToLocalTime();
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
    internal TimestampInformation(X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO timestamper)
    {
      this.m_hashAlgorithmId = timestamper.algHash;
      this.m_verificationResult = (SignatureVerificationResult) timestamper.dwError;
      this.m_timestamp = DateTime.FromFileTimeUtc((long) ((ulong) (uint) timestamper.ftTimestamp.dwHighDateTime << 32 | (ulong) (uint) timestamper.ftTimestamp.dwLowDateTime));
      if (!(timestamper.pChainContext != IntPtr.Zero))
        return;
      this.m_timestampChain = new X509Chain(timestamper.pChainContext);
    }

    internal TimestampInformation(SignatureVerificationResult error)
    {
      this.m_verificationResult = error;
    }
  }
}
