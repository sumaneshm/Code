// Type: System.IO.BufferedStream2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
  [HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
  internal abstract class BufferedStream2 : Stream
  {
    protected internal const int DefaultBufferSize = 32768;
    protected int bufferSize;
    private byte[] _buffer;
    private int _pendingBufferCopy;
    private int _writePos;
    private int _readPos;
    private int _readLen;
    protected long pos;

    protected long UnderlyingStreamPosition
    {
      get
      {
        return this.pos;
      }
      set
      {
        Interlocked.Exchange(ref this.pos, value);
      }
    }

    public override void Write(byte[] array, int offset, int count)
    {
      if (array == null)
        throw new ArgumentNullException("array", SR.GetString("ArgumentNull_Buffer"));
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (array.Length - offset < count)
        throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
      if (this._writePos == 0)
      {
        if (!this.CanWrite)
          __Error.WriteNotSupported();
        if (this._readPos < this._readLen)
          this.FlushRead();
        this._readPos = 0;
        this._readLen = 0;
      }
      if (count == 0)
        return;
      while (true)
      {
        while (this._writePos <= this.bufferSize)
        {
          if (this._writePos == 0 && count >= this.bufferSize)
          {
            this.WriteCore(array, offset, count, true);
            return;
          }
          else
          {
            Thread.BeginCriticalRegion();
            Interlocked.Increment(ref this._pendingBufferCopy);
            int num1 = Interlocked.Add(ref this._writePos, count);
            int num2 = num1 - count;
            if (num1 > this.bufferSize)
            {
              Interlocked.Decrement(ref this._pendingBufferCopy);
              Thread.EndCriticalRegion();
              if (this._writePos > this.bufferSize && num2 <= this.bufferSize && num2 > 0)
              {
                while (this._pendingBufferCopy != 0)
                  Thread.SpinWait(1);
                this.WriteCore(this._buffer, 0, num2, true);
                this._writePos = 0;
              }
            }
            else
            {
              if (this._buffer == null)
                Interlocked.CompareExchange<byte[]>(ref this._buffer, new byte[this.bufferSize], (byte[]) null);
              Buffer.BlockCopy((Array) array, offset, (Array) this._buffer, num2, count);
              Interlocked.Decrement(ref this._pendingBufferCopy);
              Thread.EndCriticalRegion();
              return;
            }
          }
        }
        Thread.Sleep(1);
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Flush()
    {
      try
      {
        if (this._writePos > 0)
        {
          this.FlushWrite(false);
        }
        else
        {
          if (this._readPos >= this._readLen)
            return;
          this.FlushRead();
        }
      }
      finally
      {
        this._writePos = 0;
        this._readPos = 0;
        this._readLen = 0;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    protected void FlushRead()
    {
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    protected void FlushWrite(bool blockForWrite)
    {
      if (this._writePos > 0)
        this.WriteCore(this._buffer, 0, this._writePos, blockForWrite);
      this._writePos = 0;
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this._writePos <= 0)
          return;
        this.FlushWrite(disposing);
      }
      finally
      {
        this._readPos = 0;
        this._readLen = 0;
        this._writePos = 0;
        base.Dispose(disposing);
      }
    }

    protected long AddUnderlyingStreamPosition(long posDelta)
    {
      return Interlocked.Add(ref this.pos, posDelta);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    protected internal void DiscardBuffer()
    {
      this._readPos = 0;
      this._readLen = 0;
      this._writePos = 0;
    }

    private void WriteCore(byte[] buffer, int offset, int count, bool blockForWrite)
    {
      long streamPos;
      this.WriteCore(buffer, offset, count, blockForWrite, out streamPos);
    }

    protected abstract void WriteCore(byte[] buffer, int offset, int count, bool blockForWrite, out long streamPos);
  }
}
