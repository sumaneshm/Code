// Type: System.Security.Cryptography.CngProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CngProvider : IEquatable<CngProvider>
  {
    private static volatile CngProvider s_msSmartCardKsp;
    private static volatile CngProvider s_msSoftwareKsp;
    private string m_provider;

    public string Provider
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_provider;
      }
    }

    public static CngProvider MicrosoftSmartCardKeyStorageProvider
    {
      get
      {
        if (CngProvider.s_msSmartCardKsp == (CngProvider) null)
          CngProvider.s_msSmartCardKsp = new CngProvider("Microsoft Smart Card Key Storage Provider");
        return CngProvider.s_msSmartCardKsp;
      }
    }

    public static CngProvider MicrosoftSoftwareKeyStorageProvider
    {
      get
      {
        if (CngProvider.s_msSoftwareKsp == (CngProvider) null)
          CngProvider.s_msSoftwareKsp = new CngProvider("Microsoft Software Key Storage Provider");
        return CngProvider.s_msSoftwareKsp;
      }
    }

    public CngProvider(string provider)
    {
      if (provider == null)
        throw new ArgumentNullException("provider");
      if (provider.Length == 0)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidProviderName", new object[1]
        {
          (object) provider
        }), "provider");
      else
        this.m_provider = provider;
    }

    public static bool operator ==(CngProvider left, CngProvider right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return object.ReferenceEquals((object) right, (object) null);
      else
        return left.Equals(right);
    }

    public static bool operator !=(CngProvider left, CngProvider right)
    {
      if (object.ReferenceEquals((object) left, (object) null))
        return !object.ReferenceEquals((object) right, (object) null);
      else
        return !left.Equals(right);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as CngProvider);
    }

    public bool Equals(CngProvider other)
    {
      if (object.ReferenceEquals((object) other, (object) null))
        return false;
      else
        return this.m_provider.Equals(other.Provider);
    }

    public override int GetHashCode()
    {
      return this.m_provider.GetHashCode();
    }

    public override string ToString()
    {
      return ((object) this.m_provider).ToString();
    }
  }
}
