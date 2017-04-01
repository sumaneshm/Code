// Type: System.IO.Pipes.PipeStreamAsyncResult
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Security;
using System.Threading;

namespace System.IO.Pipes
{
  internal sealed class PipeStreamAsyncResult : IAsyncResult
  {
    internal AsyncCallback _userCallback;
    internal object _userStateObject;
    internal ManualResetEvent _waitHandle;
    [SecurityCritical]
    internal SafePipeHandle _handle;
    [SecurityCritical]
    internal unsafe NativeOverlapped* _overlapped;
    internal int _EndXxxCalled;
    internal int _numBytes;
    internal int _errorCode;
    internal bool _isMessageComplete;
    internal bool _isWrite;
    internal bool _isComplete;
    internal bool _completedSynchronously;

    public object AsyncState
    {
      get
      {
        return this._userStateObject;
      }
    }

    public bool IsCompleted
    {
      get
      {
        return this._isComplete;
      }
    }

    public unsafe WaitHandle AsyncWaitHandle
    {
      [SecurityCritical] get
      {
        if (this._waitHandle == null)
        {
          ManualResetEvent manualResetEvent = new ManualResetEvent(false);
          if ((IntPtr) this._overlapped != IntPtr.Zero && this._overlapped->EventHandle != IntPtr.Zero)
            manualResetEvent.SafeWaitHandle = new SafeWaitHandle(this._overlapped->EventHandle, true);
          if (this._isComplete)
            manualResetEvent.Set();
          this._waitHandle = manualResetEvent;
        }
        return (WaitHandle) this._waitHandle;
      }
    }

    public bool CompletedSynchronously
    {
      get
      {
        return this._completedSynchronously;
      }
    }

    private void CallUserCallbackWorker(object callbackState)
    {
      this._isComplete = true;
      if (this._waitHandle != null)
        this._waitHandle.Set();
      this._userCallback((IAsyncResult) this);
    }

    internal void CallUserCallback()
    {
      if (this._userCallback != null)
      {
        this._completedSynchronously = false;
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.CallUserCallbackWorker));
      }
      else
      {
        this._isComplete = true;
        if (this._waitHandle == null)
          return;
        this._waitHandle.Set();
      }
    }
  }
}
