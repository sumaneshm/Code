// Type: Microsoft.Win32.UnsafeNativeMethods
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.Eventing;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace Microsoft.Win32
{
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethods
  {
    internal static readonly IntPtr NULL = IntPtr.Zero;
    internal const string KERNEL32 = "kernel32.dll";
    internal const string ADVAPI32 = "advapi32.dll";
    internal const string WEVTAPI = "wevtapi.dll";
    internal const int CREDUI_MAX_USERNAME_LENGTH = 513;
    internal const int ERROR_SUCCESS = 0;
    internal const int ERROR_FILE_NOT_FOUND = 2;
    internal const int ERROR_PATH_NOT_FOUND = 3;
    internal const int ERROR_ACCESS_DENIED = 5;
    internal const int ERROR_INVALID_HANDLE = 6;
    internal const int ERROR_NOT_ENOUGH_MEMORY = 8;
    internal const int ERROR_INVALID_DRIVE = 15;
    internal const int ERROR_NO_MORE_FILES = 18;
    internal const int ERROR_NOT_READY = 21;
    internal const int ERROR_BAD_LENGTH = 24;
    internal const int ERROR_SHARING_VIOLATION = 32;
    internal const int ERROR_LOCK_VIOLATION = 33;
    internal const int ERROR_HANDLE_EOF = 38;
    internal const int ERROR_FILE_EXISTS = 80;
    internal const int ERROR_INVALID_PARAMETER = 87;
    internal const int ERROR_BROKEN_PIPE = 109;
    internal const int ERROR_INSUFFICIENT_BUFFER = 122;
    internal const int ERROR_INVALID_NAME = 123;
    internal const int ERROR_BAD_PATHNAME = 161;
    internal const int ERROR_ALREADY_EXISTS = 183;
    internal const int ERROR_ENVVAR_NOT_FOUND = 203;
    internal const int ERROR_FILENAME_EXCED_RANGE = 206;
    internal const int ERROR_PIPE_BUSY = 231;
    internal const int ERROR_NO_DATA = 232;
    internal const int ERROR_PIPE_NOT_CONNECTED = 233;
    internal const int ERROR_MORE_DATA = 234;
    internal const int ERROR_NO_MORE_ITEMS = 259;
    internal const int ERROR_PIPE_CONNECTED = 535;
    internal const int ERROR_PIPE_LISTENING = 536;
    internal const int ERROR_OPERATION_ABORTED = 995;
    internal const int ERROR_IO_PENDING = 997;
    internal const int ERROR_NOT_FOUND = 1168;
    internal const int ERROR_ARITHMETIC_OVERFLOW = 534;
    internal const int ERROR_RESOURCE_LANG_NOT_FOUND = 1815;
    internal const int ERROR_EVT_MESSAGE_NOT_FOUND = 15027;
    internal const int ERROR_EVT_MESSAGE_ID_NOT_FOUND = 15028;
    internal const int ERROR_EVT_UNRESOLVED_VALUE_INSERT = 15029;
    internal const int ERROR_EVT_UNRESOLVED_PARAMETER_INSERT = 15030;
    internal const int ERROR_EVT_MAX_INSERTS_REACHED = 15031;
    internal const int ERROR_EVT_MESSAGE_LOCALE_NOT_FOUND = 15033;
    internal const int ERROR_MUI_FILE_NOT_FOUND = 15100;
    internal const int SECURITY_SQOS_PRESENT = 1048576;
    internal const int SECURITY_ANONYMOUS = 0;
    internal const int SECURITY_IDENTIFICATION = 65536;
    internal const int SECURITY_IMPERSONATION = 131072;
    internal const int SECURITY_DELEGATION = 196608;
    internal const int GENERIC_READ = -2147483648;
    internal const int GENERIC_WRITE = 1073741824;
    internal const int STD_INPUT_HANDLE = -10;
    internal const int STD_OUTPUT_HANDLE = -11;
    internal const int STD_ERROR_HANDLE = -12;
    internal const int DUPLICATE_SAME_ACCESS = 2;
    internal const int PIPE_ACCESS_INBOUND = 1;
    internal const int PIPE_ACCESS_OUTBOUND = 2;
    internal const int PIPE_ACCESS_DUPLEX = 3;
    internal const int PIPE_TYPE_BYTE = 0;
    internal const int PIPE_TYPE_MESSAGE = 4;
    internal const int PIPE_READMODE_BYTE = 0;
    internal const int PIPE_READMODE_MESSAGE = 2;
    internal const int PIPE_UNLIMITED_INSTANCES = 255;
    internal const int FILE_FLAG_FIRST_PIPE_INSTANCE = 524288;
    internal const int FILE_SHARE_READ = 1;
    internal const int FILE_SHARE_WRITE = 2;
    internal const int FILE_ATTRIBUTE_NORMAL = 128;
    internal const int FILE_FLAG_OVERLAPPED = 1073741824;
    internal const int OPEN_EXISTING = 3;
    internal const int FILE_TYPE_DISK = 1;
    internal const int FILE_TYPE_CHAR = 2;
    internal const int FILE_TYPE_PIPE = 3;
    internal const int MEM_COMMIT = 4096;
    internal const int MEM_RESERVE = 8192;
    internal const int INVALID_FILE_SIZE = -1;
    internal const int PAGE_READWRITE = 4;
    internal const int PAGE_READONLY = 2;
    internal const int PAGE_WRITECOPY = 8;
    internal const int PAGE_EXECUTE_READ = 32;
    internal const int PAGE_EXECUTE_READWRITE = 64;
    internal const int FILE_MAP_COPY = 1;
    internal const int FILE_MAP_WRITE = 2;
    internal const int FILE_MAP_READ = 4;
    internal const int FILE_MAP_EXECUTE = 32;
    internal const int SEM_FAILCRITICALERRORS = 1;
    private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;
    private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;
    private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

    static UnsafeNativeMethods()
    {
    }

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool FreeLibrary(IntPtr hModule);

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool RevertToSelf();

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool ImpersonateNamedPipeClient(SafePipeHandle hNamedPipe);

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("wevtapi.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtClose(IntPtr handle);

    [SecurityCritical]
    [DllImport("kernel32.dll")]
    internal static int GetFileType(SafeFileHandle handle);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, NativeOverlapped* lpOverlapped);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, UnsafeNativeMethods.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

    [SecurityCritical]
    internal static SafeFileHandle SafeCreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, UnsafeNativeMethods.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile)
    {
      SafeFileHandle file = UnsafeNativeMethods.CreateFile(lpFileName, dwDesiredAccess, dwShareMode, securityAttrs, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
      if (file.IsInvalid || UnsafeNativeMethods.GetFileType(file) == 1)
        return file;
      file.Dispose();
      throw new NotSupportedException(SR.GetString("NotSupported_IONonFileDevices"));
    }

    [SecurityCritical]
    [DllImport("kernel32.dll")]
    internal static int SetErrorMode(int newMode);

    [SecurityCritical]
    [DllImport("kernel32.dll", EntryPoint = "SetFilePointer", SetLastError = true)]
    private static int SetFilePointerWin32(SafeFileHandle handle, int lo, int* hi, int origin);

    [SecurityCritical]
    internal static unsafe long SetFilePointer(SafeFileHandle handle, long offset, SeekOrigin origin, out int hr)
    {
      hr = 0;
      int lo = (int) offset;
      int num1 = (int) (offset >> 32);
      int num2 = UnsafeNativeMethods.SetFilePointerWin32(handle, lo, &num1, (int) origin);
      if (num2 == -1 && (hr = Marshal.GetLastWin32Error()) != 0)
        return -1L;
      else
        return (long) (uint) num1 << 32 | (long) (uint) num2;
    }

    internal static int MakeHRFromErrorCode(int errorCode)
    {
      return -2147024896 | errorCode;
    }

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, BestFitMapping = false)]
    internal static int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

    [SecurityCritical]
    internal static string GetMessage(int errorCode)
    {
      StringBuilder lpBuffer = new StringBuilder(512);
      if (UnsafeNativeMethods.FormatMessage(12800, UnsafeNativeMethods.NULL, errorCode, 0, lpBuffer, lpBuffer.Capacity, UnsafeNativeMethods.NULL) != 0)
        return ((object) lpBuffer).ToString();
      else
        return "UnknownError_Num " + (object) errorCode;
    }

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static SafeLibraryHandle LoadLibraryEx(string libFilename, IntPtr reserved, int flags);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool CloseHandle(IntPtr handle);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static IntPtr GetCurrentProcess();

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool DuplicateHandle(IntPtr hSourceProcessHandle, SafePipeHandle hSourceHandle, IntPtr hTargetProcessHandle, out SafePipeHandle lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

    [SecurityCritical]
    [DllImport("kernel32.dll")]
    internal static int GetFileType(SafePipeHandle handle);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool CreatePipe(out SafePipeHandle hReadPipe, out SafePipeHandle hWritePipe, UnsafeNativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes, int nSize);

    [SecurityCritical]
    [DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
    internal static SafePipeHandle CreateNamedPipeClient(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, UnsafeNativeMethods.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool ConnectNamedPipe(SafePipeHandle handle, NativeOverlapped* overlapped);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool ConnectNamedPipe(SafePipeHandle handle, IntPtr overlapped);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static bool WaitNamedPipe(string name, int timeout);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GetNamedPipeHandleState(SafePipeHandle hNamedPipe, out int lpState, IntPtr lpCurInstances, IntPtr lpMaxCollectionCount, IntPtr lpCollectDataTimeout, IntPtr lpUserName, int nMaxUserNameSize);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GetNamedPipeHandleState(SafePipeHandle hNamedPipe, IntPtr lpState, out int lpCurInstances, IntPtr lpMaxCollectionCount, IntPtr lpCollectDataTimeout, IntPtr lpUserName, int nMaxUserNameSize);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GetNamedPipeHandleState(SafePipeHandle hNamedPipe, IntPtr lpState, IntPtr lpCurInstances, IntPtr lpMaxCollectionCount, IntPtr lpCollectDataTimeout, StringBuilder lpUserName, int nMaxUserNameSize);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GetNamedPipeInfo(SafePipeHandle hNamedPipe, out int lpFlags, IntPtr lpOutBufferSize, IntPtr lpInBufferSize, IntPtr lpMaxInstances);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GetNamedPipeInfo(SafePipeHandle hNamedPipe, IntPtr lpFlags, out int lpOutBufferSize, IntPtr lpInBufferSize, IntPtr lpMaxInstances);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GetNamedPipeInfo(SafePipeHandle hNamedPipe, IntPtr lpFlags, IntPtr lpOutBufferSize, out int lpInBufferSize, IntPtr lpMaxInstances);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool SetNamedPipeHandleState(SafePipeHandle hNamedPipe, int* lpMode, IntPtr lpMaxCollectionCount, IntPtr lpCollectDataTimeout);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool DisconnectNamedPipe(SafePipeHandle hNamedPipe);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool FlushFileBuffers(SafePipeHandle hNamedPipe);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
    internal static SafePipeHandle CreateNamedPipe(string pipeName, int openMode, int pipeMode, int maxInstances, int outBufferSize, int inBufferSize, int defaultTimeout, UnsafeNativeMethods.SECURITY_ATTRIBUTES securityAttributes);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static int ReadFile(SafePipeHandle handle, byte* bytes, int numBytesToRead, IntPtr numBytesRead_mustBeZero, NativeOverlapped* overlapped);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static int ReadFile(SafePipeHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr mustBeZero);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static int WriteFile(SafePipeHandle handle, byte* bytes, int numBytesToWrite, IntPtr numBytesWritten_mustBeZero, NativeOverlapped* lpOverlapped);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static int WriteFile(SafePipeHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static bool SetEndOfFile(IntPtr hNamedPipe);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint EventRegister([In] ref Guid providerId, [In] UnsafeNativeMethods.EtwEnableCallback enableCallback, [In] void* callbackContext, [In, Out] ref long registrationHandle);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static int EventUnregister([In] long registrationHandle);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static int EventEnabled([In] long registrationHandle, [In] ref EventDescriptor eventDescriptor);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static int EventProviderEnabled([In] long registrationHandle, [In] byte level, [In] long keywords);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint EventWrite([In] long registrationHandle, [In] ref EventDescriptor eventDescriptor, [In] uint userDataCount, [In] void* userData);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint EventWrite([In] long registrationHandle, [In] EventDescriptor* eventDescriptor, [In] uint userDataCount, [In] void* userData);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint EventWriteTransfer([In] long registrationHandle, [In] ref EventDescriptor eventDescriptor, [In] Guid* activityId, [In] Guid* relatedActivityId, [In] uint userDataCount, [In] void* userData);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint EventWriteString([In] long registrationHandle, [In] byte level, [In] long keywords, [In] char* message);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint EventActivityIdControl([In] int ControlCode, [In, Out] ref Guid ActivityId);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint PerfStartProvider([In] ref Guid ProviderGuid, [In] UnsafeNativeMethods.PERFLIBREQUEST ControlCallback, out SafePerfProviderHandle phProvider);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint PerfStopProvider([In] IntPtr hProvider);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint PerfSetCounterSetInfo([In] SafePerfProviderHandle hProvider, [In, Out] UnsafeNativeMethods.PerfCounterSetInfoStruct* pTemplate, [In] uint dwTemplateSize);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static UnsafeNativeMethods.PerfCounterSetInstanceStruct* PerfCreateInstance([In] SafePerfProviderHandle hProvider, [In] ref Guid CounterSetGuid, [In] string szInstanceName, [In] uint dwInstance);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint PerfDeleteInstance([In] SafePerfProviderHandle hProvider, [In] UnsafeNativeMethods.PerfCounterSetInstanceStruct* InstanceBlock);

    [SecurityCritical]
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    internal static uint PerfSetCounterRefValue([In] SafePerfProviderHandle hProvider, [In] UnsafeNativeMethods.PerfCounterSetInstanceStruct* pInstance, [In] uint CounterId, [In] void* lpAddr);

    [SecurityCritical]
    [DllImport("wevtapi.dll", SetLastError = true)]
    internal static EventLogHandle EvtQuery(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string query, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtSeek(EventLogHandle resultSet, long position, EventLogHandle bookmark, int timeout, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtSeekFlags flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", SetLastError = true)]
    internal static EventLogHandle EvtSubscribe(EventLogHandle session, SafeWaitHandle signalEvent, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string query, EventLogHandle bookmark, IntPtr context, IntPtr callback, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtNext(EventLogHandle queryHandle, int eventSize, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] events, int timeout, int flags, ref int returned);

    [SecurityCritical]
    [DllImport("wevtapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtCancel(EventLogHandle handle);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetEventInfo(EventLogHandle eventHandle, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtEventPropertyId propertyId, int bufferSize, IntPtr bufferPtr, out int bufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetQueryInfo(EventLogHandle queryHandle, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtQueryPropertyId propertyId, int bufferSize, IntPtr buffer, ref int bufferRequired);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenPublisherMetadata(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string publisherId, [MarshalAs(UnmanagedType.LPWStr)] string logFilePath, int locale, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetPublisherMetadataProperty(EventLogHandle publisherMetadataHandle, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtPublisherMetadataPropertyId propertyId, int flags, int publisherMetadataPropertyBufferSize, IntPtr publisherMetadataPropertyBuffer, out int publisherMetadataPropertyBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetObjectArraySize(EventLogHandle objectArray, out int objectArraySize);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetObjectArrayProperty(EventLogHandle objectArray, int propertyId, int arrayIndex, int flags, int propertyValueBufferSize, IntPtr propertyValueBuffer, out int propertyValueBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenEventMetadataEnum(EventLogHandle publisherMetadata, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtNextEventMetadata(EventLogHandle eventMetadataEnum, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetEventMetadataProperty(EventLogHandle eventMetadata, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtEventMetadataPropertyId propertyId, int flags, int eventMetadataPropertyBufferSize, IntPtr eventMetadataPropertyBuffer, out int eventMetadataPropertyBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenChannelEnum(EventLogHandle session, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtNextChannelPath(EventLogHandle channelEnum, int channelPathBufferSize, [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder channelPathBuffer, out int channelPathBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenPublisherEnum(EventLogHandle session, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtNextPublisherId(EventLogHandle publisherEnum, int publisherIdBufferSize, [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder publisherIdBuffer, out int publisherIdBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenChannelConfig(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string channelPath, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtSaveChannelConfig(EventLogHandle channelConfig, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtSetChannelConfigProperty(EventLogHandle channelConfig, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtChannelConfigPropertyId propertyId, int flags, ref UnsafeNativeMethods.EvtVariant propertyValue);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetChannelConfigProperty(EventLogHandle channelConfig, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtChannelConfigPropertyId propertyId, int flags, int propertyValueBufferSize, IntPtr propertyValueBuffer, out int propertyValueBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenLog(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.I4)] PathType flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtGetLogInfo(EventLogHandle log, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtLogPropertyId propertyId, int propertyValueBufferSize, IntPtr propertyValueBuffer, out int propertyValueBufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtExportLog(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string channelPath, [MarshalAs(UnmanagedType.LPWStr)] string query, [MarshalAs(UnmanagedType.LPWStr)] string targetFilePath, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtArchiveExportedLog(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string logFilePath, int locale, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtClearLog(EventLogHandle session, [MarshalAs(UnmanagedType.LPWStr)] string channelPath, [MarshalAs(UnmanagedType.LPWStr)] string targetFilePath, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtCreateRenderContext(int valuePathsCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] valuePaths, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtRenderContextFlags flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtRender(EventLogHandle context, EventLogHandle eventHandle, UnsafeNativeMethods.EvtRenderFlags flags, int buffSize, [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder buffer, out int buffUsed, out int propCount);

    [SecurityCritical]
    [DllImport("wevtapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtRender(EventLogHandle context, EventLogHandle eventHandle, UnsafeNativeMethods.EvtRenderFlags flags, int buffSize, IntPtr buffer, out int buffUsed, out int propCount);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtFormatMessage(EventLogHandle publisherMetadataHandle, EventLogHandle eventHandle, uint messageId, int valueCount, UnsafeNativeMethods.EvtStringVariant[] values, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtFormatMessageFlags flags, int bufferSize, [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder buffer, out int bufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", EntryPoint = "EvtFormatMessage", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtFormatMessageBuffer(EventLogHandle publisherMetadataHandle, EventLogHandle eventHandle, uint messageId, int valueCount, IntPtr values, [MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtFormatMessageFlags flags, int bufferSize, IntPtr buffer, out int bufferUsed);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtOpenSession([MarshalAs(UnmanagedType.I4)] UnsafeNativeMethods.EvtLoginClass loginClass, ref UnsafeNativeMethods.EvtRpcLogin login, int timeout, int flags);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static EventLogHandle EvtCreateBookmark([MarshalAs(UnmanagedType.LPWStr)] string bookmarkXml);

    [SecurityCritical]
    [DllImport("wevtapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool EvtUpdateBookmark(EventLogHandle bookmark, EventLogHandle eventHandle);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static void GetSystemInfo(ref UnsafeNativeMethods.SYSTEM_INFO lpSystemInfo);

    [SecurityCritical]
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static int GetFileSize(SafeMemoryMappedFileHandle hFile, out int highSize);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static IntPtr VirtualQuery(SafeMemoryMappedViewHandle address, ref UnsafeNativeMethods.MEMORY_BASIC_INFORMATION buffer, IntPtr sizeOfBuffer);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
    internal static SafeMemoryMappedFileHandle CreateFileMapping(SafeFileHandle hFile, UnsafeNativeMethods.SECURITY_ATTRIBUTES lpAttributes, int fProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, string lpName);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool FlushViewOfFile(byte* lpBaseAddress, IntPtr dwNumberOfBytesToFlush);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
    internal static SafeMemoryMappedFileHandle OpenFileMapping(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static SafeMemoryMappedViewHandle MapViewOfFile(SafeMemoryMappedFileHandle handle, int dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

    [SecurityCritical]
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static IntPtr VirtualAlloc(SafeMemoryMappedViewHandle address, UIntPtr numBytes, int commitOrReserve, int pageProtectionMode);

    [SecurityCritical]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static bool GlobalMemoryStatusEx([In, Out] UnsafeNativeMethods.MEMORYSTATUSEX lpBuffer);

    [SecurityCritical(SecurityCriticalScope.Everything)]
    internal delegate void EtwEnableCallback([In] ref Guid sourceId, [In] int isEnabled, [In] byte level, [In] long matchAnyKeywords, [In] long matchAllKeywords, [In] void* filterData, [In] void* callbackContext);

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    internal struct PerfCounterSetInstanceStruct
    {
      [FieldOffset(0)]
      internal Guid CounterSetGuid;
      [FieldOffset(16)]
      internal uint dwSize;
      [FieldOffset(20)]
      internal uint InstanceId;
      [FieldOffset(24)]
      internal uint InstanceNameOffset;
      [FieldOffset(28)]
      internal uint InstanceNameSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class SECURITY_ATTRIBUTES
    {
      internal int nLength;
      [SecurityCritical]
      internal unsafe byte* pSecurityDescriptor;
      internal int bInheritHandle;
    }

    [StructLayout(LayoutKind.Explicit, Size = 40)]
    internal struct PerfCounterSetInfoStruct
    {
      [FieldOffset(0)]
      internal Guid CounterSetGuid;
      [FieldOffset(16)]
      internal Guid ProviderGuid;
      [FieldOffset(32)]
      internal uint NumCounters;
      [FieldOffset(36)]
      internal uint InstanceType;
    }

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    internal struct PerfCounterInfoStruct
    {
      [FieldOffset(0)]
      internal uint CounterId;
      [FieldOffset(4)]
      internal uint CounterType;
      [FieldOffset(8)]
      internal long Attrib;
      [FieldOffset(16)]
      internal uint Size;
      [FieldOffset(20)]
      internal uint DetailLevel;
      [FieldOffset(24)]
      internal uint Scale;
      [FieldOffset(28)]
      internal uint Offset;
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    internal delegate uint PERFLIBREQUEST([In] uint RequestCode, [In] void* Buffer, [In] uint BufferSize);

    [System.Flags]
    internal enum EvtQueryFlags
    {
      EvtQueryChannelPath = 1,
      EvtQueryFilePath = 2,
      EvtQueryForwardDirection = 256,
      EvtQueryReverseDirection = 512,
      EvtQueryTolerateQueryErrors = 4096,
    }

    [System.Flags]
    internal enum EvtSubscribeFlags
    {
      EvtSubscribeToFutureEvents = 1,
      EvtSubscribeStartAtOldestRecord = 2,
      EvtSubscribeStartAfterBookmark = EvtSubscribeStartAtOldestRecord | EvtSubscribeToFutureEvents,
      EvtSubscribeTolerateQueryErrors = 4096,
      EvtSubscribeStrict = 65536,
    }

    internal enum EvtVariantType
    {
      EvtVarTypeNull = 0,
      EvtVarTypeString = 1,
      EvtVarTypeAnsiString = 2,
      EvtVarTypeSByte = 3,
      EvtVarTypeByte = 4,
      EvtVarTypeInt16 = 5,
      EvtVarTypeUInt16 = 6,
      EvtVarTypeInt32 = 7,
      EvtVarTypeUInt32 = 8,
      EvtVarTypeInt64 = 9,
      EvtVarTypeUInt64 = 10,
      EvtVarTypeSingle = 11,
      EvtVarTypeDouble = 12,
      EvtVarTypeBoolean = 13,
      EvtVarTypeBinary = 14,
      EvtVarTypeGuid = 15,
      EvtVarTypeSizeT = 16,
      EvtVarTypeFileTime = 17,
      EvtVarTypeSysTime = 18,
      EvtVarTypeSid = 19,
      EvtVarTypeHexInt32 = 20,
      EvtVarTypeHexInt64 = 21,
      EvtVarTypeEvtHandle = 32,
      EvtVarTypeEvtXml = 35,
      EvtVarTypeStringArray = 129,
      EvtVarTypeUInt32Array = 136,
    }

    internal enum EvtMasks
    {
      EVT_VARIANT_TYPE_MASK = 127,
      EVT_VARIANT_TYPE_ARRAY = 128,
    }

    internal struct SystemTime
    {
      [MarshalAs(UnmanagedType.U2)]
      public short Year;
      [MarshalAs(UnmanagedType.U2)]
      public short Month;
      [MarshalAs(UnmanagedType.U2)]
      public short DayOfWeek;
      [MarshalAs(UnmanagedType.U2)]
      public short Day;
      [MarshalAs(UnmanagedType.U2)]
      public short Hour;
      [MarshalAs(UnmanagedType.U2)]
      public short Minute;
      [MarshalAs(UnmanagedType.U2)]
      public short Second;
      [MarshalAs(UnmanagedType.U2)]
      public short Milliseconds;
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
    internal struct EvtVariant
    {
      [FieldOffset(0)]
      public uint UInteger;
      [FieldOffset(0)]
      public int Integer;
      [FieldOffset(0)]
      public byte UInt8;
      [FieldOffset(0)]
      public short Short;
      [FieldOffset(0)]
      public ushort UShort;
      [FieldOffset(0)]
      public uint Bool;
      [FieldOffset(0)]
      public byte ByteVal;
      [FieldOffset(0)]
      public byte SByte;
      [FieldOffset(0)]
      public ulong ULong;
      [FieldOffset(0)]
      public long Long;
      [FieldOffset(0)]
      public float Single;
      [FieldOffset(0)]
      public double Double;
      [FieldOffset(0)]
      public IntPtr StringVal;
      [FieldOffset(0)]
      public IntPtr AnsiString;
      [FieldOffset(0)]
      public IntPtr SidVal;
      [FieldOffset(0)]
      public IntPtr Binary;
      [FieldOffset(0)]
      public IntPtr Reference;
      [FieldOffset(0)]
      public IntPtr Handle;
      [FieldOffset(0)]
      public IntPtr GuidReference;
      [FieldOffset(0)]
      public ulong FileTime;
      [FieldOffset(0)]
      public IntPtr SystemTime;
      [FieldOffset(0)]
      public IntPtr SizeT;
      [FieldOffset(8)]
      public uint Count;
      [FieldOffset(12)]
      public uint Type;
    }

    internal enum EvtEventPropertyId
    {
      EvtEventQueryIDs,
      EvtEventPath,
    }

    internal enum EvtQueryPropertyId
    {
      EvtQueryNames,
      EvtQueryStatuses,
    }

    internal enum EvtPublisherMetadataPropertyId
    {
      EvtPublisherMetadataPublisherGuid,
      EvtPublisherMetadataResourceFilePath,
      EvtPublisherMetadataParameterFilePath,
      EvtPublisherMetadataMessageFilePath,
      EvtPublisherMetadataHelpLink,
      EvtPublisherMetadataPublisherMessageID,
      EvtPublisherMetadataChannelReferences,
      EvtPublisherMetadataChannelReferencePath,
      EvtPublisherMetadataChannelReferenceIndex,
      EvtPublisherMetadataChannelReferenceID,
      EvtPublisherMetadataChannelReferenceFlags,
      EvtPublisherMetadataChannelReferenceMessageID,
      EvtPublisherMetadataLevels,
      EvtPublisherMetadataLevelName,
      EvtPublisherMetadataLevelValue,
      EvtPublisherMetadataLevelMessageID,
      EvtPublisherMetadataTasks,
      EvtPublisherMetadataTaskName,
      EvtPublisherMetadataTaskEventGuid,
      EvtPublisherMetadataTaskValue,
      EvtPublisherMetadataTaskMessageID,
      EvtPublisherMetadataOpcodes,
      EvtPublisherMetadataOpcodeName,
      EvtPublisherMetadataOpcodeValue,
      EvtPublisherMetadataOpcodeMessageID,
      EvtPublisherMetadataKeywords,
      EvtPublisherMetadataKeywordName,
      EvtPublisherMetadataKeywordValue,
      EvtPublisherMetadataKeywordMessageID,
    }

    internal enum EvtChannelReferenceFlags
    {
      EvtChannelReferenceImported = 1,
    }

    internal enum EvtEventMetadataPropertyId
    {
      EventMetadataEventID,
      EventMetadataEventVersion,
      EventMetadataEventChannel,
      EventMetadataEventLevel,
      EventMetadataEventOpcode,
      EventMetadataEventTask,
      EventMetadataEventKeyword,
      EventMetadataEventMessageID,
      EventMetadataEventTemplate,
    }

    internal enum EvtChannelConfigPropertyId
    {
      EvtChannelConfigEnabled,
      EvtChannelConfigIsolation,
      EvtChannelConfigType,
      EvtChannelConfigOwningPublisher,
      EvtChannelConfigClassicEventlog,
      EvtChannelConfigAccess,
      EvtChannelLoggingConfigRetention,
      EvtChannelLoggingConfigAutoBackup,
      EvtChannelLoggingConfigMaxSize,
      EvtChannelLoggingConfigLogFilePath,
      EvtChannelPublishingConfigLevel,
      EvtChannelPublishingConfigKeywords,
      EvtChannelPublishingConfigControlGuid,
      EvtChannelPublishingConfigBufferSize,
      EvtChannelPublishingConfigMinBuffers,
      EvtChannelPublishingConfigMaxBuffers,
      EvtChannelPublishingConfigLatency,
      EvtChannelPublishingConfigClockType,
      EvtChannelPublishingConfigSidType,
      EvtChannelPublisherList,
      EvtChannelConfigPropertyIdEND,
    }

    internal enum EvtLogPropertyId
    {
      EvtLogCreationTime,
      EvtLogLastAccessTime,
      EvtLogLastWriteTime,
      EvtLogFileSize,
      EvtLogAttributes,
      EvtLogNumberOfLogRecords,
      EvtLogOldestRecordNumber,
      EvtLogFull,
    }

    internal enum EvtExportLogFlags
    {
      EvtExportLogChannelPath = 1,
      EvtExportLogFilePath = 2,
      EvtExportLogTolerateQueryErrors = 4096,
    }

    internal enum EvtRenderContextFlags
    {
      EvtRenderContextValues,
      EvtRenderContextSystem,
      EvtRenderContextUser,
    }

    internal enum EvtRenderFlags
    {
      EvtRenderEventValues,
      EvtRenderEventXml,
      EvtRenderBookmark,
    }

    internal enum EvtFormatMessageFlags
    {
      EvtFormatMessageEvent = 1,
      EvtFormatMessageLevel = 2,
      EvtFormatMessageTask = 3,
      EvtFormatMessageOpcode = 4,
      EvtFormatMessageKeyword = 5,
      EvtFormatMessageChannel = 6,
      EvtFormatMessageProvider = 7,
      EvtFormatMessageId = 8,
      EvtFormatMessageXml = 9,
    }

    internal enum EvtSystemPropertyId
    {
      EvtSystemProviderName,
      EvtSystemProviderGuid,
      EvtSystemEventID,
      EvtSystemQualifiers,
      EvtSystemLevel,
      EvtSystemTask,
      EvtSystemOpcode,
      EvtSystemKeywords,
      EvtSystemTimeCreated,
      EvtSystemEventRecordId,
      EvtSystemActivityID,
      EvtSystemRelatedActivityID,
      EvtSystemProcessID,
      EvtSystemThreadID,
      EvtSystemChannel,
      EvtSystemComputer,
      EvtSystemUserID,
      EvtSystemVersion,
      EvtSystemPropertyIdEND,
    }

    internal enum EvtLoginClass
    {
      EvtRpcLogin = 1,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct EvtRpcLogin
    {
      [MarshalAs(UnmanagedType.LPWStr)]
      public string Server;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string User;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string Domain;
      [SecurityCritical]
      public CoTaskMemUnicodeSafeHandle Password;
      public int Flags;
    }

    [System.Flags]
    internal enum EvtSeekFlags
    {
      EvtSeekRelativeToFirst = 1,
      EvtSeekRelativeToLast = 2,
      EvtSeekRelativeToCurrent = EvtSeekRelativeToLast | EvtSeekRelativeToFirst,
      EvtSeekRelativeToBookmark = 4,
      EvtSeekOriginMask = EvtSeekRelativeToBookmark | EvtSeekRelativeToCurrent,
      EvtSeekStrict = 65536,
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
    internal struct EvtStringVariant
    {
      [MarshalAs(UnmanagedType.LPWStr)]
      [FieldOffset(0)]
      public string StringVal;
      [FieldOffset(8)]
      public uint Count;
      [FieldOffset(12)]
      public uint Type;
    }

    [SecurityCritical(SecurityCriticalScope.Everything)]
    internal struct MEMORY_BASIC_INFORMATION
    {
      internal unsafe void* BaseAddress;
      internal unsafe void* AllocationBase;
      internal uint AllocationProtect;
      internal UIntPtr RegionSize;
      internal uint State;
      internal uint Protect;
      internal uint Type;
    }

    internal struct SYSTEM_INFO
    {
      internal int dwOemId;
      internal int dwPageSize;
      internal IntPtr lpMinimumApplicationAddress;
      internal IntPtr lpMaximumApplicationAddress;
      internal IntPtr dwActiveProcessorMask;
      internal int dwNumberOfProcessors;
      internal int dwProcessorType;
      internal int dwAllocationGranularity;
      internal short wProcessorLevel;
      internal short wProcessorRevision;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class MEMORYSTATUSEX
    {
      internal uint dwLength;
      internal uint dwMemoryLoad;
      internal ulong ullTotalPhys;
      internal ulong ullAvailPhys;
      internal ulong ullTotalPageFile;
      internal ulong ullAvailPageFile;
      internal ulong ullTotalVirtual;
      internal ulong ullAvailVirtual;
      internal ulong ullAvailExtendedVirtual;

      [SecurityCritical]
      internal MEMORYSTATUSEX()
      {
        this.dwLength = (uint) Marshal.SizeOf(typeof (UnsafeNativeMethods.MEMORYSTATUSEX));
      }
    }
  }
}
