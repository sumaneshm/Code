// Type: System.Security.Cryptography.Rfc4050KeyFormatter
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace System.Security.Cryptography
{
  internal static class Rfc4050KeyFormatter
  {
    private const string DomainParametersRoot = "DomainParameters";
    private const string ECDHRoot = "ECDHKeyValue";
    private const string ECDsaRoot = "ECDSAKeyValue";
    private const string NamedCurveElement = "NamedCurve";
    private const string Namespace = "http://www.w3.org/2001/04/xmldsig-more#";
    private const string PublicKeyRoot = "PublicKey";
    private const string UrnAttribute = "URN";
    private const string ValueAttribute = "Value";
    private const string XElement = "X";
    private const string YElement = "Y";
    private const string XsiTypeAttribute = "type";
    private const string XsiTypeAttributeValue = "PrimeFieldElemType";
    private const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
    private const string XsiNamespacePrefix = "xsi";
    private const string Prime256CurveUrn = "urn:oid:1.2.840.10045.3.1.7";
    private const string Prime384CurveUrn = "urn:oid:1.3.132.0.34";
    private const string Prime521CurveUrn = "urn:oid:1.3.132.0.35";

    internal static CngKey FromXml(string xml)
    {
      using (TextReader input = (TextReader) new StringReader(xml))
      {
        using (XmlTextReader xmlTextReader = new XmlTextReader(input))
        {
          XPathNavigator navigator = new XPathDocument((XmlReader) xmlTextReader).CreateNavigator();
          if (!navigator.MoveToFirstChild())
            throw new ArgumentException(SR.GetString("Cryptography_MissingDomainParameters"));
          CngAlgorithm cngAlgorithm = Rfc4050KeyFormatter.ReadAlgorithm(navigator);
          if (!navigator.MoveToNext(XPathNodeType.Element))
            throw new ArgumentException(SR.GetString("Cryptography_MissingPublicKey"));
          BigInteger x;
          BigInteger y;
          Rfc4050KeyFormatter.ReadPublicKey(navigator, out x, out y);
          return CngKey.Import(NCryptNative.BuildEccPublicBlob(cngAlgorithm.Algorithm, x, y), CngKeyBlobFormat.EccPublicBlob);
        }
      }
    }

    private static int GetKeySize(string urn)
    {
      switch (urn)
      {
        case "urn:oid:1.2.840.10045.3.1.7":
          return 256;
        case "urn:oid:1.3.132.0.34":
          return 384;
        case "urn:oid:1.3.132.0.35":
          return 521;
        default:
          throw new ArgumentException(SR.GetString("Cryptography_UnknownEllipticCurve"), "algorithm");
      }
    }

    private static string GetCurveUrn(CngAlgorithm algorithm)
    {
      if (algorithm == CngAlgorithm.ECDsaP256 || algorithm == CngAlgorithm.ECDiffieHellmanP256)
        return "urn:oid:1.2.840.10045.3.1.7";
      if (algorithm == CngAlgorithm.ECDsaP384 || algorithm == CngAlgorithm.ECDiffieHellmanP384)
        return "urn:oid:1.3.132.0.34";
      if (algorithm == CngAlgorithm.ECDsaP521 || algorithm == CngAlgorithm.ECDiffieHellmanP521)
        return "urn:oid:1.3.132.0.35";
      else
        throw new ArgumentException(SR.GetString("Cryptography_UnknownEllipticCurve"), "algorithm");
    }

    private static CngAlgorithm ReadAlgorithm(XPathNavigator navigator)
    {
      if (navigator.NamespaceURI != "http://www.w3.org/2001/04/xmldsig-more#")
      {
        throw new ArgumentException(SR.GetString("Cryptography_UnexpectedXmlNamespace", (object) navigator.NamespaceURI, (object) "http://www.w3.org/2001/04/xmldsig-more#"));
      }
      else
      {
        bool flag1 = navigator.Name == "ECDHKeyValue";
        bool flag2 = navigator.Name == "ECDSAKeyValue";
        if (!flag1 && !flag2)
          throw new ArgumentException(SR.GetString("Cryptography_UnknownEllipticCurveAlgorithm"));
        if (!navigator.MoveToFirstChild() || navigator.Name != "DomainParameters")
          throw new ArgumentException(SR.GetString("Cryptography_MissingDomainParameters"));
        if (!navigator.MoveToFirstChild() || navigator.Name != "NamedCurve")
          throw new ArgumentException(SR.GetString("Cryptography_MissingDomainParameters"));
        if (!navigator.MoveToFirstAttribute() || navigator.Name != "URN" || string.IsNullOrEmpty(navigator.Value))
          throw new ArgumentException(SR.GetString("Cryptography_MissingDomainParameters"));
        int keySize = Rfc4050KeyFormatter.GetKeySize(navigator.Value);
        navigator.MoveToParent();
        navigator.MoveToParent();
        if (flag1)
        {
          if (keySize == 256)
            return CngAlgorithm.ECDiffieHellmanP256;
          if (keySize == 384)
            return CngAlgorithm.ECDiffieHellmanP384;
          else
            return CngAlgorithm.ECDiffieHellmanP521;
        }
        else
        {
          if (keySize == 256)
            return CngAlgorithm.ECDsaP256;
          if (keySize == 384)
            return CngAlgorithm.ECDsaP384;
          else
            return CngAlgorithm.ECDsaP521;
        }
      }
    }

    private static void ReadPublicKey(XPathNavigator navigator, out BigInteger x, out BigInteger y)
    {
      if (navigator.NamespaceURI != "http://www.w3.org/2001/04/xmldsig-more#")
      {
        throw new ArgumentException(SR.GetString("Cryptography_UnexpectedXmlNamespace", (object) navigator.NamespaceURI, (object) "http://www.w3.org/2001/04/xmldsig-more#"));
      }
      else
      {
        if (navigator.Name != "PublicKey")
          throw new ArgumentException(SR.GetString("Cryptography_MissingPublicKey"));
        if (!navigator.MoveToFirstChild() || navigator.Name != "X")
          throw new ArgumentException(SR.GetString("Cryptography_MissingPublicKey"));
        if (!navigator.MoveToFirstAttribute() || navigator.Name != "Value" || string.IsNullOrEmpty(navigator.Value))
          throw new ArgumentException(SR.GetString("Cryptography_MissingPublicKey"));
        x = BigInteger.Parse(navigator.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        navigator.MoveToParent();
        if (!navigator.MoveToNext(XPathNodeType.Element) || navigator.Name != "Y")
          throw new ArgumentException(SR.GetString("Cryptography_MissingPublicKey"));
        if (!navigator.MoveToFirstAttribute() || navigator.Name != "Value" || string.IsNullOrEmpty(navigator.Value))
          throw new ArgumentException(SR.GetString("Cryptography_MissingPublicKey"));
        y = BigInteger.Parse(navigator.Value, (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    private static void WriteDomainParameters(XmlWriter writer, CngKey key)
    {
      writer.WriteStartElement("DomainParameters");
      writer.WriteStartElement("NamedCurve");
      writer.WriteAttributeString("URN", Rfc4050KeyFormatter.GetCurveUrn(key.Algorithm));
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private static void WritePublicKeyValue(XmlWriter writer, CngKey key)
    {
      writer.WriteStartElement("PublicKey");
      BigInteger x;
      BigInteger y;
      NCryptNative.UnpackEccPublicBlob(key.Export(CngKeyBlobFormat.EccPublicBlob), out x, out y);
      writer.WriteStartElement("X");
      writer.WriteAttributeString("Value", x.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "PrimeFieldElemType");
      writer.WriteEndElement();
      writer.WriteStartElement("Y");
      writer.WriteAttributeString("Value", y.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "PrimeFieldElemType");
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    internal static string ToXml(CngKey key)
    {
      StringBuilder output = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        OmitXmlDeclaration = true
      }))
      {
        string localName = key.AlgorithmGroup == CngAlgorithmGroup.ECDsa ? "ECDSAKeyValue" : "ECDHKeyValue";
        writer.WriteStartElement(localName, "http://www.w3.org/2001/04/xmldsig-more#");
        Rfc4050KeyFormatter.WriteDomainParameters(writer, key);
        Rfc4050KeyFormatter.WritePublicKeyValue(writer, key);
        writer.WriteEndElement();
      }
      return ((object) output).ToString();
    }
  }
}
