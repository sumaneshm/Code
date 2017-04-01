// Type: System.IO.Pipes.PipeAuditRule
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.IO.Pipes
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class PipeAuditRule : AuditRule
  {
    public PipeAccessRights PipeAccessRights
    {
      get
      {
        return PipeAccessRule.RightsFromAccessMask(this.AccessMask);
      }
    }

    public PipeAuditRule(IdentityReference identity, PipeAccessRights rights, AuditFlags flags)
      : this(identity, PipeAuditRule.AccessMaskFromRights(rights), false, flags)
    {
    }

    public PipeAuditRule(string identity, PipeAccessRights rights, AuditFlags flags)
      : this((IdentityReference) new NTAccount(identity), PipeAuditRule.AccessMaskFromRights(rights), false, flags)
    {
    }

    internal PipeAuditRule(IdentityReference identity, int accessMask, bool isInherited, AuditFlags flags)
      : base(identity, accessMask, isInherited, InheritanceFlags.None, PropagationFlags.None, flags)
    {
    }

    private static int AccessMaskFromRights(PipeAccessRights rights)
    {
      if (rights < (PipeAccessRights) 0 || rights > (PipeAccessRights.FullControl | PipeAccessRights.AccessSystemSecurity))
        throw new ArgumentOutOfRangeException("rights", SR.GetString("ArgumentOutOfRange_NeedValidPipeAccessRights"));
      else
        return (int) rights;
    }
  }
}
