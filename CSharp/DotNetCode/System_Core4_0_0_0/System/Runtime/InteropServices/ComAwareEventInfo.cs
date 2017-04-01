// Type: System.Runtime.InteropServices.ComAwareEventInfo
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
  [SecuritySafeCritical]
  [__DynamicallyInvokable]
  public class ComAwareEventInfo : EventInfo
  {
    private EventInfo _innerEventInfo;

    [__DynamicallyInvokable]
    public override EventAttributes Attributes
    {
      [__DynamicallyInvokable] get
      {
        return this._innerEventInfo.Attributes;
      }
    }

    [__DynamicallyInvokable]
    public override Type DeclaringType
    {
      [__DynamicallyInvokable] get
      {
        return this._innerEventInfo.DeclaringType;
      }
    }

    [__DynamicallyInvokable]
    public override string Name
    {
      [__DynamicallyInvokable] get
      {
        return this._innerEventInfo.Name;
      }
    }

    [__DynamicallyInvokable]
    public override Type ReflectedType
    {
      [__DynamicallyInvokable] get
      {
        return this._innerEventInfo.ReflectedType;
      }
    }

    [__DynamicallyInvokable]
    public ComAwareEventInfo(Type type, string eventName)
    {
      this._innerEventInfo = type.GetEvent(eventName);
    }

    [__DynamicallyInvokable]
    public override void AddEventHandler(object target, Delegate handler)
    {
      if (Marshal.IsComObject(target))
      {
        Guid sourceIid;
        int dispid;
        ComAwareEventInfo.GetDataForComInvocation(this._innerEventInfo, out sourceIid, out dispid);
        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        ComEventsHelper.Combine(target, sourceIid, dispid, handler);
      }
      else
        this._innerEventInfo.AddEventHandler(target, handler);
    }

    [__DynamicallyInvokable]
    public override void RemoveEventHandler(object target, Delegate handler)
    {
      if (Marshal.IsComObject(target))
      {
        Guid sourceIid;
        int dispid;
        ComAwareEventInfo.GetDataForComInvocation(this._innerEventInfo, out sourceIid, out dispid);
        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        ComEventsHelper.Remove(target, sourceIid, dispid, handler);
      }
      else
        this._innerEventInfo.RemoveEventHandler(target, handler);
    }

    [__DynamicallyInvokable]
    public override MethodInfo GetAddMethod(bool nonPublic)
    {
      return this._innerEventInfo.GetAddMethod(nonPublic);
    }

    [__DynamicallyInvokable]
    public override MethodInfo GetRaiseMethod(bool nonPublic)
    {
      return this._innerEventInfo.GetRaiseMethod(nonPublic);
    }

    [__DynamicallyInvokable]
    public override MethodInfo GetRemoveMethod(bool nonPublic)
    {
      return this._innerEventInfo.GetRemoveMethod(nonPublic);
    }

    [__DynamicallyInvokable]
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return this._innerEventInfo.GetCustomAttributes(attributeType, inherit);
    }

    [__DynamicallyInvokable]
    public override object[] GetCustomAttributes(bool inherit)
    {
      return this._innerEventInfo.GetCustomAttributes(inherit);
    }

    [__DynamicallyInvokable]
    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this._innerEventInfo.IsDefined(attributeType, inherit);
    }

    private static void GetDataForComInvocation(EventInfo eventInfo, out Guid sourceIid, out int dispid)
    {
      object[] customAttributes = eventInfo.DeclaringType.GetCustomAttributes(typeof (ComEventInterfaceAttribute), false);
      if (customAttributes == null || customAttributes.Length == 0)
        throw new InvalidOperationException("event invocation for COM objects requires interface to be attributed with ComSourceInterfaceGuidAttribute");
      if (customAttributes.Length > 1)
        throw new AmbiguousMatchException("more than one ComSourceInterfaceGuidAttribute found");
      Type sourceInterface = ((ComEventInterfaceAttribute) customAttributes[0]).SourceInterface;
      Guid guid = sourceInterface.GUID;
      Attribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) sourceInterface.GetMethod(eventInfo.Name), typeof (DispIdAttribute));
      if (customAttribute == null)
        throw new InvalidOperationException("event invocation for COM objects requires event to be attributed with DispIdAttribute");
      sourceIid = guid;
      dispid = ((DispIdAttribute) customAttribute).Value;
    }
  }
}
