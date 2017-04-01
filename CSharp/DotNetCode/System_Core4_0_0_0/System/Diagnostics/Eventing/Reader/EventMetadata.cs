// Type: System.Diagnostics.Eventing.Reader.EventMetadata
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class EventMetadata
  {
    private long id;
    private byte version;
    private byte channelId;
    private byte level;
    private short opcode;
    private int task;
    private long keywords;
    private string template;
    private string description;
    private ProviderMetadata pmReference;

    public long Id
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.id;
      }
    }

    public byte Version
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.version;
      }
    }

    public EventLogLink LogLink
    {
      get
      {
        return new EventLogLink((uint) this.channelId, this.pmReference);
      }
    }

    public EventLevel Level
    {
      get
      {
        return new EventLevel((int) this.level, this.pmReference);
      }
    }

    public EventOpcode Opcode
    {
      get
      {
        return new EventOpcode((int) this.opcode, this.pmReference);
      }
    }

    public EventTask Task
    {
      get
      {
        return new EventTask(this.task, this.pmReference);
      }
    }

    public IEnumerable<EventKeyword> Keywords
    {
      get
      {
        List<EventKeyword> list = new List<EventKeyword>();
        ulong num1 = (ulong) this.keywords;
        ulong num2 = 9223372036854775808UL;
        for (int index = 0; index < 64; ++index)
        {
          if ((num1 & num2) > 0UL)
            list.Add(new EventKeyword((long) num2, this.pmReference));
          num2 >>= 1;
        }
        return (IEnumerable<EventKeyword>) list;
      }
    }

    public string Template
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.template;
      }
    }

    public string Description
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.description;
      }
    }

    internal EventMetadata(uint id, byte version, byte channelId, byte level, byte opcode, short task, long keywords, string template, string description, ProviderMetadata pmReference)
    {
      this.id = (long) id;
      this.version = version;
      this.channelId = channelId;
      this.level = level;
      this.opcode = (short) opcode;
      this.task = (int) task;
      this.keywords = keywords;
      this.template = template;
      this.description = description;
      this.pmReference = pmReference;
    }
  }
}
