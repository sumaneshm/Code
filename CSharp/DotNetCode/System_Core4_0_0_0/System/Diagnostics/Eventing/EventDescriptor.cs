// Type: System.Diagnostics.Eventing.EventDescriptor
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing
{
  [StructLayout(LayoutKind.Explicit, Size = 16)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public struct EventDescriptor
  {
    [FieldOffset(0)]
    private ushort m_id;
    [FieldOffset(2)]
    private byte m_version;
    [FieldOffset(3)]
    private byte m_channel;
    [FieldOffset(4)]
    private byte m_level;
    [FieldOffset(5)]
    private byte m_opcode;
    [FieldOffset(6)]
    private ushort m_task;
    [FieldOffset(8)]
    private long m_keywords;

    public int EventId
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return (int) this.m_id;
      }
    }

    public byte Version
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_version;
      }
    }

    public byte Channel
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_channel;
      }
    }

    public byte Level
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_level;
      }
    }

    public byte Opcode
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_opcode;
      }
    }

    public int Task
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return (int) this.m_task;
      }
    }

    public long Keywords
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_keywords;
      }
    }

    public EventDescriptor(int id, byte version, byte channel, byte level, byte opcode, int task, long keywords)
    {
      if (id < 0)
        throw new ArgumentOutOfRangeException("id", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (id > (int) ushort.MaxValue)
      {
        throw new ArgumentOutOfRangeException("id", SR.GetString("ArgumentOutOfRange_NeedValidId", (object) 1, (object) ushort.MaxValue));
      }
      else
      {
        this.m_id = (ushort) id;
        this.m_version = version;
        this.m_channel = channel;
        this.m_level = level;
        this.m_opcode = opcode;
        this.m_keywords = keywords;
        if (task < 0)
          throw new ArgumentOutOfRangeException("task", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
        if (task > (int) ushort.MaxValue)
          throw new ArgumentOutOfRangeException("task", SR.GetString("ArgumentOutOfRange_NeedValidId", (object) 1, (object) ushort.MaxValue));
        else
          this.m_task = (ushort) task;
      }
    }
  }
}
