// Type: System.Diagnostics.Eventing.EventProviderTraceListener
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics.Eventing
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventProviderTraceListener : TraceListener
  {
    private string m_delimiter = ";";
    private object m_Lock = new object();
    private EventProvider m_provider;
    private int m_initializedDelim;
    private const string s_nullStringValue = "null";
    private const string s_nullStringComaValue = "null,";
    private const string s_nullCStringValue = ": null";
    private const string s_activityIdString = "activityId=";
    private const string s_relatedActivityIdString = "relatedActivityId=";
    private const string s_callStackString = " : CallStack:";
    private const string s_optionDelimiter = "delimiter";
    private const uint s_keyWordMask = 4294967040U;
    private const int s_defaultPayloadSize = 512;

    public string Delimiter
    {
      get
      {
        if (this.m_initializedDelim == 0)
        {
          lock (this.m_Lock)
          {
            if (this.m_initializedDelim == 0)
            {
              if (this.Attributes.ContainsKey("delimiter"))
                this.m_delimiter = this.Attributes["delimiter"];
              this.m_initializedDelim = 1;
            }
          }
          if (this.m_delimiter == null)
            throw new ArgumentNullException("Delimiter");
          if (this.m_delimiter.Length == 0)
            throw new ArgumentException(SR.GetString("Argument_NeedNonemptyDelimiter"));
        }
        return this.m_delimiter;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("Delimiter");
        if (value.Length == 0)
          throw new ArgumentException(SR.GetString("Argument_NeedNonemptyDelimiter"));
        lock (this.m_Lock)
        {
          this.m_delimiter = value;
          this.m_initializedDelim = 1;
        }
      }
    }

    public override sealed bool IsThreadSafe
    {
      get
      {
        return true;
      }
    }

    public EventProviderTraceListener(string providerId)
    {
      this.InitProvider(providerId);
    }

    public EventProviderTraceListener(string providerId, string name)
      : base(name)
    {
      this.InitProvider(providerId);
    }

    public EventProviderTraceListener(string providerId, string name, string delimiter)
      : base(name)
    {
      if (delimiter == null)
        throw new ArgumentNullException("delimiter");
      if (delimiter.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_NeedNonemptyDelimiter"));
      this.m_delimiter = delimiter;
      this.m_initializedDelim = 1;
      this.InitProvider(providerId);
    }

    protected override string[] GetSupportedAttributes()
    {
      return new string[1]
      {
        "delimiter"
      };
    }

    public override sealed void Flush()
    {
    }

    public override void Close()
    {
      this.m_provider.Close();
    }

    public override sealed void Write(string message)
    {
      if (!this.m_provider.IsEnabled())
        return;
      this.m_provider.WriteMessageEvent(message, (byte) 8, 0L);
    }

    public override sealed void WriteLine(string message)
    {
      this.Write(message);
    }

    public override sealed void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
    {
      if (!this.m_provider.IsEnabled() || this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, (object) null, (object[]) null))
        return;
      StringBuilder stringBuilder = new StringBuilder(512);
      if (data != null)
        stringBuilder.Append(data.ToString());
      else
        stringBuilder.Append(": null");
      if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
      {
        stringBuilder.Append(" : CallStack:");
        stringBuilder.Append(eventCache.Callstack);
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) eventType, (long) eventType & 4294967040L);
      }
      else
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) eventType, (long) eventType & 4294967040L);
    }

    public override sealed void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
    {
      if (!this.m_provider.IsEnabled() || this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, (object) null, (object[]) null))
        return;
      StringBuilder stringBuilder = new StringBuilder(512);
      if (data != null && data.Length > 0)
      {
        int index;
        for (index = 0; index < data.Length - 1; ++index)
        {
          if (data[index] != null)
          {
            stringBuilder.Append(data[index].ToString());
            stringBuilder.Append(this.Delimiter);
          }
          else
            stringBuilder.Append("null,");
        }
        if (data[index] != null)
          stringBuilder.Append(data[index].ToString());
        else
          stringBuilder.Append("null");
      }
      else
        stringBuilder.Append("null");
      if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
      {
        stringBuilder.Append(" : CallStack:");
        stringBuilder.Append(eventCache.Callstack);
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) eventType, (long) eventType & 4294967040L);
      }
      else
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) eventType, (long) eventType & 4294967040L);
    }

    public override sealed void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
    {
      if (!this.m_provider.IsEnabled() || this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, (object) null, (object[]) null))
        return;
      if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
        this.m_provider.WriteMessageEvent(" : CallStack:" + eventCache.Callstack, (byte) eventType, (long) eventType & 4294967040L);
      else
        this.m_provider.WriteMessageEvent(string.Empty, (byte) eventType, (long) eventType & 4294967040L);
    }

    public override sealed void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
    {
      if (!this.m_provider.IsEnabled() || this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, (object) null, (object[]) null))
        return;
      StringBuilder stringBuilder = new StringBuilder(512);
      stringBuilder.Append(message);
      if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
      {
        stringBuilder.Append(" : CallStack:");
        stringBuilder.Append(eventCache.Callstack);
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) eventType, (long) eventType & 4294967040L);
      }
      else
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) eventType, (long) eventType & 4294967040L);
    }

    public override sealed void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
    {
      if (!this.m_provider.IsEnabled() || this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, (object) null, (object[]) null))
        return;
      if (args == null)
      {
        if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
          this.m_provider.WriteMessageEvent(format + " : CallStack:" + eventCache.Callstack, (byte) eventType, (long) eventType & 4294967040L);
        else
          this.m_provider.WriteMessageEvent(format, (byte) eventType, (long) eventType & 4294967040L);
      }
      else if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
        this.m_provider.WriteMessageEvent(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args) + " : CallStack:" + eventCache.Callstack, (byte) eventType, (long) eventType & 4294967040L);
      else
        this.m_provider.WriteMessageEvent(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args), (byte) eventType, (long) eventType & 4294967040L);
    }

    public override void Fail(string message, string detailMessage)
    {
      StringBuilder stringBuilder = new StringBuilder(message);
      if (detailMessage != null)
      {
        stringBuilder.Append(" ");
        stringBuilder.Append(detailMessage);
      }
      this.TraceEvent((TraceEventCache) null, (string) null, TraceEventType.Error, 0, ((object) stringBuilder).ToString());
    }

    [SecurityCritical]
    public override sealed void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
    {
      if (!this.m_provider.IsEnabled())
        return;
      StringBuilder stringBuilder = new StringBuilder(512);
      object obj = (object) Trace.CorrelationManager.ActivityId;
      if (obj != null)
      {
        Guid guid = (Guid) obj;
        stringBuilder.Append("activityId=");
        stringBuilder.Append(guid.ToString());
        stringBuilder.Append(this.Delimiter);
      }
      stringBuilder.Append("relatedActivityId=");
      stringBuilder.Append(relatedActivityId.ToString());
      stringBuilder.Append(this.Delimiter + message);
      if (eventCache != null && (this.TraceOutputOptions & TraceOptions.Callstack) != TraceOptions.None)
      {
        stringBuilder.Append(" : CallStack:");
        stringBuilder.Append(eventCache.Callstack);
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) 0, 4096L);
      }
      else
        this.m_provider.WriteMessageEvent(((object) stringBuilder).ToString(), (byte) 0, 4096L);
    }

    private void InitProvider(string providerId)
    {
      this.m_provider = new EventProvider(new Guid(providerId));
    }
  }
}
