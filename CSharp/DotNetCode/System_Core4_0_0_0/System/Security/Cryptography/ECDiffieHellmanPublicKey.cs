// Type: System.Security.Cryptography.ECDiffieHellmanPublicKey
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
  public abstract class ECDiffieHellmanPublicKey : IDisposable
  {
    private byte[] m_keyBlob;

    protected ECDiffieHellmanPublicKey(byte[] keyBlob)
    {
      if (keyBlob == null)
        throw new ArgumentNullException("keyBlob");
      this.m_keyBlob = keyBlob.Clone() as byte[];
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Dispose()
    {
      this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public virtual byte[] ToByteArray()
    {
      return this.m_keyBlob.Clone() as byte[];
    }

    public abstract string ToXmlString();
  }
}
