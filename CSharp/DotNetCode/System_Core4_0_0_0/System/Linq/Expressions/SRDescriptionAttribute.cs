// Type: System.Linq.Expressions.SRDescriptionAttribute
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.ComponentModel;

namespace System.Linq.Expressions
{
  [AttributeUsage(AttributeTargets.All)]
  internal sealed class SRDescriptionAttribute : DescriptionAttribute
  {
    private bool replaced;

    public override string Description
    {
      get
      {
        if (!this.replaced)
        {
          this.replaced = true;
          this.DescriptionValue = SR.GetString(base.Description);
        }
        return base.Description;
      }
    }

    public SRDescriptionAttribute(string description)
      : base(description)
    {
    }
  }
}
