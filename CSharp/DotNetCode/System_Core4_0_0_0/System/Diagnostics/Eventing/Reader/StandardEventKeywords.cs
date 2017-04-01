// Type: System.Diagnostics.Eventing.Reader.StandardEventKeywords
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Diagnostics.Eventing.Reader
{
  [Flags]
  public enum StandardEventKeywords : long
  {
    None = 0L,
    ResponseTime = 281474976710656L,
    WdiContext = 562949953421312L,
    WdiDiagnostic = 1125899906842624L,
    Sqm = 2251799813685248L,
    AuditFailure = 4503599627370496L,
    AuditSuccess = 9007199254740992L,
    [Obsolete("Incorrect value: use CorrelationHint2 instead", false)] CorrelationHint = AuditFailure,
    CorrelationHint2 = 18014398509481984L,
    EventLogClassic = 36028797018963968L,
  }
}
