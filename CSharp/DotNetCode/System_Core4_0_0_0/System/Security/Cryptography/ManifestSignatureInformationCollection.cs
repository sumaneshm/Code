// Type: System.Security.Cryptography.ManifestSignatureInformationCollection
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class ManifestSignatureInformationCollection : ReadOnlyCollection<ManifestSignatureInformation>
  {
    internal ManifestSignatureInformationCollection(IList<ManifestSignatureInformation> signatureInformation)
      : base(signatureInformation)
    {
    }
  }
}
