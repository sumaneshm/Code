// Type: System.Security.Cryptography.X509Certificates.X509Native
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
  internal static class X509Native
  {
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public struct AXL_AUTHENTICODE_SIGNER_INFO
    {
      public int cbSize;
      public int dwError;
      public CapiNative.AlgorithmId algHash;
      public IntPtr pwszHash;
      public IntPtr pwszDescription;
      public IntPtr pwszDescriptionUrl;
      public IntPtr pChainContext;
    }

    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public struct AXL_AUTHENTICODE_TIMESTAMPER_INFO
    {
      public int cbsize;
      public int dwError;
      public CapiNative.AlgorithmId algHash;
      public System.Runtime.InteropServices.ComTypes.FILETIME ftTimestamp;
      public IntPtr pChainContext;
    }

    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical(SecurityCriticalScope.Everything)]
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public static class UnsafeNativeMethods
    {
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("clr")]
      public static int CertFreeAuthenticodeSignerInfo(ref X509Native.AXL_AUTHENTICODE_SIGNER_INFO pSignerInfo);

      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      [DllImport("clr")]
      public static int CertFreeAuthenticodeTimestamperInfo(ref X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO pTimestamperInfo);

      [DllImport("clr")]
      public static int _AxlGetIssuerPublicKeyHash(IntPtr pCertContext, out SafeAxlBufferHandle ppwszPublicKeyHash);

      [DllImport("clr")]
      public static int CertVerifyAuthenticodeLicense(ref CapiNative.CRYPTOAPI_BLOB pLicenseBlob, X509Native.AxlVerificationFlags dwFlags, [In, Out] ref X509Native.AXL_AUTHENTICODE_SIGNER_INFO pSignerInfo, [In, Out] ref X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO pTimestamperInfo);
    }

    [Flags]
    public enum AxlVerificationFlags
    {
      None = 0,
      NoRevocationCheck = 1,
      RevocationCheckEndCertOnly = 2,
      RevocationCheckEntireChain = 4,
      UrlOnlyCacheRetrieval = 8,
      LifetimeSigning = 16,
      TrustMicrosoftRootOnly = 32,
    }
  }
}
