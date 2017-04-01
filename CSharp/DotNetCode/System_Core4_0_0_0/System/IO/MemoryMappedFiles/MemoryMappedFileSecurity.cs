// Type: System.IO.MemoryMappedFiles.MemoryMappedFileSecurity
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace System.IO.MemoryMappedFiles
{
  public class MemoryMappedFileSecurity : ObjectSecurity<MemoryMappedFileRights>
  {
    public MemoryMappedFileSecurity()
      : base(false, ResourceType.KernelObject)
    {
    }

    [SecuritySafeCritical]
    internal MemoryMappedFileSecurity(SafeMemoryMappedFileHandle safeHandle, AccessControlSections includeSections)
      : base(false, ResourceType.KernelObject, (SafeHandle) safeHandle, includeSections)
    {
    }

    [SecuritySafeCritical]
    internal void PersistHandle(SafeHandle handle)
    {
      this.Persist(handle);
    }
  }
}
