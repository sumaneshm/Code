// Type: System.IO.Pipes.PipeAccessRights
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.IO.Pipes
{
  [Flags]
  public enum PipeAccessRights
  {
    ReadData = 1,
    WriteData = 2,
    ReadAttributes = 128,
    WriteAttributes = 256,
    ReadExtendedAttributes = 8,
    WriteExtendedAttributes = 16,
    CreateNewInstance = 4,
    Delete = 65536,
    ReadPermissions = 131072,
    ChangePermissions = 262144,
    TakeOwnership = 524288,
    Synchronize = 1048576,
    FullControl = Synchronize | TakeOwnership | ChangePermissions | ReadPermissions | Delete | CreateNewInstance | WriteExtendedAttributes | ReadExtendedAttributes | WriteAttributes | ReadAttributes | WriteData | ReadData,
    Read = ReadPermissions | ReadExtendedAttributes | ReadAttributes | ReadData,
    Write = WriteExtendedAttributes | WriteAttributes | WriteData,
    ReadWrite = Write | Read,
    AccessSystemSecurity = 16777216,
  }
}
