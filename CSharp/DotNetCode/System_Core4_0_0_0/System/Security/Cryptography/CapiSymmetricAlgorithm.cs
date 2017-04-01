// Type: System.Security.Cryptography.CapiSymmetricAlgorithm
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Security.Cryptography
{
  internal sealed class CapiSymmetricAlgorithm : ICryptoTransform, IDisposable
  {
    private int m_blockSize;
    private byte[] m_depadBuffer;
    private EncryptionMode m_encryptionMode;
    [SecurityCritical]
    private SafeCapiKeyHandle m_key;
    private PaddingMode m_paddingMode;
    [SecurityCritical]
    private SafeCspHandle m_provider;

    public bool CanReuseTransform
    {
      get
      {
        return true;
      }
    }

    public bool CanTransformMultipleBlocks
    {
      get
      {
        return true;
      }
    }

    public int InputBlockSize
    {
      get
      {
        return this.m_blockSize / 8;
      }
    }

    public int OutputBlockSize
    {
      get
      {
        return this.m_blockSize / 8;
      }
    }

    [SecurityCritical]
    public CapiSymmetricAlgorithm(int blockSize, int feedbackSize, SafeCspHandle provider, SafeCapiKeyHandle key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode, EncryptionMode encryptionMode)
    {
      this.m_blockSize = blockSize;
      this.m_encryptionMode = encryptionMode;
      this.m_paddingMode = paddingMode;
      this.m_provider = provider.Duplicate();
      this.m_key = CapiSymmetricAlgorithm.SetupKey(key, CapiSymmetricAlgorithm.ProcessIV(iv, blockSize, cipherMode), cipherMode, feedbackSize);
    }

    [SecuritySafeCritical]
    public void Dispose()
    {
      if (this.m_key != null)
        this.m_key.Dispose();
      if (this.m_provider != null)
        this.m_provider.Dispose();
      if (this.m_depadBuffer == null)
        return;
      Array.Clear((Array) this.m_depadBuffer, 0, this.m_depadBuffer.Length);
    }

    [SecuritySafeCritical]
    private int DecryptBlocks(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      int num = 0;
      if (this.m_paddingMode != PaddingMode.None && this.m_paddingMode != PaddingMode.Zeros)
      {
        if (this.m_depadBuffer != null)
        {
          int count = this.RawDecryptBlocks(this.m_depadBuffer, 0, this.m_depadBuffer.Length);
          Buffer.BlockCopy((Array) this.m_depadBuffer, 0, (Array) outputBuffer, outputOffset, count);
          Array.Clear((Array) this.m_depadBuffer, 0, this.m_depadBuffer.Length);
          outputOffset += count;
          num += count;
        }
        else
          this.m_depadBuffer = new byte[this.InputBlockSize];
        Buffer.BlockCopy((Array) inputBuffer, inputOffset + inputCount - this.m_depadBuffer.Length, (Array) this.m_depadBuffer, 0, this.m_depadBuffer.Length);
        inputCount -= this.m_depadBuffer.Length;
      }
      if (inputCount > 0)
      {
        Buffer.BlockCopy((Array) inputBuffer, inputOffset, (Array) outputBuffer, outputOffset, inputCount);
        num += this.RawDecryptBlocks(outputBuffer, outputOffset, inputCount);
      }
      return num;
    }

    private byte[] DepadBlock(byte[] block, int offset, int count)
    {
      int num;
      switch (this.m_paddingMode)
      {
        case PaddingMode.None:
        case PaddingMode.Zeros:
          num = 0;
          break;
        case PaddingMode.PKCS7:
          num = (int) block[offset + count - 1];
          if (num <= 0 || num > this.InputBlockSize)
            throw new CryptographicException(SR.GetString("Cryptography_InvalidPadding"));
          for (int index = offset + count - num; index < offset + count; ++index)
          {
            if ((int) block[index] != num)
              throw new CryptographicException(SR.GetString("Cryptography_InvalidPadding"));
          }
          break;
        case PaddingMode.ANSIX923:
          num = (int) block[offset + count - 1];
          if (num <= 0 || num > this.InputBlockSize)
            throw new CryptographicException(SR.GetString("Cryptography_InvalidPadding"));
          for (int index = offset + count - num; index < offset + count - 1; ++index)
          {
            if ((int) block[index] != 0)
              throw new CryptographicException(SR.GetString("Cryptography_InvalidPadding"));
          }
          break;
        case PaddingMode.ISO10126:
          num = (int) block[offset + count - 1];
          if (num <= 0 || num > this.InputBlockSize)
            throw new CryptographicException(SR.GetString("Cryptography_InvalidPadding"));
          else
            break;
        default:
          throw new CryptographicException(SR.GetString("Cryptography_UnknownPaddingMode"));
      }
      byte[] numArray = new byte[count - num];
      Buffer.BlockCopy((Array) block, offset, (Array) numArray, 0, numArray.Length);
      return numArray;
    }

    [SecurityCritical]
    private unsafe int EncryptBlocks(byte[] buffer, int offset, int count)
    {
      int pdwDataLen = count;
      fixed (byte* numPtr = &buffer[offset])
      {
        if (!CapiNative.UnsafeNativeMethods.CryptEncrypt(this.m_key, SafeCapiHashHandle.InvalidHandle, false, 0, new IntPtr((void*) numPtr), out pdwDataLen, buffer.Length - offset))
          throw new CryptographicException(Marshal.GetLastWin32Error());
      }
      return pdwDataLen;
    }

    [SecuritySafeCritical]
    private byte[] PadBlock(byte[] block, int offset, int count)
    {
      int num = this.InputBlockSize - count % this.InputBlockSize;
      byte[] pbBuffer;
      switch (this.m_paddingMode)
      {
        case PaddingMode.None:
          if (count % this.InputBlockSize != 0)
            throw new CryptographicException(SR.GetString("Cryptography_PartialBlock"));
          pbBuffer = new byte[count];
          Buffer.BlockCopy((Array) block, offset, (Array) pbBuffer, 0, pbBuffer.Length);
          break;
        case PaddingMode.PKCS7:
          pbBuffer = new byte[count + num];
          Buffer.BlockCopy((Array) block, offset, (Array) pbBuffer, 0, count);
          for (int index = count; index < pbBuffer.Length; ++index)
            pbBuffer[index] = (byte) num;
          break;
        case PaddingMode.Zeros:
          if (num == this.InputBlockSize)
            num = 0;
          pbBuffer = new byte[count + num];
          Buffer.BlockCopy((Array) block, offset, (Array) pbBuffer, 0, count);
          break;
        case PaddingMode.ANSIX923:
          pbBuffer = new byte[count + num];
          Buffer.BlockCopy((Array) block, 0, (Array) pbBuffer, 0, count);
          pbBuffer[pbBuffer.Length - 1] = (byte) num;
          break;
        case PaddingMode.ISO10126:
          pbBuffer = new byte[count + num];
          CapiNative.UnsafeNativeMethods.CryptGenRandom(this.m_provider, pbBuffer.Length - 1, pbBuffer);
          Buffer.BlockCopy((Array) block, 0, (Array) pbBuffer, 0, count);
          pbBuffer[pbBuffer.Length - 1] = (byte) num;
          break;
        default:
          throw new CryptographicException(SR.GetString("Cryptography_UnknownPaddingMode"));
      }
      return pbBuffer;
    }

    private static byte[] ProcessIV(byte[] iv, int blockSize, CipherMode cipherMode)
    {
      byte[] numArray = (byte[]) null;
      if (iv != null)
      {
        if (blockSize / 8 > iv.Length)
          throw new CryptographicException(SR.GetString("Cryptography_InvalidIVSize"));
        numArray = new byte[blockSize / 8];
        Buffer.BlockCopy((Array) iv, 0, (Array) numArray, 0, numArray.Length);
      }
      else if (cipherMode != CipherMode.ECB)
        throw new CryptographicException(SR.GetString("Cryptography_MissingIV"));
      return numArray;
    }

    [SecurityCritical]
    private unsafe int RawDecryptBlocks(byte[] buffer, int offset, int count)
    {
      int pdwDataLen = count;
      fixed (byte* numPtr = &buffer[offset])
      {
        if (!CapiNative.UnsafeNativeMethods.CryptDecrypt(this.m_key, SafeCapiHashHandle.InvalidHandle, false, 0, new IntPtr((void*) numPtr), out pdwDataLen))
          throw new CryptographicException(Marshal.GetLastWin32Error());
      }
      return pdwDataLen;
    }

    [SecuritySafeCritical]
    private unsafe void Reset()
    {
      byte[] numArray = new byte[this.OutputBlockSize];
      int pdwDataLen = 0;
      fixed (byte* numPtr = numArray)
      {
        if (this.m_encryptionMode == EncryptionMode.Encrypt)
          CapiNative.UnsafeNativeMethods.CryptEncrypt(this.m_key, SafeCapiHashHandle.InvalidHandle, true, 0, new IntPtr((void*) numPtr), out pdwDataLen, numArray.Length);
        else
          CapiNative.UnsafeNativeMethods.CryptDecrypt(this.m_key, SafeCapiHashHandle.InvalidHandle, true, 0, new IntPtr((void*) numPtr), out pdwDataLen);
      }
      if (this.m_depadBuffer == null)
        return;
      Array.Clear((Array) this.m_depadBuffer, 0, this.m_depadBuffer.Length);
      this.m_depadBuffer = (byte[]) null;
    }

    [SecuritySafeCritical]
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw new ArgumentOutOfRangeException("inputOffset");
      if (inputCount <= 0)
        throw new ArgumentOutOfRangeException("inputCount");
      if (inputCount % this.InputBlockSize != 0)
        throw new ArgumentOutOfRangeException("inputCount", SR.GetString("Cryptography_MustTransformWholeBlock"));
      if (inputCount > inputBuffer.Length - inputOffset)
        throw new ArgumentOutOfRangeException("inputCount", SR.GetString("Cryptography_TransformBeyondEndOfBuffer"));
      if (outputBuffer == null)
        throw new ArgumentNullException("outputBuffer");
      if (inputCount > outputBuffer.Length - outputOffset)
        throw new ArgumentOutOfRangeException("outputOffset", SR.GetString("Cryptography_TransformBeyondEndOfBuffer"));
      if (this.m_encryptionMode != EncryptionMode.Encrypt)
        return this.DecryptBlocks(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
      Buffer.BlockCopy((Array) inputBuffer, inputOffset, (Array) outputBuffer, outputOffset, inputCount);
      return this.EncryptBlocks(outputBuffer, outputOffset, inputCount);
    }

    [SecuritySafeCritical]
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw new ArgumentOutOfRangeException("inputOffset");
      if (inputCount < 0)
        throw new ArgumentOutOfRangeException("inputCount");
      if (inputCount > inputBuffer.Length - inputOffset)
        throw new ArgumentOutOfRangeException("inputCount", SR.GetString("Cryptography_TransformBeyondEndOfBuffer"));
      byte[] buffer;
      if (this.m_encryptionMode == EncryptionMode.Encrypt)
      {
        buffer = this.PadBlock(inputBuffer, inputOffset, inputCount);
        if (buffer.Length > 0)
          this.EncryptBlocks(buffer, 0, buffer.Length);
      }
      else
      {
        if (inputCount % this.InputBlockSize != 0)
          throw new CryptographicException(SR.GetString("Cryptography_PartialBlock"));
        byte[] numArray;
        if (this.m_depadBuffer == null)
        {
          numArray = new byte[inputCount];
          Buffer.BlockCopy((Array) inputBuffer, inputOffset, (Array) numArray, 0, inputCount);
        }
        else
        {
          numArray = new byte[this.m_depadBuffer.Length + inputCount];
          Buffer.BlockCopy((Array) this.m_depadBuffer, 0, (Array) numArray, 0, this.m_depadBuffer.Length);
          Buffer.BlockCopy((Array) inputBuffer, inputOffset, (Array) numArray, this.m_depadBuffer.Length, inputCount);
        }
        if (numArray.Length > 0)
        {
          int count = this.RawDecryptBlocks(numArray, 0, numArray.Length);
          buffer = this.DepadBlock(numArray, 0, count);
        }
        else
          buffer = new byte[0];
      }
      this.Reset();
      return buffer;
    }

    [SecurityCritical]
    private static SafeCapiKeyHandle SetupKey(SafeCapiKeyHandle key, byte[] iv, CipherMode cipherMode, int feedbackSize)
    {
      SafeCapiKeyHandle key1 = key.Duplicate();
      CapiNative.SetKeyParameter(key1, CapiNative.KeyParameter.Mode, (int) cipherMode);
      if (cipherMode != CipherMode.ECB)
        CapiNative.SetKeyParameter(key1, CapiNative.KeyParameter.IV, iv);
      if (cipherMode == CipherMode.CFB || cipherMode == CipherMode.OFB)
        CapiNative.SetKeyParameter(key1, CapiNative.KeyParameter.ModeBits, feedbackSize);
      return key1;
    }
  }
}
