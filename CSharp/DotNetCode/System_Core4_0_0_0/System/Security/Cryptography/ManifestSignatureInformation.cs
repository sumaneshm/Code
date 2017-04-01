// Type: System.Security.Cryptography.ManifestSignatureInformation
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class ManifestSignatureInformation
  {
    private ManifestKinds m_manifest;
    private StrongNameSignatureInformation m_strongNameSignature;
    private AuthenticodeSignatureInformation m_authenticodeSignature;

    public AuthenticodeSignatureInformation AuthenticodeSignature
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_authenticodeSignature;
      }
    }

    public ManifestKinds Manifest
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_manifest;
      }
    }

    public StrongNameSignatureInformation StrongNameSignature
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_strongNameSignature;
      }
    }

    internal ManifestSignatureInformation(ManifestKinds manifest, StrongNameSignatureInformation strongNameSignature, AuthenticodeSignatureInformation authenticodeSignature)
    {
      this.m_manifest = manifest;
      this.m_strongNameSignature = strongNameSignature;
      this.m_authenticodeSignature = authenticodeSignature;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ManifestSignatureInformationCollection VerifySignature(ActivationContext application)
    {
      return ManifestSignatureInformation.VerifySignature(application, ManifestKinds.ApplicationAndDeployment);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ManifestSignatureInformationCollection VerifySignature(ActivationContext application, ManifestKinds manifests)
    {
      return ManifestSignatureInformation.VerifySignature(application, manifests, X509RevocationFlag.ExcludeRoot, X509RevocationMode.Online);
    }

    [SecuritySafeCritical]
    public static ManifestSignatureInformationCollection VerifySignature(ActivationContext application, ManifestKinds manifests, X509RevocationFlag revocationFlag, X509RevocationMode revocationMode)
    {
      if (application == null)
        throw new ArgumentNullException("application");
      if (revocationFlag < X509RevocationFlag.EndCertificateOnly || X509RevocationFlag.ExcludeRoot < revocationFlag)
        throw new ArgumentOutOfRangeException("revocationFlag");
      if (revocationMode < X509RevocationMode.NoCheck || X509RevocationMode.Offline < revocationMode)
        throw new ArgumentOutOfRangeException("revocationMode");
      List<ManifestSignatureInformation> list = new List<ManifestSignatureInformation>();
      if ((manifests & ManifestKinds.Deployment) == ManifestKinds.Deployment)
      {
        ManifestSignedXml manifestSignedXml = new ManifestSignedXml(ManifestSignatureInformation.GetManifestXml(application, ManifestKinds.Deployment), ManifestKinds.Deployment);
        list.Add(manifestSignedXml.VerifySignature(revocationFlag, revocationMode));
      }
      if ((manifests & ManifestKinds.Application) == ManifestKinds.Application)
      {
        ManifestSignedXml manifestSignedXml = new ManifestSignedXml(ManifestSignatureInformation.GetManifestXml(application, ManifestKinds.Application), ManifestKinds.Application);
        list.Add(manifestSignedXml.VerifySignature(revocationFlag, revocationMode));
      }
      return new ManifestSignatureInformationCollection((IList<ManifestSignatureInformation>) list);
    }

    [SecuritySafeCritical]
    private static unsafe XmlDocument GetManifestXml(ActivationContext application, ManifestKinds manifest)
    {
      IStream stream = (IStream) null;
      if (manifest == ManifestKinds.Application)
        stream = InternalActivationContextHelper.GetApplicationComponentManifest(application) as IStream;
      else if (manifest == ManifestKinds.Deployment)
        stream = InternalActivationContextHelper.GetDeploymentComponentManifest(application) as IStream;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        byte[] numArray = new byte[4096];
        int count = 0;
        do
        {
          stream.Read(numArray, numArray.Length, new IntPtr((void*) &count));
          memoryStream.Write(numArray, 0, count);
        }
        while (count == numArray.Length);
        memoryStream.Position = 0L;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.Load((Stream) memoryStream);
        return xmlDocument;
      }
    }
  }
}
