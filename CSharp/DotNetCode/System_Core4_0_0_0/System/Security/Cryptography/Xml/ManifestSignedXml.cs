// Type: System.Security.Cryptography.Xml.ManifestSignedXml
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
  internal sealed class ManifestSignedXml : SignedXml
  {
    private ManifestKinds m_manifest;
    private XmlDocument m_manifestXml;
    private XmlNamespaceManager m_namespaceManager;

    public ManifestSignedXml(XmlDocument manifestXml, ManifestKinds manifest)
      : base(manifestXml)
    {
      this.m_manifest = manifest;
      this.m_manifestXml = manifestXml;
      this.m_namespaceManager = new XmlNamespaceManager(manifestXml.NameTable);
      this.m_namespaceManager.AddNamespace("as", "http://schemas.microsoft.com/windows/pki/2005/Authenticode");
      this.m_namespaceManager.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");
      this.m_namespaceManager.AddNamespace("asmv2", "urn:schemas-microsoft-com:asm.v2");
      this.m_namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
      this.m_namespaceManager.AddNamespace("msrel", "http://schemas.microsoft.com/windows/rel/2005/reldata");
      this.m_namespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
    }

    private static byte[] BackwardHexToBytes(string hex)
    {
      if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
        return (byte[]) null;
      byte[] numArray = new byte[hex.Length / 2];
      int index1 = hex.Length - 2;
      for (int index2 = 0; index2 < numArray.Length; ++index2)
      {
        byte? nullable1 = ManifestSignedXml.HexToByte(hex[index1]);
        byte? nullable2 = ManifestSignedXml.HexToByte(hex[index1 + 1]);
        if (!nullable1.HasValue || !nullable2.HasValue)
          return (byte[]) null;
        numArray[index2] = (byte) ((uint) nullable1.Value << 4 | (uint) nullable2.Value);
        index1 -= 2;
      }
      return numArray;
    }

    [SecurityCritical]
    [StorePermission(SecurityAction.Assert, EnumerateCertificates = true, OpenStore = true)]
    private X509Chain BuildSignatureChain(X509Native.AXL_AUTHENTICODE_SIGNER_INFO signer, XmlElement licenseNode, X509RevocationFlag revocationFlag, X509RevocationMode revocationMode)
    {
      X509Chain x509Chain = (X509Chain) null;
      if (signer.pChainContext != IntPtr.Zero)
        x509Chain = new X509Chain(signer.pChainContext);
      else if (signer.dwError == -2146762487)
      {
        XmlElement xmlElement = licenseNode.SelectSingleNode("r:issuer/ds:Signature/ds:KeyInfo/ds:X509Data", this.m_namespaceManager) as XmlElement;
        if (xmlElement != null)
        {
          XmlNodeList xmlNodeList = xmlElement.SelectNodes("ds:X509Certificate", this.m_namespaceManager);
          if (xmlNodeList.Count == 1 && xmlNodeList[0] is XmlElement)
          {
            X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(xmlNodeList[0].InnerText.Trim()));
            x509Chain = new X509Chain();
            x509Chain.ChainPolicy.RevocationFlag = revocationFlag;
            x509Chain.ChainPolicy.RevocationMode = revocationMode;
            x509Chain.Build(certificate);
          }
        }
      }
      return x509Chain;
    }

    private byte[] CalculateManifestPublicKeyToken()
    {
      XmlElement xmlElement = this.m_manifestXml.SelectSingleNode("//asm:assembly/asm:assemblyIdentity", this.m_namespaceManager) as XmlElement;
      if (xmlElement == null)
        return (byte[]) null;
      else
        return ManifestSignedXml.HexStringToBytes(xmlElement.GetAttribute("publicKeyToken"));
    }

    [SecuritySafeCritical]
    private static unsafe byte[] CalculateSignerPublicKeyToken(AsymmetricAlgorithm key)
    {
      ICspAsymmetricAlgorithm asymmetricAlgorithm = key as ICspAsymmetricAlgorithm;
      if (asymmetricAlgorithm == null)
        return (byte[]) null;
      byte[] numArray = asymmetricAlgorithm.ExportCspBlob(false);
      SafeAxlBufferHandle ppwszPublicKeyToken;
      fixed (byte* numPtr = numArray)
      {
        if ((CapiNative.UnsafeNativeMethods._AxlPublicKeyBlobToPublicKeyToken(ref new CapiNative.CRYPTOAPI_BLOB()
        {
          cbData = numArray.Length,
          pbData = new IntPtr((void*) numPtr)
        }, out ppwszPublicKeyToken) & int.MinValue) != 0)
          return (byte[]) null;
      }
      bool success = false;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        ppwszPublicKeyToken.DangerousAddRef(ref success);
        return ManifestSignedXml.HexStringToBytes(Marshal.PtrToStringUni(ppwszPublicKeyToken.DangerousGetHandle()));
      }
      finally
      {
        if (success)
          ppwszPublicKeyToken.DangerousRelease();
      }
    }

    private static bool CompareBytes(byte[] lhs, byte[] rhs)
    {
      if (lhs == null || rhs == null)
        return false;
      for (int index = 0; index < lhs.Length; ++index)
      {
        if ((int) lhs[index] != (int) rhs[index])
          return false;
      }
      return true;
    }

    public override XmlElement GetIdElement(XmlDocument document, string idValue)
    {
      if (this.KeyInfo != null && string.Compare(this.KeyInfo.Id, idValue, StringComparison.OrdinalIgnoreCase) == 0)
        return this.KeyInfo.GetXml();
      else
        return (XmlElement) null;
    }

    [SecurityCritical]
    private TimestampInformation GetTimestampInformation(X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO timestamper, XmlElement licenseNode)
    {
      TimestampInformation timestampInformation = (TimestampInformation) null;
      if (timestamper.dwError == 0)
        timestampInformation = new TimestampInformation(timestamper);
      else if (timestamper.dwError == -2146762748 || timestamper.dwError == -2146762496)
      {
        XmlElement xmlElement = licenseNode.SelectSingleNode("r:issuer/ds:Signature/ds:Object/as:Timestamp", this.m_namespaceManager) as XmlElement;
        if (xmlElement != null)
        {
          byte[] encodedMessage = Convert.FromBase64String(xmlElement.InnerText);
          try
          {
            SignedCms signedCms = new SignedCms();
            signedCms.Decode(encodedMessage);
            signedCms.CheckSignature(true);
            timestampInformation = (TimestampInformation) null;
          }
          catch (CryptographicException ex)
          {
            timestampInformation = new TimestampInformation((SignatureVerificationResult) Marshal.GetHRForException((Exception) ex));
          }
        }
      }
      else
        timestampInformation = (TimestampInformation) null;
      return timestampInformation;
    }

    private static byte[] HexStringToBytes(string hex)
    {
      if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
        return (byte[]) null;
      byte[] numArray = new byte[hex.Length / 2];
      for (int index = 0; index < numArray.Length; ++index)
      {
        byte? nullable1 = ManifestSignedXml.HexToByte(hex[index]);
        byte? nullable2 = ManifestSignedXml.HexToByte(hex[index + 1]);
        if (!nullable1.HasValue || !nullable2.HasValue)
          return (byte[]) null;
        numArray[index] = (byte) ((uint) nullable1.Value << 4 | (uint) nullable2.Value);
      }
      return numArray;
    }

    private static byte? HexToByte(char hex)
    {
      if ((int) hex >= 48 && (int) hex <= 57)
        return new byte?((byte) ((uint) hex - 48U));
      if ((int) hex >= 97 && (int) hex <= 102)
        return new byte?((byte) ((int) hex - 97 + 10));
      if ((int) hex >= 65 && (int) hex <= 70)
        return new byte?((byte) ((int) hex - 65 + 10));
      else
        return new byte?();
    }

    private static X509Native.AxlVerificationFlags MapRevocationFlags(X509RevocationFlag revocationFlag, X509RevocationMode revocationMode)
    {
      X509Native.AxlVerificationFlags verificationFlags1 = X509Native.AxlVerificationFlags.None;
      X509Native.AxlVerificationFlags verificationFlags2;
      switch (revocationFlag)
      {
        case X509RevocationFlag.EndCertificateOnly:
          verificationFlags2 = verificationFlags1 | X509Native.AxlVerificationFlags.RevocationCheckEndCertOnly;
          break;
        case X509RevocationFlag.EntireChain:
          verificationFlags2 = verificationFlags1 | X509Native.AxlVerificationFlags.RevocationCheckEntireChain;
          break;
        default:
          verificationFlags2 = verificationFlags1;
          break;
      }
      X509Native.AxlVerificationFlags verificationFlags3;
      switch (revocationMode)
      {
        case X509RevocationMode.NoCheck:
          verificationFlags3 = verificationFlags2 | X509Native.AxlVerificationFlags.NoRevocationCheck;
          break;
        case X509RevocationMode.Offline:
          verificationFlags3 = verificationFlags2 | X509Native.AxlVerificationFlags.UrlOnlyCacheRetrieval;
          break;
        default:
          verificationFlags3 = verificationFlags2;
          break;
      }
      return verificationFlags3;
    }

    private SignatureVerificationResult VerifyAuthenticodeExpectedHash(XmlElement licenseNode)
    {
      XmlElement xmlElement1 = licenseNode.SelectSingleNode("r:grant/as:ManifestInformation", this.m_namespaceManager) as XmlElement;
      if (xmlElement1 == null)
        return SignatureVerificationResult.BadSignatureFormat;
      string attribute = xmlElement1.GetAttribute("Hash");
      if (string.IsNullOrEmpty(attribute))
        return SignatureVerificationResult.BadSignatureFormat;
      byte[] lhs = ManifestSignedXml.BackwardHexToBytes(attribute);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.PreserveWhitespace = true;
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.DtdProcessing = DtdProcessing.Parse;
      using (TextReader input = (TextReader) new StringReader(this.m_manifestXml.OuterXml))
      {
        using (XmlReader reader = XmlReader.Create(input, settings, this.m_manifestXml.BaseURI))
          xmlDocument.Load(reader);
      }
      XmlElement xmlElement2 = xmlDocument.SelectSingleNode("//asm:assembly/ds:Signature", this.m_namespaceManager) as XmlElement;
      xmlElement2.ParentNode.RemoveChild((XmlNode) xmlElement2);
      XmlDsigExcC14NTransform excC14Ntransform = new XmlDsigExcC14NTransform();
      excC14Ntransform.LoadInput((object) xmlDocument);
      byte[] rhs = (byte[]) null;
      using (SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider())
        rhs = cryptoServiceProvider.ComputeHash((Stream) (excC14Ntransform.GetOutput() as MemoryStream));
      return !ManifestSignedXml.CompareBytes(lhs, rhs) ? SignatureVerificationResult.BadDigest : SignatureVerificationResult.Valid;
    }

    [SecuritySafeCritical]
    private SignatureVerificationResult VerifyAuthenticodePublisher(X509Certificate2 publisherCertificate)
    {
      XmlElement xmlElement = this.m_manifestXml.SelectSingleNode("//asm:assembly/asmv2:publisherIdentity", this.m_namespaceManager) as XmlElement;
      if (xmlElement == null)
        return SignatureVerificationResult.BadSignatureFormat;
      string attribute1 = xmlElement.GetAttribute("name");
      string attribute2 = xmlElement.GetAttribute("issuerKeyHash");
      if (string.IsNullOrEmpty(attribute1) || string.IsNullOrEmpty(attribute2))
        return SignatureVerificationResult.BadSignatureFormat;
      SafeAxlBufferHandle ppwszPublicKeyHash = (SafeAxlBufferHandle) null;
      int issuerPublicKeyHash = X509Native.UnsafeNativeMethods._AxlGetIssuerPublicKeyHash(publisherCertificate.Handle, out ppwszPublicKeyHash);
      if (issuerPublicKeyHash != 0)
        return (SignatureVerificationResult) issuerPublicKeyHash;
      string strB = (string) null;
      bool success = false;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        ppwszPublicKeyHash.DangerousAddRef(ref success);
        strB = Marshal.PtrToStringUni(ppwszPublicKeyHash.DangerousGetHandle());
      }
      finally
      {
        if (success)
          ppwszPublicKeyHash.DangerousRelease();
      }
      return string.Compare(attribute1, publisherCertificate.SubjectName.Name, StringComparison.Ordinal) != 0 || string.Compare(attribute2, strB, StringComparison.Ordinal) != 0 ? SignatureVerificationResult.PublisherMismatch : SignatureVerificationResult.Valid;
    }

    [SecuritySafeCritical]
    private unsafe AuthenticodeSignatureInformation VerifyAuthenticodeSignature(XmlElement signatureNode, X509RevocationFlag revocationFlag, X509RevocationMode revocationMode)
    {
      XmlElement licenseNode = signatureNode.SelectSingleNode("ds:KeyInfo/msrel:RelData/r:license", this.m_namespaceManager) as XmlElement;
      if (licenseNode == null)
        return (AuthenticodeSignatureInformation) null;
      SignatureVerificationResult error1 = this.VerifyAuthenticodeSignatureIdentity(licenseNode);
      if (error1 != SignatureVerificationResult.Valid)
        return new AuthenticodeSignatureInformation(error1);
      SignatureVerificationResult error2 = this.VerifyAuthenticodeExpectedHash(licenseNode);
      if (error2 != SignatureVerificationResult.Valid)
        return new AuthenticodeSignatureInformation(error2);
      AuthenticodeSignatureInformation signatureInformation = (AuthenticodeSignatureInformation) null;
      X509Native.AXL_AUTHENTICODE_SIGNER_INFO pSignerInfo = new X509Native.AXL_AUTHENTICODE_SIGNER_INFO();
      pSignerInfo.cbSize = Marshal.SizeOf(typeof (X509Native.AXL_AUTHENTICODE_SIGNER_INFO));
      X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO pTimestamperInfo = new X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO();
      pTimestamperInfo.cbsize = Marshal.SizeOf(typeof (X509Native.AXL_AUTHENTICODE_TIMESTAMPER_INFO));
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        byte[] bytes = Encoding.UTF8.GetBytes(licenseNode.OuterXml);
        X509Native.AxlVerificationFlags dwFlags = ManifestSignedXml.MapRevocationFlags(revocationFlag, revocationMode);
        fixed (byte* numPtr = bytes)
        {
          if (X509Native.UnsafeNativeMethods.CertVerifyAuthenticodeLicense(ref new CapiNative.CRYPTOAPI_BLOB()
          {
            cbData = bytes.Length,
            pbData = new IntPtr((void*) numPtr)
          }, dwFlags, out pSignerInfo, out pTimestamperInfo) == -2146762496)
            return new AuthenticodeSignatureInformation(SignatureVerificationResult.MissingSignature);
        }
        X509Chain signatureChain = this.BuildSignatureChain(pSignerInfo, licenseNode, revocationFlag, revocationMode);
        TimestampInformation timestampInformation = this.GetTimestampInformation(pTimestamperInfo, licenseNode);
        signatureInformation = new AuthenticodeSignatureInformation(pSignerInfo, signatureChain, timestampInformation);
      }
      finally
      {
        X509Native.UnsafeNativeMethods.CertFreeAuthenticodeSignerInfo(ref pSignerInfo);
        X509Native.UnsafeNativeMethods.CertFreeAuthenticodeTimestamperInfo(ref pTimestamperInfo);
      }
      if (signatureInformation.SigningCertificate == null)
        return new AuthenticodeSignatureInformation(signatureInformation.VerificationResult);
      SignatureVerificationResult error3 = this.VerifyAuthenticodePublisher(signatureInformation.SigningCertificate);
      if (error3 != SignatureVerificationResult.Valid)
        return new AuthenticodeSignatureInformation(error3);
      else
        return signatureInformation;
    }

    private SignatureVerificationResult VerifyAuthenticodeSignatureIdentity(XmlElement licenseNode)
    {
      XmlElement xmlElement1 = licenseNode.SelectSingleNode("r:grant/as:ManifestInformation/as:assemblyIdentity", this.m_namespaceManager) as XmlElement;
      XmlElement xmlElement2 = this.m_manifestXml.SelectSingleNode("//asm:assembly/asm:assemblyIdentity", this.m_namespaceManager) as XmlElement;
      if (xmlElement2 == null || !xmlElement2.HasAttributes || (xmlElement1 == null || !xmlElement1.HasAttributes) || xmlElement2.Attributes.Count != xmlElement1.Attributes.Count)
        return SignatureVerificationResult.BadSignatureFormat;
      foreach (XmlAttribute xmlAttribute in (XmlNamedNodeMap) xmlElement2.Attributes)
      {
        string attribute = xmlElement1.GetAttribute(xmlAttribute.LocalName);
        if (attribute == null || string.Compare(xmlAttribute.Value, attribute, StringComparison.Ordinal) != 0)
          return SignatureVerificationResult.AssemblyIdentityMismatch;
      }
      return SignatureVerificationResult.Valid;
    }

    private static SignatureVerificationResult VerifyStrongNameSignatureId(XmlElement signatureNode)
    {
      string strA = (string) null;
      for (int index = 0; index < signatureNode.Attributes.Count && strA == null; ++index)
      {
        if (string.Compare(signatureNode.Attributes[index].LocalName, "id", StringComparison.OrdinalIgnoreCase) == 0)
          strA = signatureNode.Attributes[index].Value;
      }
      return string.IsNullOrEmpty(strA) || string.Compare(strA, "StrongNameSignature", StringComparison.Ordinal) != 0 ? SignatureVerificationResult.BadSignatureFormat : SignatureVerificationResult.Valid;
    }

    private static SignatureVerificationResult VerifyStrongNameSignatureTransforms(SignedInfo signedInfo)
    {
      int num = 0;
      foreach (Reference reference in signedInfo.References)
      {
        TransformChain transformChain = reference.TransformChain;
        bool flag;
        if (string.IsNullOrEmpty(reference.Uri))
        {
          ++num;
          flag = transformChain != null && transformChain.Count == 2 && string.Compare(transformChain[0].Algorithm, "http://www.w3.org/2000/09/xmldsig#enveloped-signature", StringComparison.Ordinal) == 0 && string.Compare(transformChain[1].Algorithm, "http://www.w3.org/2001/10/xml-exc-c14n#", StringComparison.Ordinal) == 0;
        }
        else if (string.Compare(reference.Uri, "#StrongNameKeyInfo", StringComparison.Ordinal) == 0)
        {
          ++num;
          flag = transformChain != null && transformChain.Count == 1 && string.Compare(transformChain[0].Algorithm, "http://www.w3.org/2001/10/xml-exc-c14n#", StringComparison.Ordinal) == 0;
        }
        else
          flag = true;
        if (!flag)
          return SignatureVerificationResult.BadSignatureFormat;
      }
      return num == 0 ? SignatureVerificationResult.BadSignatureFormat : SignatureVerificationResult.Valid;
    }

    private StrongNameSignatureInformation VerifyStrongNameSignature(XmlElement signatureNode)
    {
      AsymmetricAlgorithm signingKey;
      if (!this.CheckSignatureReturningKey(out signingKey))
        return new StrongNameSignatureInformation(SignatureVerificationResult.BadDigest);
      SignatureVerificationResult error1 = ManifestSignedXml.VerifyStrongNameSignatureId(signatureNode);
      if (error1 != SignatureVerificationResult.Valid)
        return new StrongNameSignatureInformation(error1);
      SignatureVerificationResult error2 = ManifestSignedXml.VerifyStrongNameSignatureTransforms(this.Signature.SignedInfo);
      if (error2 != SignatureVerificationResult.Valid)
        return new StrongNameSignatureInformation(error2);
      if (!ManifestSignedXml.CompareBytes(this.CalculateManifestPublicKeyToken(), ManifestSignedXml.CalculateSignerPublicKeyToken(signingKey)))
        return new StrongNameSignatureInformation(SignatureVerificationResult.PublicKeyTokenMismatch);
      else
        return new StrongNameSignatureInformation(signingKey);
    }

    public ManifestSignatureInformation VerifySignature(X509RevocationFlag revocationFlag, X509RevocationMode revocationMode)
    {
      XmlElement signatureNode = this.m_manifestXml.SelectSingleNode("//ds:Signature", this.m_namespaceManager) as XmlElement;
      if (signatureNode == null)
        return new ManifestSignatureInformation(this.m_manifest, (StrongNameSignatureInformation) null, (AuthenticodeSignatureInformation) null);
      this.LoadXml(signatureNode);
      StrongNameSignatureInformation strongNameSignature = this.VerifyStrongNameSignature(signatureNode);
      AuthenticodeSignatureInformation authenticodeSignature = strongNameSignature.VerificationResult == SignatureVerificationResult.BadDigest ? new AuthenticodeSignatureInformation(SignatureVerificationResult.ContainingSignatureInvalid) : this.VerifyAuthenticodeSignature(signatureNode, revocationFlag, revocationMode);
      return new ManifestSignatureInformation(this.m_manifest, strongNameSignature, authenticodeSignature);
    }
  }
}
