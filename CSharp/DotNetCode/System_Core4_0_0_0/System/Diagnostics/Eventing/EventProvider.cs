// Type: System.Diagnostics.Eventing.EventProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics.Eventing
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventProvider : IDisposable
  {
    private static bool s_platformNotSupported = Environment.OSVersion.Version.Major < 6;
    private static bool s_preWin7 = Environment.OSVersion.Version.Major < 6 || Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor < 1;
    [SecurityCritical]
    private Microsoft.Win32.UnsafeNativeMethods.EtwEnableCallback m_etwCallback;
    private long m_regHandle;
    private byte m_level;
    private long m_anyKeywordMask;
    private long m_allKeywordMask;
    private int m_enabled;
    private Guid m_providerId;
    private int m_disposed;
    [ThreadStatic]
    private static EventProvider.WriteEventErrorCode t_returnCode;
    private const int s_basicTypeAllocationBufferSize = 16;
    private const int s_etwMaxMumberArguments = 32;
    private const int s_etwAPIMaxStringCount = 8;
    private const int s_maxEventDataDescriptors = 128;
    private const int s_traceEventMaximumSize = 65482;
    private const int s_traceEventMaximumStringSize = 32724;

    static EventProvider()
    {
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
    public EventProvider(Guid providerGuid)
    {
      this.m_providerId = providerGuid;
      this.EtwRegister();
    }

    ~EventProvider()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SecuritySafeCritical]
    protected virtual void Dispose(bool disposing)
    {
      if (this.m_disposed == 1 || Interlocked.Exchange(ref this.m_disposed, 1) != 0)
        return;
      this.m_enabled = 0;
      this.Deregister();
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public virtual void Close()
    {
      this.Dispose();
    }

    public bool IsEnabled()
    {
      return this.m_enabled != 0;
    }

    public bool IsEnabled(byte level, long keywords)
    {
      return this.m_enabled != 0 && ((int) level <= (int) this.m_level || (int) this.m_level == 0) && (keywords == 0L || (keywords & this.m_anyKeywordMask) != 0L && (keywords & this.m_allKeywordMask) == this.m_allKeywordMask);
    }

    public static EventProvider.WriteEventErrorCode GetLastWriteEventError()
    {
      return EventProvider.t_returnCode;
    }

    [SecurityCritical]
    public unsafe bool WriteMessageEvent(string eventMessage, byte eventLevel, long eventKeywords)
    {
      if (eventMessage == null)
        throw new ArgumentNullException("eventMessage");
      if (this.IsEnabled(eventLevel, eventKeywords))
      {
        if (eventMessage.Length > 32724)
        {
          EventProvider.t_returnCode = EventProvider.WriteEventErrorCode.EventTooBig;
          return false;
        }
        else
        {
          int error;
          fixed (char* message = eventMessage)
            error = (int) Microsoft.Win32.UnsafeNativeMethods.EventWriteString(this.m_regHandle, eventLevel, eventKeywords, message);
          if (error != 0)
          {
            EventProvider.SetLastError(error);
            return false;
          }
        }
      }
      return true;
    }

    public bool WriteMessageEvent(string eventMessage)
    {
      return this.WriteMessageEvent(eventMessage, (byte) 0, 0L);
    }

    public bool WriteEvent(ref EventDescriptor eventDescriptor, params object[] eventPayload)
    {
      return this.WriteTransferEvent(ref eventDescriptor, Guid.Empty, eventPayload);
    }

    [SecurityCritical]
    public unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, string data)
    {
      uint num = 0U;
      if (data == null)
        throw new ArgumentNullException("dataString");
      if (this.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
      {
        if (data.Length > 32724)
        {
          EventProvider.t_returnCode = EventProvider.WriteEventErrorCode.EventTooBig;
          return false;
        }
        else
        {
          EventProvider.EventData eventData;
          eventData.Size = (uint) ((data.Length + 1) * 2);
          eventData.Reserved = 0;
          fixed (char* chPtr = data)
          {
            Guid activityId = EventProvider.GetActivityId();
            eventData.DataPointer = (ulong) chPtr;
            num = !EventProvider.s_preWin7 ? Microsoft.Win32.UnsafeNativeMethods.EventWriteTransfer(this.m_regHandle, ref eventDescriptor, activityId == Guid.Empty ? (Guid*) null : &activityId, (Guid*) null, 1U, (void*) &eventData) : Microsoft.Win32.UnsafeNativeMethods.EventWrite(this.m_regHandle, ref eventDescriptor, 1U, (void*) &eventData);
          }
        }
      }
      if ((int) num == 0)
        return true;
      EventProvider.SetLastError((int) num);
      return false;
    }

    [SecurityCritical]
    protected unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, int dataCount, IntPtr data)
    {
      uint num;
      if (EventProvider.s_preWin7)
      {
        num = Microsoft.Win32.UnsafeNativeMethods.EventWrite(this.m_regHandle, ref eventDescriptor, (uint) dataCount, (void*) data);
      }
      else
      {
        Guid activityId = EventProvider.GetActivityId();
        num = Microsoft.Win32.UnsafeNativeMethods.EventWriteTransfer(this.m_regHandle, ref eventDescriptor, activityId == Guid.Empty ? (Guid*) null : &activityId, (Guid*) null, (uint) dataCount, (void*) data);
      }
      if ((int) num == 0)
        return true;
      EventProvider.SetLastError((int) num);
      return false;
    }

    [SecurityCritical]
    public unsafe bool WriteTransferEvent(ref EventDescriptor eventDescriptor, Guid relatedActivityId, params object[] eventPayload)
    {
      uint num1 = 0U;
      if (this.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
      {
        Guid activityId = EventProvider.GetActivityId();
        int num2 = 0;
        EventProvider.EventData* eventDataPtr1 = (EventProvider.EventData*) null;
        if (eventPayload != null && eventPayload.Length != 0)
        {
          num2 = eventPayload.Length;
          if (num2 > 32)
          {
            throw new ArgumentOutOfRangeException("eventPayload", SR.GetString("ArgumentOutOfRange_MaxArgExceeded", new object[1]
            {
              (object) 32
            }));
          }
          else
          {
            uint num3 = 0U;
            int index1 = 0;
            int[] numArray = new int[8];
            string[] strArray = new string[8];
            EventProvider.EventData* eventDataPtr2 = stackalloc EventProvider.EventData[num2];
            EventProvider.EventData* dataDescriptor = eventDataPtr2;
            byte* numPtr = stackalloc byte[16 * num2];
            byte* dataBuffer = numPtr;
            for (int index2 = 0; index2 < eventPayload.Length; ++index2)
            {
              string str = EventProvider.EncodeObject(ref eventPayload[index2], dataDescriptor, dataBuffer);
              dataBuffer += 16;
              num3 += dataDescriptor->Size;
              ++dataDescriptor;
              if (str != null)
              {
                if (index1 < 8)
                {
                  strArray[index1] = str;
                  numArray[index1] = index2;
                  ++index1;
                }
                else
                  throw new ArgumentOutOfRangeException("eventPayload", SR.GetString("ArgumentOutOfRange_MaxStringsExceeded", new object[1]
                  {
                    (object) 8
                  }));
              }
            }
            if (num3 > 65482U)
            {
              EventProvider.t_returnCode = EventProvider.WriteEventErrorCode.EventTooBig;
              return false;
            }
            else
            {
              fixed (char* chPtr1 = strArray[0])
                fixed (char* chPtr2 = strArray[1])
                  fixed (char* chPtr3 = strArray[2])
                    fixed (char* chPtr4 = strArray[3])
                      fixed (char* chPtr5 = strArray[4])
                        fixed (char* chPtr6 = strArray[5])
                          fixed (char* chPtr7 = strArray[6])
                            fixed (char* chPtr8 = strArray[7])
                            {
                              eventDataPtr1 = eventDataPtr2;
                              if (strArray[0] != null)
                                eventDataPtr1[numArray[0]].DataPointer = (ulong) chPtr1;
                              if (strArray[1] != null)
                                eventDataPtr1[numArray[1]].DataPointer = (ulong) chPtr2;
                              if (strArray[2] != null)
                                eventDataPtr1[numArray[2]].DataPointer = (ulong) chPtr3;
                              if (strArray[3] != null)
                                eventDataPtr1[numArray[3]].DataPointer = (ulong) chPtr4;
                              if (strArray[4] != null)
                                eventDataPtr1[numArray[4]].DataPointer = (ulong) chPtr5;
                              if (strArray[5] != null)
                                eventDataPtr1[numArray[5]].DataPointer = (ulong) chPtr6;
                              if (strArray[6] != null)
                                eventDataPtr1[numArray[6]].DataPointer = (ulong) chPtr7;
                              if (strArray[7] != null)
                                eventDataPtr1[numArray[7]].DataPointer = (ulong) chPtr8;
                            }
            }
          }
        }
        num1 = !(relatedActivityId == Guid.Empty) || !EventProvider.s_preWin7 ? Microsoft.Win32.UnsafeNativeMethods.EventWriteTransfer(this.m_regHandle, ref eventDescriptor, activityId == Guid.Empty ? (Guid*) null : &activityId, !(relatedActivityId == Guid.Empty) || EventProvider.s_preWin7 ? &relatedActivityId : (Guid*) null, (uint) num2, (void*) eventDataPtr1) : Microsoft.Win32.UnsafeNativeMethods.EventWrite(this.m_regHandle, ref eventDescriptor, (uint) num2, (void*) eventDataPtr1);
      }
      if ((int) num1 == 0)
        return true;
      EventProvider.SetLastError((int) num1);
      return false;
    }

    [SecurityCritical]
    protected unsafe bool WriteTransferEvent(ref EventDescriptor eventDescriptor, Guid relatedActivityId, int dataCount, IntPtr data)
    {
      Guid activityId = EventProvider.GetActivityId();
      uint num = Microsoft.Win32.UnsafeNativeMethods.EventWriteTransfer(this.m_regHandle, ref eventDescriptor, activityId == Guid.Empty ? (Guid*) null : &activityId, &relatedActivityId, (uint) dataCount, (void*) data);
      if ((int) num == 0)
        return true;
      EventProvider.SetLastError((int) num);
      return false;
    }

    [SecurityCritical]
    public static void SetActivityId(ref Guid id)
    {
      Trace.CorrelationManager.ActivityId = id;
      int num = (int) Microsoft.Win32.UnsafeNativeMethods.EventActivityIdControl(2, out id);
    }

    [SecurityCritical]
    public static Guid CreateActivityId()
    {
      Guid ActivityId = new Guid();
      int num = (int) Microsoft.Win32.UnsafeNativeMethods.EventActivityIdControl(3, out ActivityId);
      return ActivityId;
    }

    [SecurityCritical]
    private unsafe void EtwRegister()
    {
      if (EventProvider.s_platformNotSupported)
        throw new PlatformNotSupportedException(SR.GetString("NotSupported_DownLevelVista"));
      this.m_etwCallback = new Microsoft.Win32.UnsafeNativeMethods.EtwEnableCallback(this.EtwEnableCallBack);
      uint num = Microsoft.Win32.UnsafeNativeMethods.EventRegister(ref this.m_providerId, this.m_etwCallback, (void*) null, out this.m_regHandle);
      if ((int) num != 0)
        throw new Win32Exception((int) num);
    }

    [SecurityCritical]
    private void Deregister()
    {
      if (this.m_regHandle == 0L)
        return;
      Microsoft.Win32.UnsafeNativeMethods.EventUnregister(this.m_regHandle);
      this.m_regHandle = 0L;
    }

    [SecurityCritical]
    private unsafe void EtwEnableCallBack([In] ref Guid sourceId, [In] int isEnabled, [In] byte setLevel, [In] long anyKeyword, [In] long allKeyword, [In] void* filterData, [In] void* callbackContext)
    {
      this.m_enabled = isEnabled;
      this.m_level = setLevel;
      this.m_anyKeywordMask = anyKeyword;
      this.m_allKeywordMask = allKeyword;
    }

    private static void SetLastError(int error)
    {
      switch (error)
      {
        case 8:
          EventProvider.t_returnCode = EventProvider.WriteEventErrorCode.NoFreeBuffers;
          break;
        case 234:
        case 534:
          EventProvider.t_returnCode = EventProvider.WriteEventErrorCode.EventTooBig;
          break;
      }
    }

    [SecurityCritical]
    private static unsafe string EncodeObject(ref object data, EventProvider.EventData* dataDescriptor, byte* dataBuffer)
    {
      dataDescriptor->Reserved = 0;
      string str1 = data as string;
      if (str1 != null)
      {
        dataDescriptor->Size = (uint) ((str1.Length + 1) * 2);
        return str1;
      }
      else
      {
        if (data == null)
        {
          dataDescriptor->Size = 0U;
          dataDescriptor->DataPointer = 0UL;
        }
        else if (data is IntPtr)
        {
          dataDescriptor->Size = (uint) sizeof (IntPtr);
          IntPtr* numPtr = (IntPtr*) dataBuffer;
          *numPtr = (IntPtr) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is int)
        {
          dataDescriptor->Size = 4U;
          int* numPtr = (int*) dataBuffer;
          *numPtr = (int) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is long)
        {
          dataDescriptor->Size = 8U;
          long* numPtr = (long*) dataBuffer;
          *numPtr = (long) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is uint)
        {
          dataDescriptor->Size = 4U;
          uint* numPtr = (uint*) dataBuffer;
          *numPtr = (uint) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is ulong)
        {
          dataDescriptor->Size = 8U;
          ulong* numPtr = (ulong*) dataBuffer;
          *numPtr = (ulong) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is char)
        {
          dataDescriptor->Size = 2U;
          char* chPtr = (char*) dataBuffer;
          *chPtr = (char) data;
          dataDescriptor->DataPointer = (ulong) chPtr;
        }
        else if (data is byte)
        {
          dataDescriptor->Size = 1U;
          byte* numPtr = dataBuffer;
          *numPtr = (byte) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is short)
        {
          dataDescriptor->Size = 2U;
          short* numPtr = (short*) dataBuffer;
          *numPtr = (short) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is sbyte)
        {
          dataDescriptor->Size = 1U;
          sbyte* numPtr = (sbyte*) dataBuffer;
          *numPtr = (sbyte) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is ushort)
        {
          dataDescriptor->Size = 2U;
          ushort* numPtr = (ushort*) dataBuffer;
          *numPtr = (ushort) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is float)
        {
          dataDescriptor->Size = 4U;
          float* numPtr = (float*) dataBuffer;
          *numPtr = (float) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is double)
        {
          dataDescriptor->Size = 8U;
          double* numPtr = (double*) dataBuffer;
          *numPtr = (double) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is bool)
        {
          dataDescriptor->Size = 1U;
          bool* flagPtr = (bool*) dataBuffer;
          *flagPtr = (bool) data;
          dataDescriptor->DataPointer = (ulong) flagPtr;
        }
        else if (data is Guid)
        {
          dataDescriptor->Size = (uint) sizeof (Guid);
          Guid* guidPtr = (Guid*) dataBuffer;
          *guidPtr = (Guid) data;
          dataDescriptor->DataPointer = (ulong) guidPtr;
        }
        else if (data is Decimal)
        {
          dataDescriptor->Size = 16U;
          Decimal* numPtr = (Decimal*) dataBuffer;
          *numPtr = (Decimal) data;
          dataDescriptor->DataPointer = (ulong) numPtr;
        }
        else if (data is bool)
        {
          dataDescriptor->Size = 1U;
          bool* flagPtr = (bool*) dataBuffer;
          *flagPtr = (bool) data;
          dataDescriptor->DataPointer = (ulong) flagPtr;
        }
        else
        {
          string str2 = data.ToString();
          dataDescriptor->Size = (uint) ((str2.Length + 1) * 2);
          return str2;
        }
        return (string) null;
      }
    }

    [SecurityCritical]
    private static Guid GetActivityId()
    {
      return Trace.CorrelationManager.ActivityId;
    }

    public enum WriteEventErrorCode
    {
      NoError,
      NoFreeBuffers,
      EventTooBig,
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    private struct EventData
    {
      [FieldOffset(0)]
      internal ulong DataPointer;
      [FieldOffset(8)]
      internal uint Size;
      [FieldOffset(12)]
      internal int Reserved;
    }

    private enum ActivityControl : uint
    {
      EVENT_ACTIVITY_CTRL_GET_ID = 1U,
      EVENT_ACTIVITY_CTRL_SET_ID = 2U,
      EVENT_ACTIVITY_CTRL_CREATE_ID = 3U,
      EVENT_ACTIVITY_CTRL_GET_SET_ID = 4U,
      EVENT_ACTIVITY_CTRL_CREATE_SET_ID = 5U,
    }
  }
}
