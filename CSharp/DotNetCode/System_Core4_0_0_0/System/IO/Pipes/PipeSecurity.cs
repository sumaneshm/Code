// Type: System.IO.Pipes.PipeSecurity
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.IO.Pipes
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class PipeSecurity : NativeObjectSecurity
  {
    public override Type AccessRightType
    {
      get
      {
        return typeof (PipeAccessRights);
      }
    }

    public override Type AccessRuleType
    {
      get
      {
        return typeof (PipeAccessRule);
      }
    }

    public override Type AuditRuleType
    {
      get
      {
        return typeof (PipeAuditRule);
      }
    }

    public PipeSecurity()
      : base(false, ResourceType.KernelObject)
    {
    }

    [SecuritySafeCritical]
    internal PipeSecurity(SafePipeHandle safeHandle, AccessControlSections includeSections)
      : base(false, ResourceType.KernelObject, (SafeHandle) safeHandle, includeSections)
    {
    }

    public void AddAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException("rule");
      base.AddAccessRule((AccessRule) rule);
    }

    public void SetAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException("rule");
      base.SetAccessRule((AccessRule) rule);
    }

    public void ResetAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException("rule");
      base.ResetAccessRule((AccessRule) rule);
    }

    public bool RemoveAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException("rule");
      AuthorizationRuleCollection accessRules = this.GetAccessRules(true, true, rule.IdentityReference.GetType());
      for (int index = 0; index < accessRules.Count; ++index)
      {
        PipeAccessRule pipeAccessRule = accessRules[index] as PipeAccessRule;
        if (pipeAccessRule != null && pipeAccessRule.PipeAccessRights == rule.PipeAccessRights && (pipeAccessRule.IdentityReference == rule.IdentityReference && pipeAccessRule.AccessControlType == rule.AccessControlType))
          return base.RemoveAccessRule((AccessRule) rule);
      }
      if (rule.PipeAccessRights != PipeAccessRights.FullControl)
        return base.RemoveAccessRule((AccessRule) new PipeAccessRule(rule.IdentityReference, PipeAccessRule.AccessMaskFromRights(rule.PipeAccessRights, AccessControlType.Deny), false, rule.AccessControlType));
      else
        return base.RemoveAccessRule((AccessRule) rule);
    }

    public void RemoveAccessRuleSpecific(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException("rule");
      AuthorizationRuleCollection accessRules = this.GetAccessRules(true, true, rule.IdentityReference.GetType());
      for (int index = 0; index < accessRules.Count; ++index)
      {
        PipeAccessRule pipeAccessRule = accessRules[index] as PipeAccessRule;
        if (pipeAccessRule != null && pipeAccessRule.PipeAccessRights == rule.PipeAccessRights && (pipeAccessRule.IdentityReference == rule.IdentityReference && pipeAccessRule.AccessControlType == rule.AccessControlType))
        {
          base.RemoveAccessRuleSpecific((AccessRule) rule);
          return;
        }
      }
      if (rule.PipeAccessRights != PipeAccessRights.FullControl)
        base.RemoveAccessRuleSpecific((AccessRule) new PipeAccessRule(rule.IdentityReference, PipeAccessRule.AccessMaskFromRights(rule.PipeAccessRights, AccessControlType.Deny), false, rule.AccessControlType));
      else
        base.RemoveAccessRuleSpecific((AccessRule) rule);
    }

    public void AddAuditRule(PipeAuditRule rule)
    {
      base.AddAuditRule((AuditRule) rule);
    }

    public void SetAuditRule(PipeAuditRule rule)
    {
      base.SetAuditRule((AuditRule) rule);
    }

    public bool RemoveAuditRule(PipeAuditRule rule)
    {
      return base.RemoveAuditRule((AuditRule) rule);
    }

    public void RemoveAuditRuleAll(PipeAuditRule rule)
    {
      base.RemoveAuditRuleAll((AuditRule) rule);
    }

    public void RemoveAuditRuleSpecific(PipeAuditRule rule)
    {
      base.RemoveAuditRuleSpecific((AuditRule) rule);
    }

    public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
    {
      if (inheritanceFlags != InheritanceFlags.None)
        throw new ArgumentException(SR.GetString("Argument_NonContainerInvalidAnyFlag"), "inheritanceFlags");
      if (propagationFlags != PropagationFlags.None)
        throw new ArgumentException(SR.GetString("Argument_NonContainerInvalidAnyFlag"), "propagationFlags");
      else
        return (AccessRule) new PipeAccessRule(identityReference, accessMask, isInherited, type);
    }

    public override sealed AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
    {
      if (inheritanceFlags != InheritanceFlags.None)
        throw new ArgumentException(SR.GetString("Argument_NonContainerInvalidAnyFlag"), "inheritanceFlags");
      if (propagationFlags != PropagationFlags.None)
        throw new ArgumentException(SR.GetString("Argument_NonContainerInvalidAnyFlag"), "propagationFlags");
      else
        return (AuditRule) new PipeAuditRule(identityReference, accessMask, isInherited, flags);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    protected internal void Persist(SafeHandle handle)
    {
      this.WriteLock();
      try
      {
        AccessControlSections sectionsFromChanges = this.GetAccessControlSectionsFromChanges();
        base.Persist(handle, sectionsFromChanges);
        this.OwnerModified = this.GroupModified = this.AuditRulesModified = this.AccessRulesModified = false;
      }
      finally
      {
        this.WriteUnlock();
      }
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    protected internal void Persist(string name)
    {
      this.WriteLock();
      try
      {
        AccessControlSections sectionsFromChanges = this.GetAccessControlSectionsFromChanges();
        base.Persist(name, sectionsFromChanges);
        this.OwnerModified = this.GroupModified = this.AuditRulesModified = this.AccessRulesModified = false;
      }
      finally
      {
        this.WriteUnlock();
      }
    }

    private AccessControlSections GetAccessControlSectionsFromChanges()
    {
      AccessControlSections accessControlSections = AccessControlSections.None;
      if (this.AccessRulesModified)
        accessControlSections = AccessControlSections.Access;
      if (this.AuditRulesModified)
        accessControlSections |= AccessControlSections.Audit;
      if (this.OwnerModified)
        accessControlSections |= AccessControlSections.Owner;
      if (this.GroupModified)
        accessControlSections |= AccessControlSections.Group;
      return accessControlSections;
    }
  }
}
