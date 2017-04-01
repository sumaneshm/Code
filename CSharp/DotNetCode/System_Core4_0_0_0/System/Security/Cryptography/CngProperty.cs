// Type: System.Security.Cryptography.CngProperty
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public struct CngProperty : IEquatable<CngProperty>
  {
    private string m_name;
    private CngPropertyOptions m_propertyOptions;
    private byte[] m_value;
    private int? m_hashCode;

    public string Name
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_name;
      }
    }

    public CngPropertyOptions Options
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_propertyOptions;
      }
    }

    internal byte[] Value
    {
      get
      {
        return this.m_value;
      }
    }

    public CngProperty(string name, byte[] value, CngPropertyOptions options)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      this.m_name = name;
      this.m_propertyOptions = options;
      this.m_hashCode = new int?();
      if (value != null)
        this.m_value = value.Clone() as byte[];
      else
        this.m_value = (byte[]) null;
    }

    public static bool operator ==(CngProperty left, CngProperty right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(CngProperty left, CngProperty right)
    {
      return !left.Equals(right);
    }

    public byte[] GetValue()
    {
      byte[] numArray = (byte[]) null;
      if (this.m_value != null)
        numArray = this.m_value.Clone() as byte[];
      return numArray;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is CngProperty))
        return false;
      else
        return this.Equals((CngProperty) obj);
    }

    public bool Equals(CngProperty other)
    {
      if (!string.Equals(this.Name, other.Name, StringComparison.Ordinal) || this.Options != other.Options)
        return false;
      if (this.m_value == null)
        return other.m_value == null;
      if (other.m_value == null || this.m_value.Length != other.m_value.Length)
        return false;
      for (int index = 0; index < this.m_value.Length; ++index)
      {
        if ((int) this.m_value[index] != (int) other.m_value[index])
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      if (!this.m_hashCode.HasValue)
      {
        int num1 = this.Name.GetHashCode() ^ this.Options.GetHashCode();
        if (this.m_value != null)
        {
          for (int index = 0; index < this.m_value.Length; ++index)
          {
            int num2 = (int) this.m_value[index] << index % 4 * 8;
            num1 ^= num2;
          }
        }
        this.m_hashCode = new int?(num1);
      }
      return this.m_hashCode.Value;
    }
  }
}
