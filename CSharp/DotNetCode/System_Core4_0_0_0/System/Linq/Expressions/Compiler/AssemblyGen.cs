// Type: System.Linq.Expressions.Compiler.AssemblyGen
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class AssemblyGen
  {
    private static AssemblyGen _assembly;
    private readonly AssemblyBuilder _myAssembly;
    private readonly ModuleBuilder _myModule;
    private int _index;

    private static AssemblyGen Assembly
    {
      get
      {
        if (AssemblyGen._assembly == null)
          Interlocked.CompareExchange<AssemblyGen>(ref AssemblyGen._assembly, new AssemblyGen(), (AssemblyGen) null);
        return AssemblyGen._assembly;
      }
    }

    private AssemblyGen()
    {
      AssemblyName name = new AssemblyName("Snippets");
      CustomAttributeBuilder[] attributeBuilderArray = new CustomAttributeBuilder[1]
      {
        new CustomAttributeBuilder(typeof (SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), new object[0])
      };
      this._myAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run, (IEnumerable<CustomAttributeBuilder>) attributeBuilderArray);
      this._myModule = this._myAssembly.DefineDynamicModule(name.Name, false);
      this._myAssembly.DefineVersionInfoResource();
    }

    private TypeBuilder DefineType(string name, Type parent, TypeAttributes attr)
    {
      ContractUtils.RequiresNotNull((object) name, "name");
      ContractUtils.RequiresNotNull((object) parent, "parent");
      StringBuilder stringBuilder = new StringBuilder(name);
      int num = Interlocked.Increment(ref this._index);
      stringBuilder.Append("$");
      stringBuilder.Append(num);
      stringBuilder.Replace('+', '_').Replace('[', '_').Replace(']', '_').Replace('*', '_').Replace('&', '_').Replace(',', '_').Replace('\\', '_');
      name = ((object) stringBuilder).ToString();
      return this._myModule.DefineType(name, attr, parent);
    }

    internal static TypeBuilder DefineDelegateType(string name)
    {
      return AssemblyGen.Assembly.DefineType(name, typeof (MulticastDelegate), TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass);
    }
  }
}
