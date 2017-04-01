// Type: System.Diagnostics.Eventing.Reader.NativeWrapper
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System.Diagnostics.Eventing.Reader
{
  internal class NativeWrapper
  {
    private static bool s_platformNotSupported = Environment.OSVersion.Version.Major < 6;

    static NativeWrapper()
    {
    }

    [SecurityCritical]
    public static EventLogHandle EvtQuery(EventLogHandle session, string path, string query, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtQuery(session, path, query, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static void EvtSeek(EventLogHandle resultSet, long position, EventLogHandle bookmark, int timeout, Microsoft.Win32.UnsafeNativeMethods.EvtSeekFlags flags)
    {
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtSeek(resultSet, position, bookmark, timeout, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecurityCritical]
    public static EventLogHandle EvtSubscribe(EventLogHandle session, SafeWaitHandle signalEvent, string path, string query, EventLogHandle bookmark, IntPtr context, IntPtr callback, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtSubscribe(session, signalEvent, path, query, bookmark, context, callback, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static bool EvtNext(EventLogHandle queryHandle, int eventSize, IntPtr[] events, int timeout, int flags, ref int returned)
    {
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtNext(queryHandle, eventSize, events, timeout, flags, ref returned);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (!flag && lastWin32Error != 259)
        EventLogException.Throw(lastWin32Error);
      return lastWin32Error == 0;
    }

    [SecuritySafeCritical]
    public static void EvtCancel(EventLogHandle handle)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (Microsoft.Win32.UnsafeNativeMethods.EvtCancel(handle))
        return;
      EventLogException.Throw(Marshal.GetLastWin32Error());
    }

    [SecurityCritical]
    public static void EvtClose(IntPtr handle)
    {
      Microsoft.Win32.UnsafeNativeMethods.EvtClose(handle);
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenProviderMetadata(EventLogHandle session, string ProviderId, string logFilePath, int locale, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenPublisherMetadata(session, ProviderId, logFilePath, 0, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static int EvtGetObjectArraySize(EventLogHandle objectArray)
    {
      int objectArraySize1;
      bool objectArraySize2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetObjectArraySize(objectArray, out objectArraySize1);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (!objectArraySize2)
        EventLogException.Throw(lastWin32Error);
      return objectArraySize1;
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenEventMetadataEnum(EventLogHandle ProviderMetadata, int flags)
    {
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenEventMetadataEnum(ProviderMetadata, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static EventLogHandle EvtNextEventMetadata(EventLogHandle eventMetadataEnum, int flags)
    {
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtNextEventMetadata(eventMetadataEnum, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (!eventLogHandle.IsInvalid)
        return eventLogHandle;
      if (lastWin32Error != 259)
        EventLogException.Throw(lastWin32Error);
      return (EventLogHandle) null;
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenChannelEnum(EventLogHandle session, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenChannelEnum(session, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenProviderEnum(EventLogHandle session, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenPublisherEnum(session, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenChannelConfig(EventLogHandle session, string channelPath, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenChannelConfig(session, channelPath, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecuritySafeCritical]
    public static void EvtSaveChannelConfig(EventLogHandle channelConfig, int flags)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtSaveChannelConfig(channelConfig, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenLog(EventLogHandle session, string path, PathType flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenLog(session, path, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecuritySafeCritical]
    public static void EvtExportLog(EventLogHandle session, string channelPath, string query, string targetFilePath, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtExportLog(session, channelPath, query, targetFilePath, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecuritySafeCritical]
    public static void EvtArchiveExportedLog(EventLogHandle session, string logFilePath, int locale, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtArchiveExportedLog(session, logFilePath, locale, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecuritySafeCritical]
    public static void EvtClearLog(EventLogHandle session, string channelPath, string targetFilePath, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtClearLog(session, channelPath, targetFilePath, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecurityCritical]
    public static EventLogHandle EvtCreateRenderContext(int valuePathsCount, string[] valuePaths, Microsoft.Win32.UnsafeNativeMethods.EvtRenderContextFlags flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle renderContext = Microsoft.Win32.UnsafeNativeMethods.EvtCreateRenderContext(valuePathsCount, valuePaths, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (renderContext.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return renderContext;
    }

    [SecurityCritical]
    public static void EvtRender(EventLogHandle context, EventLogHandle eventHandle, Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags flags, StringBuilder buffer)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      int buffUsed;
      int propCount;
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtRender(context, eventHandle, flags, buffer.Capacity, buffer, out buffUsed, out propCount);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      if (lastWin32Error == 122)
      {
        buffer.Capacity = buffUsed;
        flag = Microsoft.Win32.UnsafeNativeMethods.EvtRender(context, eventHandle, flags, buffer.Capacity, buffer, out buffUsed, out propCount);
        lastWin32Error = Marshal.GetLastWin32Error();
      }
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecurityCritical]
    public static EventLogHandle EvtOpenSession(Microsoft.Win32.UnsafeNativeMethods.EvtLoginClass loginClass, ref Microsoft.Win32.UnsafeNativeMethods.EvtRpcLogin login, int timeout, int flags)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle eventLogHandle = Microsoft.Win32.UnsafeNativeMethods.EvtOpenSession(loginClass, ref login, timeout, flags);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (eventLogHandle.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return eventLogHandle;
    }

    [SecurityCritical]
    public static EventLogHandle EvtCreateBookmark(string bookmarkXml)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogHandle bookmark = Microsoft.Win32.UnsafeNativeMethods.EvtCreateBookmark(bookmarkXml);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (bookmark.IsInvalid)
        EventLogException.Throw(lastWin32Error);
      return bookmark;
    }

    [SecurityCritical]
    public static void EvtUpdateBookmark(EventLogHandle bookmark, EventLogHandle eventHandle)
    {
      bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtUpdateBookmark(bookmark, eventHandle);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (flag)
        return;
      EventLogException.Throw(lastWin32Error);
    }

    [SecuritySafeCritical]
    public static object EvtGetEventInfo(EventLogHandle handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventPropertyId enumType)
    {
      IntPtr num = IntPtr.Zero;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      try
      {
        int bufferUsed;
        bool eventInfo1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetEventInfo(handle, enumType, 0, IntPtr.Zero, out bufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!eventInfo1 && lastWin32Error1 != 0 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(bufferUsed);
        bool eventInfo2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetEventInfo(handle, enumType, bufferUsed, num, out bufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!eventInfo2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecurityCritical]
    public static object EvtGetQueryInfo(EventLogHandle handle, Microsoft.Win32.UnsafeNativeMethods.EvtQueryPropertyId enumType)
    {
      IntPtr num = IntPtr.Zero;
      int bufferRequired = 0;
      try
      {
        bool queryInfo1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetQueryInfo(handle, enumType, 0, IntPtr.Zero, ref bufferRequired);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!queryInfo1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(bufferRequired);
        bool queryInfo2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetQueryInfo(handle, enumType, bufferRequired, num, ref bufferRequired);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!queryInfo2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecuritySafeCritical]
    public static object EvtGetPublisherMetadataProperty(EventLogHandle pmHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId thePropertyId)
    {
      IntPtr num = IntPtr.Zero;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      try
      {
        int publisherMetadataPropertyBufferUsed;
        bool metadataProperty1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetPublisherMetadataProperty(pmHandle, thePropertyId, 0, 0, IntPtr.Zero, out publisherMetadataPropertyBufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!metadataProperty1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(publisherMetadataPropertyBufferUsed);
        bool metadataProperty2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetPublisherMetadataProperty(pmHandle, thePropertyId, 0, publisherMetadataPropertyBufferUsed, num, out publisherMetadataPropertyBufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!metadataProperty2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecurityCritical]
    internal static EventLogHandle EvtGetPublisherMetadataPropertyHandle(EventLogHandle pmHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId thePropertyId)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        int publisherMetadataPropertyBufferUsed;
        bool metadataProperty1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetPublisherMetadataProperty(pmHandle, thePropertyId, 0, 0, IntPtr.Zero, out publisherMetadataPropertyBufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!metadataProperty1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(publisherMetadataPropertyBufferUsed);
        bool metadataProperty2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetPublisherMetadataProperty(pmHandle, thePropertyId, 0, publisherMetadataPropertyBufferUsed, num, out publisherMetadataPropertyBufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!metadataProperty2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToSafeHandle((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecurityCritical]
    public static string EvtFormatMessage(EventLogHandle handle, uint msgId)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      StringBuilder buffer = new StringBuilder((string) null);
      int bufferUsed;
      bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessage(handle, EventLogHandle.Zero, msgId, 0, (Microsoft.Win32.UnsafeNativeMethods.EvtStringVariant[]) null, Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageId, 0, buffer, out bufferUsed);
      int lastWin32Error1 = Marshal.GetLastWin32Error();
      if (!flag1 && lastWin32Error1 != 15029 && (lastWin32Error1 != 15030 && lastWin32Error1 != 15031))
      {
        switch (lastWin32Error1)
        {
          case 15033:
          case 15100:
          case 1815:
          case 15027:
          case 15028:
            return (string) null;
          case 122:
            break;
          default:
            EventLogException.Throw(lastWin32Error1);
            break;
        }
      }
      buffer.EnsureCapacity(bufferUsed);
      bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessage(handle, EventLogHandle.Zero, msgId, 0, (Microsoft.Win32.UnsafeNativeMethods.EvtStringVariant[]) null, Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageId, bufferUsed, buffer, out bufferUsed);
      int lastWin32Error2 = Marshal.GetLastWin32Error();
      if (!flag2 && lastWin32Error2 != 15029 && (lastWin32Error2 != 15030 && lastWin32Error2 != 15031))
      {
        switch (lastWin32Error2)
        {
          case 15033:
          case 15100:
          case 1815:
          case 15027:
          case 15028:
            return (string) null;
          case 15029:
            return (string) null;
          default:
            EventLogException.Throw(lastWin32Error2);
            break;
        }
      }
      return ((object) buffer).ToString();
    }

    [SecurityCritical]
    public static object EvtGetObjectArrayProperty(EventLogHandle objArrayHandle, int index, int thePropertyId)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        int propertyValueBufferUsed;
        bool objectArrayProperty1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetObjectArrayProperty(objArrayHandle, thePropertyId, index, 0, 0, IntPtr.Zero, out propertyValueBufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!objectArrayProperty1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(propertyValueBufferUsed);
        bool objectArrayProperty2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetObjectArrayProperty(objArrayHandle, thePropertyId, index, 0, propertyValueBufferUsed, num, out propertyValueBufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!objectArrayProperty2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecurityCritical]
    public static object EvtGetEventMetadataProperty(EventLogHandle handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId enumType)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        int eventMetadataPropertyBufferUsed;
        bool metadataProperty1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetEventMetadataProperty(handle, enumType, 0, 0, IntPtr.Zero, out eventMetadataPropertyBufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!metadataProperty1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(eventMetadataPropertyBufferUsed);
        bool metadataProperty2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetEventMetadataProperty(handle, enumType, 0, eventMetadataPropertyBufferUsed, num, out eventMetadataPropertyBufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!metadataProperty2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecuritySafeCritical]
    public static object EvtGetChannelConfigProperty(EventLogHandle handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId enumType)
    {
      IntPtr num = IntPtr.Zero;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      try
      {
        int propertyValueBufferUsed;
        bool channelConfigProperty1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetChannelConfigProperty(handle, enumType, 0, 0, IntPtr.Zero, out propertyValueBufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!channelConfigProperty1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(propertyValueBufferUsed);
        bool channelConfigProperty2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetChannelConfigProperty(handle, enumType, 0, propertyValueBufferUsed, num, out propertyValueBufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!channelConfigProperty2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecuritySafeCritical]
    public static void EvtSetChannelConfigProperty(EventLogHandle handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId enumType, object val)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      Microsoft.Win32.UnsafeNativeMethods.EvtVariant propertyValue = new Microsoft.Win32.UnsafeNativeMethods.EvtVariant();
      CoTaskMemSafeHandle taskMemSafeHandle = new CoTaskMemSafeHandle();
      using (taskMemSafeHandle)
      {
        if (val != null)
        {
          switch (enumType)
          {
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigEnabled:
              propertyValue.Type = 13U;
              propertyValue.Bool = !(bool) val ? 0U : 1U;
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigAccess:
              propertyValue.Type = 1U;
              taskMemSafeHandle.SetMemory(Marshal.StringToCoTaskMemAuto((string) val));
              propertyValue.StringVal = taskMemSafeHandle.GetMemory();
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigRetention:
              propertyValue.Type = 13U;
              propertyValue.Bool = !(bool) val ? 0U : 1U;
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigAutoBackup:
              propertyValue.Type = 13U;
              propertyValue.Bool = !(bool) val ? 0U : 1U;
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigMaxSize:
              propertyValue.Type = 10U;
              propertyValue.ULong = (ulong) (long) val;
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigLogFilePath:
              propertyValue.Type = 1U;
              taskMemSafeHandle.SetMemory(Marshal.StringToCoTaskMemAuto((string) val));
              propertyValue.StringVal = taskMemSafeHandle.GetMemory();
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigLevel:
              propertyValue.Type = 8U;
              propertyValue.UInteger = (uint) (int) val;
              break;
            case Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigKeywords:
              propertyValue.Type = 10U;
              propertyValue.ULong = (ulong) (long) val;
              break;
            default:
              throw new InvalidOperationException();
          }
        }
        else
          propertyValue.Type = 0U;
        bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtSetChannelConfigProperty(handle, enumType, 0, ref propertyValue);
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (flag)
          return;
        EventLogException.Throw(lastWin32Error);
      }
    }

    [SecurityCritical]
    public static string EvtNextChannelPath(EventLogHandle handle, ref bool finish)
    {
      StringBuilder channelPathBuffer = new StringBuilder((string) null);
      int channelPathBufferUsed;
      bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtNextChannelPath(handle, 0, channelPathBuffer, out channelPathBufferUsed);
      int lastWin32Error1 = Marshal.GetLastWin32Error();
      if (!flag1)
      {
        if (lastWin32Error1 == 259)
        {
          finish = true;
          return (string) null;
        }
        else if (lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
      }
      channelPathBuffer.EnsureCapacity(channelPathBufferUsed);
      bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtNextChannelPath(handle, channelPathBufferUsed, channelPathBuffer, out channelPathBufferUsed);
      int lastWin32Error2 = Marshal.GetLastWin32Error();
      if (!flag2)
        EventLogException.Throw(lastWin32Error2);
      return ((object) channelPathBuffer).ToString();
    }

    [SecurityCritical]
    public static string EvtNextPublisherId(EventLogHandle handle, ref bool finish)
    {
      StringBuilder publisherIdBuffer = new StringBuilder((string) null);
      int publisherIdBufferUsed;
      bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtNextPublisherId(handle, 0, publisherIdBuffer, out publisherIdBufferUsed);
      int lastWin32Error1 = Marshal.GetLastWin32Error();
      if (!flag1)
      {
        if (lastWin32Error1 == 259)
        {
          finish = true;
          return (string) null;
        }
        else if (lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
      }
      publisherIdBuffer.EnsureCapacity(publisherIdBufferUsed);
      bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtNextPublisherId(handle, publisherIdBufferUsed, publisherIdBuffer, out publisherIdBufferUsed);
      int lastWin32Error2 = Marshal.GetLastWin32Error();
      if (!flag2)
        EventLogException.Throw(lastWin32Error2);
      return ((object) publisherIdBuffer).ToString();
    }

    [SecurityCritical]
    public static object EvtGetLogInfo(EventLogHandle handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId enumType)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        int propertyValueBufferUsed;
        bool logInfo1 = Microsoft.Win32.UnsafeNativeMethods.EvtGetLogInfo(handle, enumType, 0, IntPtr.Zero, out propertyValueBufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!logInfo1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(propertyValueBufferUsed);
        bool logInfo2 = Microsoft.Win32.UnsafeNativeMethods.EvtGetLogInfo(handle, enumType, propertyValueBufferUsed, num, out propertyValueBufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!logInfo2)
          EventLogException.Throw(lastWin32Error2);
        return NativeWrapper.ConvertToObject((Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(num, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant)));
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecuritySafeCritical]
    public static void EvtRenderBufferWithContextSystem(EventLogHandle contextHandle, EventLogHandle eventHandle, Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags flag, NativeWrapper.SystemProperties systemProperties, int SYSTEM_PROPERTY_COUNT)
    {
      IntPtr num1 = IntPtr.Zero;
      IntPtr num2 = IntPtr.Zero;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      try
      {
        int buffUsed;
        int propCount;
        if (!Microsoft.Win32.UnsafeNativeMethods.EvtRender(contextHandle, eventHandle, flag, 0, IntPtr.Zero, out buffUsed, out propCount))
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          if (lastWin32Error != 122)
            EventLogException.Throw(lastWin32Error);
        }
        num1 = Marshal.AllocHGlobal(buffUsed);
        bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtRender(contextHandle, eventHandle, flag, buffUsed, num1, out buffUsed, out propCount);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!flag1)
          EventLogException.Throw(lastWin32Error1);
        if (propCount != SYSTEM_PROPERTY_COUNT)
          throw new InvalidOperationException("We do not have " + (object) SYSTEM_PROPERTY_COUNT + " variants given for the  UnsafeNativeMethods.EvtRenderFlags.EvtRenderEventValues flag. (System Properties)");
        IntPtr ptr = num1;
        for (int index = 0; index < propCount; ++index)
        {
          Microsoft.Win32.UnsafeNativeMethods.EvtVariant evtVariant = (Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(ptr, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant));
          switch (index)
          {
            case 0:
              systemProperties.ProviderName = (string) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeString);
              break;
            case 1:
              systemProperties.ProviderId = (Guid?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeGuid);
              break;
            case 2:
              systemProperties.Id = (ushort?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeUInt16);
              break;
            case 3:
              systemProperties.Qualifiers = (ushort?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeUInt16);
              break;
            case 4:
              systemProperties.Level = (byte?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeByte);
              break;
            case 5:
              systemProperties.Task = (ushort?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeUInt16);
              break;
            case 6:
              systemProperties.Opcode = (byte?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeByte);
              break;
            case 7:
              systemProperties.Keywords = (ulong?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeHexInt64);
              break;
            case 8:
              systemProperties.TimeCreated = (DateTime?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeFileTime);
              break;
            case 9:
              systemProperties.RecordId = (ulong?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeUInt64);
              break;
            case 10:
              systemProperties.ActivityId = (Guid?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeGuid);
              break;
            case 11:
              systemProperties.RelatedActivityId = (Guid?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeGuid);
              break;
            case 12:
              systemProperties.ProcessId = (uint?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeUInt32);
              break;
            case 13:
              systemProperties.ThreadId = (uint?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeUInt32);
              break;
            case 14:
              systemProperties.ChannelName = (string) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeString);
              break;
            case 15:
              systemProperties.ComputerName = (string) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeString);
              break;
            case 16:
              systemProperties.UserId = (SecurityIdentifier) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeSid);
              break;
            case 17:
              systemProperties.Version = (byte?) NativeWrapper.ConvertToObject(evtVariant, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType.EvtVarTypeByte);
              break;
          }
          ptr = new IntPtr((long) ptr + (long) Marshal.SizeOf<Microsoft.Win32.UnsafeNativeMethods.EvtVariant>(evtVariant));
        }
      }
      finally
      {
        if (num1 != IntPtr.Zero)
          Marshal.FreeHGlobal(num1);
      }
    }

    [SecuritySafeCritical]
    public static IList<object> EvtRenderBufferWithContextUserOrValues(EventLogHandle contextHandle, EventLogHandle eventHandle)
    {
      IntPtr num1 = IntPtr.Zero;
      IntPtr num2 = IntPtr.Zero;
      Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags flags = Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags.EvtRenderEventValues;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      try
      {
        int buffUsed;
        int propCount;
        if (!Microsoft.Win32.UnsafeNativeMethods.EvtRender(contextHandle, eventHandle, flags, 0, IntPtr.Zero, out buffUsed, out propCount))
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          if (lastWin32Error != 122)
            EventLogException.Throw(lastWin32Error);
        }
        num1 = Marshal.AllocHGlobal(buffUsed);
        bool flag = Microsoft.Win32.UnsafeNativeMethods.EvtRender(contextHandle, eventHandle, flags, buffUsed, num1, out buffUsed, out propCount);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!flag)
          EventLogException.Throw(lastWin32Error1);
        List<object> list = new List<object>(propCount);
        if (propCount > 0)
        {
          IntPtr ptr = num1;
          for (int index = 0; index < propCount; ++index)
          {
            Microsoft.Win32.UnsafeNativeMethods.EvtVariant evtVariant = (Microsoft.Win32.UnsafeNativeMethods.EvtVariant) Marshal.PtrToStructure(ptr, typeof (Microsoft.Win32.UnsafeNativeMethods.EvtVariant));
            list.Add(NativeWrapper.ConvertToObject(evtVariant));
            ptr = new IntPtr((long) ptr + (long) Marshal.SizeOf<Microsoft.Win32.UnsafeNativeMethods.EvtVariant>(evtVariant));
          }
        }
        return (IList<object>) list;
      }
      finally
      {
        if (num1 != IntPtr.Zero)
          Marshal.FreeHGlobal(num1);
      }
    }

    [SecuritySafeCritical]
    public static string EvtFormatMessageRenderName(EventLogHandle pmHandle, EventLogHandle eventHandle, Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageFlags flag)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      StringBuilder buffer = new StringBuilder((string) null);
      int bufferUsed;
      bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessage(pmHandle, eventHandle, 0U, 0, (Microsoft.Win32.UnsafeNativeMethods.EvtStringVariant[]) null, flag, 0, buffer, out bufferUsed);
      int lastWin32Error1 = Marshal.GetLastWin32Error();
      if (!flag1 && lastWin32Error1 != 15029)
      {
        switch (lastWin32Error1)
        {
          case 15033:
          case 15100:
          case 1815:
          case 15027:
          case 15028:
            return (string) null;
          case 122:
            break;
          default:
            EventLogException.Throw(lastWin32Error1);
            break;
        }
      }
      buffer.EnsureCapacity(bufferUsed);
      bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessage(pmHandle, eventHandle, 0U, 0, (Microsoft.Win32.UnsafeNativeMethods.EvtStringVariant[]) null, flag, bufferUsed, buffer, out bufferUsed);
      int lastWin32Error2 = Marshal.GetLastWin32Error();
      if (!flag2 && lastWin32Error2 != 15029)
      {
        switch (lastWin32Error2)
        {
          case 15033:
          case 15100:
          case 1815:
          case 15027:
          case 15028:
            return (string) null;
          default:
            EventLogException.Throw(lastWin32Error2);
            break;
        }
      }
      return ((object) buffer).ToString();
    }

    [SecuritySafeCritical]
    public static IEnumerable<string> EvtFormatMessageRenderKeywords(EventLogHandle pmHandle, EventLogHandle eventHandle, Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageFlags flag)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      IntPtr num = IntPtr.Zero;
      try
      {
        List<string> list = new List<string>();
        int bufferUsed;
        bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageBuffer(pmHandle, eventHandle, 0U, 0, IntPtr.Zero, flag, 0, IntPtr.Zero, out bufferUsed);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!flag1)
        {
          switch (lastWin32Error1)
          {
            case 15033:
            case 15100:
            case 1815:
            case 15027:
            case 15028:
              return (IEnumerable<string>) list.AsReadOnly();
            case 122:
              break;
            default:
              EventLogException.Throw(lastWin32Error1);
              break;
          }
        }
        num = Marshal.AllocHGlobal(bufferUsed * 2);
        bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageBuffer(pmHandle, eventHandle, 0U, 0, IntPtr.Zero, flag, bufferUsed, num, out bufferUsed);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!flag2)
        {
          switch (lastWin32Error2)
          {
            case 15033:
            case 15100:
            case 1815:
            case 15027:
            case 15028:
              return (IEnumerable<string>) list;
            default:
              EventLogException.Throw(lastWin32Error2);
              break;
          }
        }
        IntPtr ptr = num;
        while (true)
        {
          string str = Marshal.PtrToStringAuto(ptr);
          if (!string.IsNullOrEmpty(str))
          {
            list.Add(str);
            ptr = new IntPtr((long) ptr + (long) (str.Length * 2) + 2L);
          }
          else
            break;
        }
        return (IEnumerable<string>) list.AsReadOnly();
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecurityCritical]
    public static string EvtRenderBookmark(EventLogHandle eventHandle)
    {
      IntPtr num = IntPtr.Zero;
      Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags flags = Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags.EvtRenderBookmark;
      try
      {
        int buffUsed;
        int propCount;
        bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtRender(EventLogHandle.Zero, eventHandle, flags, 0, IntPtr.Zero, out buffUsed, out propCount);
        int lastWin32Error1 = Marshal.GetLastWin32Error();
        if (!flag1 && lastWin32Error1 != 122)
          EventLogException.Throw(lastWin32Error1);
        num = Marshal.AllocHGlobal(buffUsed);
        bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtRender(EventLogHandle.Zero, eventHandle, flags, buffUsed, num, out buffUsed, out propCount);
        int lastWin32Error2 = Marshal.GetLastWin32Error();
        if (!flag2)
          EventLogException.Throw(lastWin32Error2);
        return Marshal.PtrToStringAuto(num);
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    [SecuritySafeCritical]
    public static string EvtFormatMessageFormatDescription(EventLogHandle handle, EventLogHandle eventHandle, string[] values)
    {
      if (NativeWrapper.s_platformNotSupported)
        throw new PlatformNotSupportedException();
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      Microsoft.Win32.UnsafeNativeMethods.EvtStringVariant[] values1 = new Microsoft.Win32.UnsafeNativeMethods.EvtStringVariant[values.Length];
      for (int index = 0; index < values.Length; ++index)
      {
        values1[index].Type = 1U;
        values1[index].StringVal = values[index];
      }
      StringBuilder buffer = new StringBuilder((string) null);
      int bufferUsed;
      bool flag1 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessage(handle, eventHandle, uint.MaxValue, values.Length, values1, Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageEvent, 0, buffer, out bufferUsed);
      int lastWin32Error1 = Marshal.GetLastWin32Error();
      if (!flag1 && lastWin32Error1 != 15029)
      {
        switch (lastWin32Error1)
        {
          case 15033:
          case 15100:
          case 1815:
          case 15027:
          case 15028:
            return (string) null;
          case 122:
            break;
          default:
            EventLogException.Throw(lastWin32Error1);
            break;
        }
      }
      buffer.EnsureCapacity(bufferUsed);
      bool flag2 = Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessage(handle, eventHandle, uint.MaxValue, values.Length, values1, Microsoft.Win32.UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageEvent, bufferUsed, buffer, out bufferUsed);
      int lastWin32Error2 = Marshal.GetLastWin32Error();
      if (!flag2 && lastWin32Error2 != 15029)
      {
        switch (lastWin32Error2)
        {
          case 15033:
          case 15100:
          case 1815:
          case 15027:
          case 15028:
            return (string) null;
          default:
            EventLogException.Throw(lastWin32Error2);
            break;
        }
      }
      return ((object) buffer).ToString();
    }

    [SecurityCritical]
    private static object ConvertToObject(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      switch (val.Type)
      {
        case 0U:
          return (object) null;
        case 1U:
          return (object) NativeWrapper.ConvertToString(val);
        case 2U:
          return (object) NativeWrapper.ConvertToAnsiString(val);
        case 3U:
          return (object) val.SByte;
        case 4U:
          return (object) val.UInt8;
        case 5U:
          return (object) val.SByte;
        case 6U:
          return (object) val.UShort;
        case 7U:
          return (object) val.Integer;
        case 8U:
          return (object) val.UInteger;
        case 9U:
          return (object) val.Long;
        case 10U:
          return (object) val.ULong;
        case 11U:
          return (object) val.Single;
        case 12U:
          return (object) val.Double;
        case 13U:
          if ((int) val.Bool != 0)
            return (object) true;
          else
            return (object) false;
        case 14U:
        case 132U:
          if (val.Reference == IntPtr.Zero)
            return (object) new byte[0];
          byte[] destination1 = new byte[(IntPtr) val.Count];
          Marshal.Copy(val.Reference, destination1, 0, (int) val.Count);
          return (object) destination1;
        case 15U:
          if (!(val.GuidReference == IntPtr.Zero))
            return Marshal.PtrToStructure(val.GuidReference, typeof (Guid));
          else
            return (object) Guid.Empty;
        case 16U:
          return (object) val.SizeT;
        case 17U:
          return (object) DateTime.FromFileTime((long) val.FileTime);
        case 18U:
          Microsoft.Win32.UnsafeNativeMethods.SystemTime systemTime = (Microsoft.Win32.UnsafeNativeMethods.SystemTime) Marshal.PtrToStructure(val.SystemTime, typeof (Microsoft.Win32.UnsafeNativeMethods.SystemTime));
          return (object) new DateTime((int) systemTime.Year, (int) systemTime.Month, (int) systemTime.Day, (int) systemTime.Hour, (int) systemTime.Minute, (int) systemTime.Second, (int) systemTime.Milliseconds);
        case 19U:
          if (!(val.SidVal == IntPtr.Zero))
            return (object) new SecurityIdentifier(val.SidVal);
          else
            return (object) null;
        case 20U:
          return (object) val.Integer;
        case 21U:
          return (object) val.ULong;
        case 32U:
          return (object) NativeWrapper.ConvertToSafeHandle(val);
        case 129U:
          return (object) NativeWrapper.ConvertToStringArray(val, false);
        case 130U:
          return (object) NativeWrapper.ConvertToStringArray(val, true);
        case 131U:
          return (object) NativeWrapper.ConvertToArray(val, typeof (sbyte), 1);
        case 133U:
          if (val.Reference == IntPtr.Zero)
            return (object) new short[0];
          short[] destination2 = new short[(IntPtr) val.Count];
          Marshal.Copy(val.Reference, destination2, 0, (int) val.Count);
          return (object) destination2;
        case 134U:
          return (object) NativeWrapper.ConvertToArray(val, typeof (ushort), 2);
        case 135U:
          if (val.Reference == IntPtr.Zero)
            return (object) new int[0];
          int[] destination3 = new int[(IntPtr) val.Count];
          Marshal.Copy(val.Reference, destination3, 0, (int) val.Count);
          return (object) destination3;
        case 136U:
        case 148U:
          return (object) NativeWrapper.ConvertToArray(val, typeof (uint), 4);
        case 137U:
          if (val.Reference == IntPtr.Zero)
            return (object) new long[0];
          long[] destination4 = new long[(IntPtr) val.Count];
          Marshal.Copy(val.Reference, destination4, 0, (int) val.Count);
          return (object) destination4;
        case 138U:
        case 149U:
          return (object) NativeWrapper.ConvertToArray(val, typeof (ulong), 8);
        case 139U:
          if (val.Reference == IntPtr.Zero)
            return (object) new float[0];
          float[] destination5 = new float[(IntPtr) val.Count];
          Marshal.Copy(val.Reference, destination5, 0, (int) val.Count);
          return (object) destination5;
        case 140U:
          if (val.Reference == IntPtr.Zero)
            return (object) new double[0];
          double[] destination6 = new double[(IntPtr) val.Count];
          Marshal.Copy(val.Reference, destination6, 0, (int) val.Count);
          return (object) destination6;
        case 141U:
          return (object) NativeWrapper.ConvertToBoolArray(val);
        case 143U:
          return (object) NativeWrapper.ConvertToArray(val, typeof (Guid), 16);
        case 145U:
          return (object) NativeWrapper.ConvertToFileTimeArray(val);
        case 146U:
          return (object) NativeWrapper.ConvertToSysTimeArray(val);
        default:
          throw new EventLogInvalidDataException();
      }
    }

    [SecurityCritical]
    public static object ConvertToObject(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val, Microsoft.Win32.UnsafeNativeMethods.EvtVariantType desiredType)
    {
      if ((int) val.Type == 0)
        return (object) null;
      if ((long) val.Type != (long) desiredType)
        throw new EventLogInvalidDataException();
      else
        return NativeWrapper.ConvertToObject(val);
    }

    [SecurityCritical]
    public static string ConvertToString(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      if (val.StringVal == IntPtr.Zero)
        return string.Empty;
      else
        return Marshal.PtrToStringAuto(val.StringVal);
    }

    [SecurityCritical]
    public static string ConvertToAnsiString(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      if (val.AnsiString == IntPtr.Zero)
        return string.Empty;
      else
        return Marshal.PtrToStringAnsi(val.AnsiString);
    }

    [SecurityCritical]
    public static EventLogHandle ConvertToSafeHandle(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      if (val.Handle == IntPtr.Zero)
        return EventLogHandle.Zero;
      else
        return new EventLogHandle(val.Handle, true);
    }

    [SecurityCritical]
    public static Array ConvertToArray(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val, Type objType, int size)
    {
      IntPtr ptr = val.Reference;
      if (ptr == IntPtr.Zero)
        return Array.CreateInstance(objType, 0);
      Array instance = Array.CreateInstance(objType, new long[1]
      {
        (long) val.Count
      });
      for (int index = 0; (long) index < (long) val.Count; ++index)
      {
        instance.SetValue(Marshal.PtrToStructure(ptr, objType), index);
        ptr = new IntPtr((long) ptr + (long) size);
      }
      return instance;
    }

    [SecurityCritical]
    public static Array ConvertToBoolArray(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      IntPtr ptr = val.Reference;
      if (ptr == IntPtr.Zero)
        return (Array) new bool[0];
      bool[] flagArray = new bool[(IntPtr) val.Count];
      for (int index = 0; (long) index < (long) val.Count; ++index)
      {
        bool flag = Marshal.ReadInt32(ptr) != 0;
        flagArray[index] = flag;
        ptr = new IntPtr((long) ptr + 4L);
      }
      return (Array) flagArray;
    }

    [SecurityCritical]
    public static Array ConvertToFileTimeArray(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      IntPtr ptr = val.Reference;
      if (ptr == IntPtr.Zero)
        return (Array) new DateTime[0];
      DateTime[] dateTimeArray = new DateTime[(IntPtr) val.Count];
      for (int index = 0; (long) index < (long) val.Count; ++index)
      {
        dateTimeArray[index] = DateTime.FromFileTime(Marshal.ReadInt64(ptr));
        ptr = new IntPtr((long) ptr + 8L);
      }
      return (Array) dateTimeArray;
    }

    [SecurityCritical]
    public static Array ConvertToSysTimeArray(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val)
    {
      IntPtr ptr = val.Reference;
      if (ptr == IntPtr.Zero)
        return (Array) new DateTime[0];
      DateTime[] dateTimeArray = new DateTime[(IntPtr) val.Count];
      for (int index = 0; (long) index < (long) val.Count; ++index)
      {
        Microsoft.Win32.UnsafeNativeMethods.SystemTime systemTime = (Microsoft.Win32.UnsafeNativeMethods.SystemTime) Marshal.PtrToStructure(ptr, typeof (Microsoft.Win32.UnsafeNativeMethods.SystemTime));
        dateTimeArray[index] = new DateTime((int) systemTime.Year, (int) systemTime.Month, (int) systemTime.Day, (int) systemTime.Hour, (int) systemTime.Minute, (int) systemTime.Second, (int) systemTime.Milliseconds);
        ptr = new IntPtr((long) ptr + 16L);
      }
      return (Array) dateTimeArray;
    }

    [SecurityCritical]
    public static string[] ConvertToStringArray(Microsoft.Win32.UnsafeNativeMethods.EvtVariant val, bool ansi)
    {
      if (val.Reference == IntPtr.Zero)
        return new string[0];
      IntPtr source = val.Reference;
      IntPtr[] destination = new IntPtr[(IntPtr) val.Count];
      Marshal.Copy(source, destination, 0, (int) val.Count);
      string[] strArray = new string[(IntPtr) val.Count];
      for (int index = 0; (long) index < (long) val.Count; ++index)
        strArray[index] = ansi ? Marshal.PtrToStringAnsi(destination[index]) : Marshal.PtrToStringAuto(destination[index]);
      return strArray;
    }

    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public class SystemProperties
    {
      public ushort? Id = new ushort?();
      public byte? Version = new byte?();
      public ushort? Qualifiers = new ushort?();
      public byte? Level = new byte?();
      public ushort? Task = new ushort?();
      public byte? Opcode = new byte?();
      public ulong? Keywords = new ulong?();
      public ulong? RecordId = new ulong?();
      public Guid? ProviderId = new Guid?();
      public uint? ProcessId = new uint?();
      public uint? ThreadId = new uint?();
      public DateTime? TimeCreated = new DateTime?();
      public Guid? ActivityId = new Guid?();
      public Guid? RelatedActivityId = new Guid?();
      public bool filled;
      public string ProviderName;
      public string ChannelName;
      public string ComputerName;
      public SecurityIdentifier UserId;
    }
  }
}
