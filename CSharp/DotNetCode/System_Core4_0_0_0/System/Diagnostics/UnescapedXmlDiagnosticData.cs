// Type: System.Diagnostics.UnescapedXmlDiagnosticData
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Security.Permissions;

namespace System.Diagnostics
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class UnescapedXmlDiagnosticData
  {
    private string _xmlString;

    public string UnescapedXml
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._xmlString;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._xmlString = value;
      }
    }

    public UnescapedXmlDiagnosticData(string xmlPayload)
    {
      this._xmlString = xmlPayload;
      if (this._xmlString != null)
        return;
      this._xmlString = string.Empty;
    }

    public override string ToString()
    {
      return this._xmlString;
    }
  }
}
