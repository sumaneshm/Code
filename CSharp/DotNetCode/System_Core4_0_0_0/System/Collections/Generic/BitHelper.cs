// Type: System.Collections.Generic.BitHelper
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security;

namespace System.Collections.Generic
{
  internal class BitHelper
  {
    private int m_length;
    [SecurityCritical]
    private unsafe int* m_arrayPtr;
    private int[] m_array;
    private bool useStackAlloc;
    private const byte MarkedBitFlag = (byte) 1;
    private const byte IntSize = (byte) 32;

    [SecurityCritical]
    internal unsafe BitHelper(int* bitArrayPtr, int length)
    {
      this.m_arrayPtr = bitArrayPtr;
      this.m_length = length;
      this.useStackAlloc = true;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal BitHelper(int[] bitArray, int length)
    {
      this.m_array = bitArray;
      this.m_length = length;
    }

    [SecuritySafeCritical]
    internal unsafe void MarkBit(int bitPosition)
    {
      if (this.useStackAlloc)
      {
        int num1 = bitPosition / 32;
        if (num1 >= this.m_length || num1 < 0)
          return;
        IntPtr num2 = (IntPtr) (this.m_arrayPtr + num1);
        int num3 = *(int*) num2 | 1 << bitPosition % 32;
        *(int*) num2 = num3;
      }
      else
      {
        int index = bitPosition / 32;
        if (index >= this.m_length || index < 0)
          return;
        this.m_array[index] |= 1 << bitPosition % 32;
      }
    }

    [SecuritySafeCritical]
    internal unsafe bool IsMarked(int bitPosition)
    {
      if (this.useStackAlloc)
      {
        int index = bitPosition / 32;
        if (index < this.m_length && index >= 0)
          return (this.m_arrayPtr[index] & 1 << bitPosition % 32) != 0;
        else
          return false;
      }
      else
      {
        int index = bitPosition / 32;
        if (index < this.m_length && index >= 0)
          return (this.m_array[index] & 1 << bitPosition % 32) != 0;
        else
          return false;
      }
    }

    internal static int ToIntArrayLength(int n)
    {
      if (n <= 0)
        return 0;
      else
        return (n - 1) / 32 + 1;
    }
  }
}
