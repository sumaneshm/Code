// Type: System.IO.Pipes.PipeAccessRule
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
  public sealed class PipeAccessRule : AccessRule
  {
    public PipeAccessRights PipeAccessRights
    {
      get
      {
        return PipeAccessRule.RightsFromAccessMask(this.AccessMask);
      }
    }

    public PipeAccessRule(string identity, PipeAccessRights rights, AccessControlType type)
      : this((IdentityReference) new NTAccount(identity), PipeAccessRule.AccessMaskFromRights(rights, type), false, type)
    {
    }

    public PipeAccessRule(IdentityReference identity, PipeAccessRights rights, AccessControlType type)
      : this(identity, PipeAccessRule.AccessMaskFromRights(rights, type), false, type)
    {
    }

    internal PipeAccessRule(IdentityReference identity, int accessMask, bool isInherited, AccessControlType type)
      : base(identity, accessMask, isInherited, InheritanceFlags.None, PropagationFlags.None, type)
    {
    }

    internal static int AccessMaskFromRights(PipeAccessRights rights, AccessControlType controlType)
    {
      if (rights < (PipeAccessRights) 0 || rights > (PipeAccessRights.FullControl | PipeAccessRights.AccessSystemSecurity))
        throw new ArgumentOutOfRangeException("rights", SR.GetString("ArgumentOutOfRange_NeedValidPipeAccessRights"));
      if (controlType == AccessControlType.Allow)
        rights |= PipeAccessRights.Synchronize;
      else if (controlType == AccessControlType.Deny && rights != PipeAccessRights.FullControl)
        rights &= ~PipeAccessRights.Synchronize;
      return (int) rights;
    }

    internal static PipeAccessRights RightsFromAccessMask(int accessMask)
    {
      return (PipeAccessRights) accessMask;
    }
  }
}
