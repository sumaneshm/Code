// Type: System.IO.MemoryMappedFiles.MemoryMappedFileRights
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.IO.MemoryMappedFiles
{
  [Flags]
  public enum MemoryMappedFileRights
  {
    CopyOnWrite = 1,
    Write = 2,
    Read = 4,
    Execute = 8,
    Delete = 65536,
    ReadPermissions = 131072,
    ChangePermissions = 262144,
    TakeOwnership = 524288,
    ReadWrite = Read | Write,
    ReadExecute = Execute | Read,
    ReadWriteExecute = ReadExecute | Write,
    FullControl = ReadWriteExecute | TakeOwnership | ChangePermissions | ReadPermissions | Delete | CopyOnWrite,
    AccessSystemSecurity = 16777216,
  }
}
