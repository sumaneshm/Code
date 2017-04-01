// Type: System.Diagnostics.EventSchemaTraceListener
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  [HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
  public class EventSchemaTraceListener : TextWriterTraceListener
  {
    private static readonly string machineName = Environment.MachineName;
    private int _bufferSize = 32768;
    private TraceLogRetentionOption _retention = TraceLogRetentionOption.SingleFileUnboundedSize;
    private long _maxFileSize = 10240000L;
    private int _maxNumberOfFiles = 2;
    private readonly object m_lockObject = new object();
    private EventSchemaTraceListener.TraceWriter traceWriter;
    private string fileName;
    private bool _initialized;
    private const string s_optionBufferSize = "bufferSize";
    private const string s_optionLogRetention = "logRetentionOption";
    private const string s_optionMaximumFileSize = "maximumFileSize";
    private const string s_optionMaximumNumberOfFiles = "maximumNumberOfFiles";
    private const string s_userDataHeader = "<System.Diagnostics.UserData xmlns=\"http://schemas.microsoft.com/win/2006/09/System.Diagnostics/UserData/\">";
    private const string s_eventHeader = "<Event xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\"><System><Provider Guid=\"";
    private const int s_defaultPayloadSize = 512;
    private const int _retryThreshold = 2;

    public new TextWriter Writer
    {
      [SecurityCritical] get
      {
        this.EnsureWriter();
        return (TextWriter) this.traceWriter;
      }
      set
      {
        throw new NotSupportedException(SR.GetString("NotSupported_SetTextWriter"));
      }
    }

    public override bool IsThreadSafe
    {
      get
      {
        return true;
      }
    }

    public int BufferSize
    {
      get
      {
        this.Init();
        return this._bufferSize;
      }
    }

    public TraceLogRetentionOption TraceLogRetentionOption
    {
      get
      {
        this.Init();
        return this._retention;
      }
    }

    public long MaximumFileSize
    {
      get
      {
        this.Init();
        return this._maxFileSize;
      }
    }

    public int MaximumNumberOfFiles
    {
      get
      {
        this.Init();
        return this._maxNumberOfFiles;
      }
    }

    static EventSchemaTraceListener()
    {
    }

    public EventSchemaTraceListener(string fileName)
      : this(fileName, string.Empty)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventSchemaTraceListener(string fileName, string name)
      : this(fileName, name, 32768)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventSchemaTraceListener(string fileName, string name, int bufferSize)
      : this(fileName, name, bufferSize, TraceLogRetentionOption.SingleFileUnboundedSize)
    {
    }

    public EventSchemaTraceListener(string fileName, string name, int bufferSize, TraceLogRetentionOption logRetentionOption)
      : this(fileName, name, bufferSize, logRetentionOption, 10240000L)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventSchemaTraceListener(string fileName, string name, int bufferSize, TraceLogRetentionOption logRetentionOption, long maximumFileSize)
      : this(fileName, name, bufferSize, logRetentionOption, maximumFileSize, 2)
    {
    }

    public EventSchemaTraceListener(string fileName, string name, int bufferSize, TraceLogRetentionOption logRetentionOption, long maximumFileSize, int maximumNumberOfFiles)
    {
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException("bufferSize", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (logRetentionOption < TraceLogRetentionOption.UnlimitedSequentialFiles || logRetentionOption > TraceLogRetentionOption.SingleFileBoundedSize)
        throw new ArgumentOutOfRangeException("logRetentionOption", SR.GetString("ArgumentOutOfRange_NeedValidLogRetention"));
      this.Name = name;
      this.fileName = fileName;
      if (!string.IsNullOrEmpty(this.fileName) && (int) fileName[0] != (int) Path.DirectorySeparatorChar && ((int) fileName[0] != (int) Path.AltDirectorySeparatorChar && !Path.IsPathRooted(fileName)))
        this.fileName = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile), this.fileName);
      this._retention = logRetentionOption;
      this._bufferSize = bufferSize;
      this._SetMaxFileSize(maximumFileSize, false);
      this._SetMaxNumberOfFiles(maximumNumberOfFiles, false);
    }

    public override void Close()
    {
      try
      {
        if (this.traceWriter == null)
          return;
        this.traceWriter.Flush();
        this.traceWriter.Close();
      }
      finally
      {
        this.traceWriter = (EventSchemaTraceListener.TraceWriter) null;
        base.Close();
      }
    }

    [SecurityCritical]
    public override void Flush()
    {
      if (!this.EnsureWriter())
        return;
      this.traceWriter.Flush();
    }

    public override void Write(string message)
    {
      this.WriteLine(message);
    }

    public override void WriteLine(string message)
    {
      this.TraceEvent((TraceEventCache) null, SR.GetString("TraceAsTraceSource"), TraceEventType.Information, 0, message);
    }

    public override void Fail(string message, string detailMessage)
    {
      StringBuilder stringBuilder = new StringBuilder(message);
      if (detailMessage != null)
      {
        stringBuilder.Append(" ");
        stringBuilder.Append(detailMessage);
      }
      this.TraceEvent((TraceEventCache) null, SR.GetString("TraceAsTraceSource"), TraceEventType.Error, 0, ((object) stringBuilder).ToString());
    }

    [SecurityCritical]
    public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args, (object) null, (object[]) null))
        return;
      StringBuilder writer = new StringBuilder(512);
      EventSchemaTraceListener.BuildHeader(writer, source, eventType, id, eventCache, (string) null, false, this.TraceOutputOptions);
      string message = args == null ? format : string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args);
      EventSchemaTraceListener.BuildMessage(writer, message);
      EventSchemaTraceListener.BuildFooter(writer, eventType, eventCache, false, this.TraceOutputOptions);
      this._InternalWriteRaw(writer);
    }

    [SecurityCritical]
    public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, message, (object[]) null, (object) null, (object[]) null))
        return;
      StringBuilder writer = new StringBuilder(512);
      EventSchemaTraceListener.BuildHeader(writer, source, eventType, id, eventCache, (string) null, false, this.TraceOutputOptions);
      EventSchemaTraceListener.BuildMessage(writer, message);
      EventSchemaTraceListener.BuildFooter(writer, eventType, eventCache, false, this.TraceOutputOptions);
      this._InternalWriteRaw(writer);
    }

    [SecurityCritical]
    public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, data, (object[]) null))
        return;
      StringBuilder writer = new StringBuilder(512);
      EventSchemaTraceListener.BuildHeader(writer, source, eventType, id, eventCache, (string) null, true, this.TraceOutputOptions);
      if (data != null)
      {
        EventSchemaTraceListener._InternalBuildRaw(writer, "<System.Diagnostics.UserData xmlns=\"http://schemas.microsoft.com/win/2006/09/System.Diagnostics/UserData/\">");
        EventSchemaTraceListener.BuildUserData(writer, data);
        EventSchemaTraceListener._InternalBuildRaw(writer, "</System.Diagnostics.UserData>");
      }
      EventSchemaTraceListener.BuildFooter(writer, eventType, eventCache, true, this.TraceOutputOptions);
      this._InternalWriteRaw(writer);
    }

    [SecurityCritical]
    public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, (string) null, (object[]) null, (object) null, data))
        return;
      StringBuilder writer = new StringBuilder(512);
      EventSchemaTraceListener.BuildHeader(writer, source, eventType, id, eventCache, (string) null, true, this.TraceOutputOptions);
      if (data != null && data.Length > 0)
      {
        EventSchemaTraceListener._InternalBuildRaw(writer, "<System.Diagnostics.UserData xmlns=\"http://schemas.microsoft.com/win/2006/09/System.Diagnostics/UserData/\">");
        for (int index = 0; index < data.Length; ++index)
        {
          if (data[index] != null)
            EventSchemaTraceListener.BuildUserData(writer, data[index]);
        }
        EventSchemaTraceListener._InternalBuildRaw(writer, "</System.Diagnostics.UserData>");
      }
      EventSchemaTraceListener.BuildFooter(writer, eventType, eventCache, true, this.TraceOutputOptions);
      this._InternalWriteRaw(writer);
    }

    [SecurityCritical]
    public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
    {
      StringBuilder writer = new StringBuilder(512);
      EventSchemaTraceListener.BuildHeader(writer, source, TraceEventType.Transfer, id, eventCache, relatedActivityId.ToString("B"), false, this.TraceOutputOptions);
      EventSchemaTraceListener.BuildMessage(writer, message);
      EventSchemaTraceListener.BuildFooter(writer, TraceEventType.Transfer, eventCache, false, this.TraceOutputOptions);
      this._InternalWriteRaw(writer);
    }

    protected override string[] GetSupportedAttributes()
    {
      return new string[4]
      {
        "bufferSize",
        "logRetentionOption",
        "maximumFileSize",
        "maximumNumberOfFiles"
      };
    }

    private static void BuildMessage(StringBuilder writer, string message)
    {
      EventSchemaTraceListener._InternalBuildRaw(writer, "<Data>");
      EventSchemaTraceListener.BuildEscaped(writer, message);
      EventSchemaTraceListener._InternalBuildRaw(writer, "</Data>");
    }

    [SecurityCritical]
    private static void BuildHeader(StringBuilder writer, string source, TraceEventType eventType, int id, TraceEventCache eventCache, string relatedActivityId, bool isUserData, TraceOptions opts)
    {
      EventSchemaTraceListener._InternalBuildRaw(writer, "<Event xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\"><System><Provider Guid=\"");
      EventSchemaTraceListener._InternalBuildRaw(writer, "{00000000-0000-0000-0000-000000000000}");
      EventSchemaTraceListener._InternalBuildRaw(writer, "\"/><EventID>");
      EventSchemaTraceListener._InternalBuildRaw(writer, (id < 0 ? 0U : (uint) id).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      EventSchemaTraceListener._InternalBuildRaw(writer, "</EventID>");
      EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>");
      int num1 = (int) eventType;
      int num2 = num1;
      if (num1 > (int) byte.MaxValue || num1 < 0)
        num1 = 8;
      EventSchemaTraceListener._InternalBuildRaw(writer, num1.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      EventSchemaTraceListener._InternalBuildRaw(writer, "</Level>");
      if (num2 > (int) byte.MaxValue)
      {
        int num3 = num2 / 256;
        EventSchemaTraceListener._InternalBuildRaw(writer, "<Opcode>");
        EventSchemaTraceListener._InternalBuildRaw(writer, num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        EventSchemaTraceListener._InternalBuildRaw(writer, "</Opcode>");
      }
      if ((TraceOptions.DateTime & opts) != TraceOptions.None)
      {
        EventSchemaTraceListener._InternalBuildRaw(writer, "<TimeCreated SystemTime=\"");
        if (eventCache != null)
          EventSchemaTraceListener._InternalBuildRaw(writer, eventCache.DateTime.ToString("o", (IFormatProvider) CultureInfo.InvariantCulture));
        else
          EventSchemaTraceListener._InternalBuildRaw(writer, DateTime.UtcNow.ToString("o", (IFormatProvider) CultureInfo.InvariantCulture));
        EventSchemaTraceListener._InternalBuildRaw(writer, "\"/>");
      }
      EventSchemaTraceListener._InternalBuildRaw(writer, "<Correlation ActivityID=\"");
      EventSchemaTraceListener._InternalBuildRaw(writer, Trace.CorrelationManager.ActivityId.ToString("B"));
      if (relatedActivityId != null)
      {
        EventSchemaTraceListener._InternalBuildRaw(writer, "\" RelatedActivityID=\"");
        EventSchemaTraceListener._InternalBuildRaw(writer, relatedActivityId);
      }
      EventSchemaTraceListener._InternalBuildRaw(writer, "\"/>");
      if (eventCache != null && ((TraceOptions.ProcessId | TraceOptions.ThreadId) & opts) != TraceOptions.None)
      {
        EventSchemaTraceListener._InternalBuildRaw(writer, "<Execution ");
        EventSchemaTraceListener._InternalBuildRaw(writer, "ProcessID=\"");
        EventSchemaTraceListener._InternalBuildRaw(writer, ((uint) eventCache.ProcessId).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        EventSchemaTraceListener._InternalBuildRaw(writer, "\" ");
        EventSchemaTraceListener._InternalBuildRaw(writer, "ThreadID=\"");
        EventSchemaTraceListener._InternalBuildRaw(writer, eventCache.ThreadId);
        EventSchemaTraceListener._InternalBuildRaw(writer, "\"");
        EventSchemaTraceListener._InternalBuildRaw(writer, "/>");
      }
      EventSchemaTraceListener._InternalBuildRaw(writer, "<Computer>");
      EventSchemaTraceListener._InternalBuildRaw(writer, EventSchemaTraceListener.machineName);
      EventSchemaTraceListener._InternalBuildRaw(writer, "</Computer>");
      EventSchemaTraceListener._InternalBuildRaw(writer, "</System>");
      if (!isUserData)
        EventSchemaTraceListener._InternalBuildRaw(writer, "<EventData>");
      else
        EventSchemaTraceListener._InternalBuildRaw(writer, "<UserData>");
    }

    private static void BuildFooter(StringBuilder writer, TraceEventType eventType, TraceEventCache eventCache, bool isUserData, TraceOptions opts)
    {
      if (!isUserData)
        EventSchemaTraceListener._InternalBuildRaw(writer, "</EventData>");
      else
        EventSchemaTraceListener._InternalBuildRaw(writer, "</UserData>");
      EventSchemaTraceListener._InternalBuildRaw(writer, "<RenderingInfo Culture=\"en-EN\">");
      switch (eventType)
      {
        case TraceEventType.Resume:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Information</Level><Opcode>Resume</Opcode>");
          break;
        case TraceEventType.Transfer:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Information</Level><Opcode>Transfer</Opcode>");
          break;
        case TraceEventType.Stop:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Information</Level><Opcode>Stop</Opcode>");
          break;
        case TraceEventType.Suspend:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Information</Level><Opcode>Suspend</Opcode>");
          break;
        case TraceEventType.Verbose:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Verbose</Level>");
          break;
        case TraceEventType.Start:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Information</Level><Opcode>Start</Opcode>");
          break;
        case TraceEventType.Critical:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Critical</Level>");
          break;
        case TraceEventType.Error:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Error</Level>");
          break;
        case TraceEventType.Warning:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Warning</Level>");
          break;
        case TraceEventType.Information:
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Level>Information</Level>");
          break;
      }
      EventSchemaTraceListener._InternalBuildRaw(writer, "</RenderingInfo>");
      if (eventCache != null && ((TraceOptions.LogicalOperationStack | TraceOptions.Timestamp | TraceOptions.Callstack) & opts) != TraceOptions.None)
      {
        EventSchemaTraceListener._InternalBuildRaw(writer, "<System.Diagnostics.ExtendedData xmlns=\"http://schemas.microsoft.com/2006/09/System.Diagnostics/ExtendedData\">");
        if ((TraceOptions.Timestamp & opts) != TraceOptions.None)
        {
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Timestamp>");
          EventSchemaTraceListener._InternalBuildRaw(writer, eventCache.Timestamp.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          EventSchemaTraceListener._InternalBuildRaw(writer, "</Timestamp>");
        }
        if ((TraceOptions.LogicalOperationStack & opts) != TraceOptions.None)
        {
          Stack logicalOperationStack = eventCache.LogicalOperationStack;
          EventSchemaTraceListener._InternalBuildRaw(writer, "<LogicalOperationStack>");
          if (logicalOperationStack != null && logicalOperationStack.Count > 0)
          {
            foreach (object obj in logicalOperationStack)
            {
              EventSchemaTraceListener._InternalBuildRaw(writer, "<LogicalOperation>");
              EventSchemaTraceListener.BuildEscaped(writer, obj.ToString());
              EventSchemaTraceListener._InternalBuildRaw(writer, "</LogicalOperation>");
            }
          }
          EventSchemaTraceListener._InternalBuildRaw(writer, "</LogicalOperationStack>");
        }
        if ((TraceOptions.Callstack & opts) != TraceOptions.None)
        {
          EventSchemaTraceListener._InternalBuildRaw(writer, "<Callstack>");
          EventSchemaTraceListener.BuildEscaped(writer, eventCache.Callstack);
          EventSchemaTraceListener._InternalBuildRaw(writer, "</Callstack>");
        }
        EventSchemaTraceListener._InternalBuildRaw(writer, "</System.Diagnostics.ExtendedData>");
      }
      EventSchemaTraceListener._InternalBuildRaw(writer, "</Event>");
    }

    private static void BuildEscaped(StringBuilder writer, string str)
    {
      if (str == null)
        return;
      int startIndex = 0;
      for (int index = 0; index < str.Length; ++index)
      {
        switch (str[index])
        {
          case '"':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&quot;");
            startIndex = index + 1;
            break;
          case '&':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&amp;");
            startIndex = index + 1;
            break;
          case '\'':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&apos;");
            startIndex = index + 1;
            break;
          case '<':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&lt;");
            startIndex = index + 1;
            break;
          case '>':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&gt;");
            startIndex = index + 1;
            break;
          case '\n':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&#xA;");
            startIndex = index + 1;
            break;
          case '\r':
            EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, index - startIndex));
            EventSchemaTraceListener._InternalBuildRaw(writer, "&#xD;");
            startIndex = index + 1;
            break;
        }
      }
      EventSchemaTraceListener._InternalBuildRaw(writer, str.Substring(startIndex, str.Length - startIndex));
    }

    private static void BuildUserData(StringBuilder writer, object data)
    {
      UnescapedXmlDiagnosticData xmlDiagnosticData = data as UnescapedXmlDiagnosticData;
      if (xmlDiagnosticData == null)
        EventSchemaTraceListener.BuildMessage(writer, data.ToString());
      else
        EventSchemaTraceListener._InternalBuildRaw(writer, xmlDiagnosticData.ToString());
    }

    private static void _InternalBuildRaw(StringBuilder writer, string message)
    {
      writer.Append(message);
    }

    [SecurityCritical]
    private void _InternalWriteRaw(StringBuilder writer)
    {
      if (!this.EnsureWriter())
        return;
      this.traceWriter.Write(((object) writer).ToString());
    }

    private void Init()
    {
      if (this._initialized)
        return;
      lock (this.m_lockObject)
      {
        if (this._initialized)
          return;
        try
        {
          if (this.Attributes.ContainsKey("bufferSize"))
          {
            int local_0 = int.Parse(this.Attributes["bufferSize"], (IFormatProvider) CultureInfo.InvariantCulture);
            if (local_0 > 0)
              this._bufferSize = local_0;
          }
          if (this.Attributes.ContainsKey("logRetentionOption"))
          {
            string local_1 = this.Attributes["logRetentionOption"];
            this._retention = string.Compare(local_1, "SingleFileUnboundedSize", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(local_1, "LimitedCircularFiles", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(local_1, "UnlimitedSequentialFiles", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(local_1, "SingleFileBoundedSize", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(local_1, "LimitedSequentialFiles", StringComparison.OrdinalIgnoreCase) != 0 ? TraceLogRetentionOption.SingleFileUnboundedSize : TraceLogRetentionOption.LimitedSequentialFiles) : TraceLogRetentionOption.SingleFileBoundedSize) : TraceLogRetentionOption.UnlimitedSequentialFiles) : TraceLogRetentionOption.LimitedCircularFiles) : TraceLogRetentionOption.SingleFileUnboundedSize;
          }
          if (this.Attributes.ContainsKey("maximumFileSize"))
            this._SetMaxFileSize(long.Parse(this.Attributes["maximumFileSize"], (IFormatProvider) CultureInfo.InvariantCulture), false);
          if (!this.Attributes.ContainsKey("maximumNumberOfFiles"))
            return;
          this._SetMaxNumberOfFiles(int.Parse(this.Attributes["maximumNumberOfFiles"], (IFormatProvider) CultureInfo.InvariantCulture), false);
        }
        catch (Exception exception_0)
        {
        }
        finally
        {
          this._initialized = true;
        }
      }
    }

    private void _SetMaxFileSize(long maximumFileSize, bool throwOnError)
    {
      switch (this._retention)
      {
        case TraceLogRetentionOption.UnlimitedSequentialFiles:
        case TraceLogRetentionOption.LimitedCircularFiles:
        case TraceLogRetentionOption.LimitedSequentialFiles:
        case TraceLogRetentionOption.SingleFileBoundedSize:
          if (maximumFileSize < 0L && throwOnError)
            throw new ArgumentOutOfRangeException("maximumFileSize", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
          if (maximumFileSize < (long) this._bufferSize)
          {
            if (throwOnError)
              throw new ArgumentOutOfRangeException("maximumFileSize", SR.GetString("ArgumentOutOfRange_NeedMaxFileSizeGEBufferSize"));
            this._maxFileSize = (long) this._bufferSize;
            break;
          }
          else
          {
            this._maxFileSize = maximumFileSize;
            break;
          }
        case TraceLogRetentionOption.SingleFileUnboundedSize:
          this._maxFileSize = -1L;
          break;
      }
    }

    private void _SetMaxNumberOfFiles(int maximumNumberOfFiles, bool throwOnError)
    {
      switch (this._retention)
      {
        case TraceLogRetentionOption.UnlimitedSequentialFiles:
          this._maxNumberOfFiles = -1;
          break;
        case TraceLogRetentionOption.LimitedCircularFiles:
          if (maximumNumberOfFiles < 2)
          {
            if (throwOnError)
            {
              throw new ArgumentOutOfRangeException("maximumNumberOfFiles", SR.GetString("ArgumentOutOfRange_NeedValidMaxNumFiles", new object[1]
              {
                (object) 2
              }));
            }
            else
            {
              this._maxNumberOfFiles = 2;
              break;
            }
          }
          else
          {
            this._maxNumberOfFiles = maximumNumberOfFiles;
            break;
          }
        case TraceLogRetentionOption.SingleFileUnboundedSize:
        case TraceLogRetentionOption.SingleFileBoundedSize:
          this._maxNumberOfFiles = 1;
          break;
        case TraceLogRetentionOption.LimitedSequentialFiles:
          if (maximumNumberOfFiles < 1)
          {
            if (throwOnError)
            {
              throw new ArgumentOutOfRangeException("maximumNumberOfFiles", SR.GetString("ArgumentOutOfRange_NeedValidMaxNumFiles", new object[1]
              {
                (object) 1
              }));
            }
            else
            {
              this._maxNumberOfFiles = 1;
              break;
            }
          }
          else
          {
            this._maxNumberOfFiles = maximumNumberOfFiles;
            break;
          }
      }
    }

    [SecurityCritical]
    private new bool EnsureWriter()
    {
      if (this.traceWriter == null)
      {
        if (string.IsNullOrEmpty(this.fileName))
          return false;
        lock (this.m_lockObject)
        {
          if (this.traceWriter != null)
            return true;
          string local_0 = this.fileName;
          for (int local_1 = 0; local_1 < 2; ++local_1)
          {
            try
            {
              this.Init();
              this.traceWriter = new EventSchemaTraceListener.TraceWriter(local_0, this._bufferSize, this._retention, this._maxFileSize, this._maxNumberOfFiles);
              break;
            }
            catch (IOException exception_0)
            {
              local_0 = Path.GetFileNameWithoutExtension(this.fileName) + Guid.NewGuid().ToString() + Path.GetExtension(this.fileName);
            }
            catch (UnauthorizedAccessException exception_1)
            {
              break;
            }
            catch (Exception exception_2)
            {
              break;
            }
          }
          if (this.traceWriter == null)
            this.fileName = (string) null;
        }
      }
      return this.traceWriter != null;
    }

    private sealed class TraceWriter : TextWriter
    {
      private object m_lockObject = new object();
      private Encoding encNoBOMwithFallback;
      private Stream stream;

      public override Encoding Encoding
      {
        get
        {
          if (this.encNoBOMwithFallback == null)
          {
            lock (this.m_lockObject)
            {
              if (this.encNoBOMwithFallback == null)
                this.encNoBOMwithFallback = EventSchemaTraceListener.TraceWriter.GetEncodingWithFallback((Encoding) new UTF8Encoding(false));
            }
          }
          return this.encNoBOMwithFallback;
        }
      }

      [SecurityCritical]
      internal TraceWriter(string _fileName, int bufferSize, TraceLogRetentionOption retention, long maxFileSize, int maxNumberOfFiles)
        : base((IFormatProvider) CultureInfo.InvariantCulture)
      {
        this.stream = (Stream) new LogStream(_fileName, bufferSize, (LogRetentionOption) retention, maxFileSize, maxNumberOfFiles);
      }

      public override void Write(string value)
      {
        try
        {
          byte[] bytes = this.Encoding.GetBytes(value);
          this.stream.Write(bytes, 0, bytes.Length);
        }
        catch (Exception ex)
        {
          if (!(this.stream is BufferedStream2))
            return;
          ((BufferedStream2) this.stream).DiscardBuffer();
        }
      }

      public override void Flush()
      {
        this.stream.Flush();
      }

      protected override void Dispose(bool disposing)
      {
        try
        {
          if (!disposing)
            return;
          this.stream.Close();
        }
        finally
        {
          base.Dispose(disposing);
        }
      }

      private static Encoding GetEncodingWithFallback(Encoding encoding)
      {
        Encoding encoding1 = (Encoding) encoding.Clone();
        encoding1.EncoderFallback = EncoderFallback.ReplacementFallback;
        encoding1.DecoderFallback = DecoderFallback.ReplacementFallback;
        return encoding1;
      }
    }
  }
}
