// Type: System.Dynamic.ExpandoObject
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public sealed class ExpandoObject : IDynamicMetaObjectProvider, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, INotifyPropertyChanged
  {
    internal static readonly object Uninitialized = new object();
    internal readonly object LockObject;
    private ExpandoObject.ExpandoData _data;
    private int _count;
    private PropertyChangedEventHandler _propertyChanged;
    internal const int AmbiguousMatchFound = -2;
    internal const int NoMatch = -1;

    internal ExpandoClass Class
    {
      get
      {
        return this._data.Class;
      }
    }

    [__DynamicallyInvokable]
    ICollection<string> IDictionary<string, object>.Keys
    {
      [__DynamicallyInvokable] get
      {
        return (ICollection<string>) new ExpandoObject.KeyCollection(this);
      }
    }

    [__DynamicallyInvokable]
    ICollection<object> IDictionary<string, object>.Values
    {
      [__DynamicallyInvokable] get
      {
        return (ICollection<object>) new ExpandoObject.ValueCollection(this);
      }
    }

    [__DynamicallyInvokable]
    int ICollection<KeyValuePair<string, object>>.Count
    {
      [__DynamicallyInvokable] get
      {
        return this._count;
      }
    }

    [__DynamicallyInvokable]
    bool ICollection<KeyValuePair<string, object>>.IsReadOnly
    {
      [__DynamicallyInvokable] get
      {
        return false;
      }
    }

    [__DynamicallyInvokable]
    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
      [__DynamicallyInvokable] add
      {
        this._propertyChanged += value;
      }
      [__DynamicallyInvokable] remove
      {
        this._propertyChanged -= value;
      }
    }

    static ExpandoObject()
    {
    }

    [__DynamicallyInvokable]
    public ExpandoObject()
    {
      this._data = ExpandoObject.ExpandoData.Empty;
      this.LockObject = new object();
    }

    internal bool TryGetValue(object indexClass, int index, string name, bool ignoreCase, out object value)
    {
      ExpandoObject.ExpandoData expandoData = this._data;
      if (expandoData.Class != indexClass || ignoreCase)
      {
        index = expandoData.Class.GetValueIndex(name, ignoreCase, this);
        if (index == -2)
          throw Error.AmbiguousMatchInExpandoObject((object) name);
      }
      if (index == -1)
      {
        value = (object) null;
        return false;
      }
      else
      {
        object obj = expandoData[index];
        if (obj == ExpandoObject.Uninitialized)
        {
          value = (object) null;
          return false;
        }
        else
        {
          value = obj;
          return true;
        }
      }
    }

    [__DynamicallyInvokable]
    DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
    {
      return (DynamicMetaObject) new ExpandoObject.MetaExpando(parameter, this);
    }

    private void TryAddMember(string key, object value)
    {
      ContractUtils.RequiresNotNull((object) key, "key");
      this.TrySetValue((object) null, -1, value, key, false, true);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private bool TryGetValueForKey(string key, out object value)
    {
      return this.TryGetValue((object) null, -1, key, false, out value);
    }

    [__DynamicallyInvokable]
    object IDictionary<string, object>.get_Item(string key)
    {
      object obj;
      if (!this.TryGetValueForKey(key, out obj))
        throw Error.KeyDoesNotExistInExpando((object) key);
      else
        return obj;
    }

    [__DynamicallyInvokable]
    void IDictionary<string, object>.set_Item(string key, object value)
    {
      ContractUtils.RequiresNotNull((object) key, "key");
      this.TrySetValue((object) null, -1, value, key, false, false);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    void IDictionary<string, object>.Add(string key, object value)
    {
      this.TryAddMember(key, value);
    }

    [__DynamicallyInvokable]
    bool IDictionary<string, object>.ContainsKey(string key)
    {
      ContractUtils.RequiresNotNull((object) key, "key");
      ExpandoObject.ExpandoData expandoData = this._data;
      int indexCaseSensitive = expandoData.Class.GetValueIndexCaseSensitive(key);
      if (indexCaseSensitive >= 0)
        return expandoData[indexCaseSensitive] != ExpandoObject.Uninitialized;
      else
        return false;
    }

    [__DynamicallyInvokable]
    bool IDictionary<string, object>.Remove(string key)
    {
      ContractUtils.RequiresNotNull((object) key, "key");
      return this.TryDeleteValue((object) null, -1, key, false, ExpandoObject.Uninitialized);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    bool IDictionary<string, object>.TryGetValue(string key, out object value)
    {
      return this.TryGetValueForKey(key, out value);
    }

    [__DynamicallyInvokable]
    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
    {
      this.TryAddMember(item.Key, item.Value);
    }

    [__DynamicallyInvokable]
    void ICollection<KeyValuePair<string, object>>.Clear()
    {
      ExpandoObject.ExpandoData expandoData;
      lock (this.LockObject)
      {
        expandoData = this._data;
        this._data = ExpandoObject.ExpandoData.Empty;
        this._count = 0;
      }
      PropertyChangedEventHandler changedEventHandler = this._propertyChanged;
      if (changedEventHandler == null)
        return;
      int index = 0;
      for (int length = expandoData.Class.Keys.Length; index < length; ++index)
      {
        if (expandoData[index] != ExpandoObject.Uninitialized)
          changedEventHandler((object) this, new PropertyChangedEventArgs(expandoData.Class.Keys[index]));
      }
    }

    [__DynamicallyInvokable]
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
    {
      object objA;
      if (!this.TryGetValueForKey(item.Key, out objA))
        return false;
      else
        return object.Equals(objA, item.Value);
    }

    [__DynamicallyInvokable]
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      ContractUtils.RequiresNotNull((object) array, "array");
      ContractUtils.RequiresArrayRange<KeyValuePair<string, object>>((IList<KeyValuePair<string, object>>) array, arrayIndex, this._count, "arrayIndex", "Count");
      lock (this.LockObject)
      {
        foreach (KeyValuePair<string, object> item_0 in (IEnumerable<KeyValuePair<string, object>>) this)
          array[arrayIndex++] = item_0;
      }
    }

    [__DynamicallyInvokable]
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
    {
      return this.TryDeleteValue((object) null, -1, item.Key, false, item.Value);
    }

    [__DynamicallyInvokable]
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
      ExpandoObject.ExpandoData data = this._data;
      return this.GetExpandoEnumerator(data, data.Version);
    }

    [__DynamicallyInvokable]
    IEnumerator IEnumerable.GetEnumerator()
    {
      ExpandoObject.ExpandoData data = this._data;
      return (IEnumerator) this.GetExpandoEnumerator(data, data.Version);
    }

    internal void TrySetValue(object indexClass, int index, object value, string name, bool ignoreCase, bool add)
    {
      ExpandoObject.ExpandoData expandoData;
      object obj;
      lock (this.LockObject)
      {
        expandoData = this._data;
        if (expandoData.Class != indexClass || ignoreCase)
        {
          index = expandoData.Class.GetValueIndex(name, ignoreCase, this);
          if (index == -2)
            throw Error.AmbiguousMatchInExpandoObject((object) name);
          if (index == -1)
          {
            int local_2 = ignoreCase ? expandoData.Class.GetValueIndexCaseSensitive(name) : index;
            if (local_2 != -1)
            {
              index = local_2;
            }
            else
            {
              ExpandoClass local_3 = expandoData.Class.FindNewClass(name);
              expandoData = this.PromoteClassCore(expandoData.Class, local_3);
              index = expandoData.Class.GetValueIndexCaseSensitive(name);
            }
          }
        }
        obj = expandoData[index];
        if (obj == ExpandoObject.Uninitialized)
          ++this._count;
        else if (add)
          throw Error.SameKeyExistsInExpando((object) name);
        expandoData[index] = value;
      }
      PropertyChangedEventHandler changedEventHandler = this._propertyChanged;
      if (changedEventHandler == null || value == obj)
        return;
      changedEventHandler((object) this, new PropertyChangedEventArgs(expandoData.Class.Keys[index]));
    }

    internal bool TryDeleteValue(object indexClass, int index, string name, bool ignoreCase, object deleteValue)
    {
      ExpandoObject.ExpandoData expandoData;
      lock (this.LockObject)
      {
        expandoData = this._data;
        if (expandoData.Class != indexClass || ignoreCase)
        {
          index = expandoData.Class.GetValueIndex(name, ignoreCase, this);
          if (index == -2)
            throw Error.AmbiguousMatchInExpandoObject((object) name);
        }
        if (index == -1)
          return false;
        object local_1 = expandoData[index];
        if (local_1 == ExpandoObject.Uninitialized || deleteValue != ExpandoObject.Uninitialized && !object.Equals(local_1, deleteValue))
          return false;
        expandoData[index] = ExpandoObject.Uninitialized;
        --this._count;
      }
      PropertyChangedEventHandler changedEventHandler = this._propertyChanged;
      if (changedEventHandler != null)
        changedEventHandler((object) this, new PropertyChangedEventArgs(expandoData.Class.Keys[index]));
      return true;
    }

    internal bool IsDeletedMember(int index)
    {
      if (index == this._data.Length)
        return false;
      else
        return this._data[index] == ExpandoObject.Uninitialized;
    }

    private ExpandoObject.ExpandoData PromoteClassCore(ExpandoClass oldClass, ExpandoClass newClass)
    {
      lock (this.LockObject)
      {
        if (this._data.Class == oldClass)
          this._data = this._data.UpdateClass(newClass);
        return this._data;
      }
    }

    internal void PromoteClass(object oldClass, object newClass)
    {
      this.PromoteClassCore((ExpandoClass) oldClass, (ExpandoClass) newClass);
    }

    private bool ExpandoContainsKey(string key)
    {
      return this._data.Class.GetValueIndexCaseSensitive(key) >= 0;
    }

    private IEnumerator<KeyValuePair<string, object>> GetExpandoEnumerator(ExpandoObject.ExpandoData data, int version)
    {
      for (int i = 0; i < data.Class.Keys.Length; ++i)
      {
        if (this._data.Version != version || data != this._data)
          throw Error.CollectionModifiedWhileEnumerating();
        object temp = data[i];
        if (temp != ExpandoObject.Uninitialized)
          yield return new KeyValuePair<string, object>(data.Class.Keys[i], temp);
      }
    }

    private class ExpandoData
    {
      internal static ExpandoObject.ExpandoData Empty = new ExpandoObject.ExpandoData();
      internal readonly ExpandoClass Class;
      private readonly object[] _dataArray;
      private int _version;

      internal object this[int index]
      {
        get
        {
          return this._dataArray[index];
        }
        set
        {
          ++this._version;
          this._dataArray[index] = value;
        }
      }

      internal int Version
      {
        get
        {
          return this._version;
        }
      }

      internal int Length
      {
        get
        {
          return this._dataArray.Length;
        }
      }

      static ExpandoData()
      {
      }

      private ExpandoData()
      {
        this.Class = ExpandoClass.Empty;
        this._dataArray = new object[0];
      }

      internal ExpandoData(ExpandoClass klass, object[] data, int version)
      {
        this.Class = klass;
        this._dataArray = data;
        this._version = version;
      }

      internal ExpandoObject.ExpandoData UpdateClass(ExpandoClass newClass)
      {
        if (this._dataArray.Length >= newClass.Keys.Length)
        {
          this[newClass.Keys.Length - 1] = ExpandoObject.Uninitialized;
          return new ExpandoObject.ExpandoData(newClass, this._dataArray, this._version);
        }
        else
        {
          int length = this._dataArray.Length;
          object[] data = new object[ExpandoObject.ExpandoData.GetAlignedSize(newClass.Keys.Length)];
          Array.Copy((Array) this._dataArray, (Array) data, this._dataArray.Length);
          ExpandoObject.ExpandoData expandoData = new ExpandoObject.ExpandoData(newClass, data, this._version);
          expandoData[length] = ExpandoObject.Uninitialized;
          return expandoData;
        }
      }

      private static int GetAlignedSize(int len)
      {
        return len + 7 & -8;
      }
    }

    private sealed class KeyCollectionDebugView
    {
      private ICollection<string> collection;

      [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
      public string[] Items
      {
        get
        {
          string[] array = new string[this.collection.Count];
          this.collection.CopyTo(array, 0);
          return array;
        }
      }

      public KeyCollectionDebugView(ICollection<string> collection)
      {
        this.collection = collection;
      }
    }

    [DebuggerTypeProxy(typeof (ExpandoObject.KeyCollectionDebugView))]
    [DebuggerDisplay("Count = {Count}")]
    private class KeyCollection : ICollection<string>, IEnumerable<string>, IEnumerable
    {
      private readonly ExpandoObject _expando;
      private readonly int _expandoVersion;
      private readonly int _expandoCount;
      private readonly ExpandoObject.ExpandoData _expandoData;

      public int Count
      {
        get
        {
          this.CheckVersion();
          return this._expandoCount;
        }
      }

      public bool IsReadOnly
      {
        get
        {
          return true;
        }
      }

      internal KeyCollection(ExpandoObject expando)
      {
        lock (expando.LockObject)
        {
          this._expando = expando;
          this._expandoVersion = expando._data.Version;
          this._expandoCount = expando._count;
          this._expandoData = expando._data;
        }
      }

      private void CheckVersion()
      {
        if (this._expando._data.Version != this._expandoVersion || this._expandoData != this._expando._data)
          throw Error.CollectionModifiedWhileEnumerating();
      }

      public void Add(string item)
      {
        throw Error.CollectionReadOnly();
      }

      public void Clear()
      {
        throw Error.CollectionReadOnly();
      }

      public bool Contains(string item)
      {
        lock (this._expando.LockObject)
        {
          this.CheckVersion();
          return this._expando.ExpandoContainsKey(item);
        }
      }

      public void CopyTo(string[] array, int arrayIndex)
      {
        ContractUtils.RequiresNotNull((object) array, "array");
        ContractUtils.RequiresArrayRange<string>((IList<string>) array, arrayIndex, this._expandoCount, "arrayIndex", "Count");
        lock (this._expando.LockObject)
        {
          this.CheckVersion();
          ExpandoObject.ExpandoData local_0 = this._expando._data;
          for (int local_1 = 0; local_1 < local_0.Class.Keys.Length; ++local_1)
          {
            if (local_0[local_1] != ExpandoObject.Uninitialized)
              array[arrayIndex++] = local_0.Class.Keys[local_1];
          }
        }
      }

      public bool Remove(string item)
      {
        throw Error.CollectionReadOnly();
      }

      public IEnumerator<string> GetEnumerator()
      {
        int i = 0;
        for (int n = this._expandoData.Class.Keys.Length; i < n; ++i)
        {
          this.CheckVersion();
          if (this._expandoData[i] != ExpandoObject.Uninitialized)
            yield return this._expandoData.Class.Keys[i];
        }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) this.GetEnumerator();
      }
    }

    private sealed class ValueCollectionDebugView
    {
      private ICollection<object> collection;

      [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
      public object[] Items
      {
        get
        {
          object[] array = new object[this.collection.Count];
          this.collection.CopyTo(array, 0);
          return array;
        }
      }

      public ValueCollectionDebugView(ICollection<object> collection)
      {
        this.collection = collection;
      }
    }

    [DebuggerTypeProxy(typeof (ExpandoObject.ValueCollectionDebugView))]
    [DebuggerDisplay("Count = {Count}")]
    private class ValueCollection : ICollection<object>, IEnumerable<object>, IEnumerable
    {
      private readonly ExpandoObject _expando;
      private readonly int _expandoVersion;
      private readonly int _expandoCount;
      private readonly ExpandoObject.ExpandoData _expandoData;

      public int Count
      {
        get
        {
          this.CheckVersion();
          return this._expandoCount;
        }
      }

      public bool IsReadOnly
      {
        get
        {
          return true;
        }
      }

      internal ValueCollection(ExpandoObject expando)
      {
        lock (expando.LockObject)
        {
          this._expando = expando;
          this._expandoVersion = expando._data.Version;
          this._expandoCount = expando._count;
          this._expandoData = expando._data;
        }
      }

      private void CheckVersion()
      {
        if (this._expando._data.Version != this._expandoVersion || this._expandoData != this._expando._data)
          throw Error.CollectionModifiedWhileEnumerating();
      }

      public void Add(object item)
      {
        throw Error.CollectionReadOnly();
      }

      public void Clear()
      {
        throw Error.CollectionReadOnly();
      }

      public bool Contains(object item)
      {
        lock (this._expando.LockObject)
        {
          this.CheckVersion();
          ExpandoObject.ExpandoData local_0 = this._expando._data;
          for (int local_1 = 0; local_1 < local_0.Class.Keys.Length; ++local_1)
          {
            if (object.Equals(local_0[local_1], item))
              return true;
          }
          return false;
        }
      }

      public void CopyTo(object[] array, int arrayIndex)
      {
        ContractUtils.RequiresNotNull((object) array, "array");
        ContractUtils.RequiresArrayRange<object>((IList<object>) array, arrayIndex, this._expandoCount, "arrayIndex", "Count");
        lock (this._expando.LockObject)
        {
          this.CheckVersion();
          ExpandoObject.ExpandoData local_0 = this._expando._data;
          for (int local_1 = 0; local_1 < local_0.Class.Keys.Length; ++local_1)
          {
            if (local_0[local_1] != ExpandoObject.Uninitialized)
              array[arrayIndex++] = local_0[local_1];
          }
        }
      }

      public bool Remove(object item)
      {
        throw Error.CollectionReadOnly();
      }

      public IEnumerator<object> GetEnumerator()
      {
        ExpandoObject.ExpandoData data = this._expando._data;
        for (int i = 0; i < data.Class.Keys.Length; ++i)
        {
          this.CheckVersion();
          object temp = data[i];
          if (temp != ExpandoObject.Uninitialized)
            yield return temp;
        }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) this.GetEnumerator();
      }
    }

    private class MetaExpando : DynamicMetaObject
    {
      public ExpandoObject Value
      {
        get
        {
          return (ExpandoObject) base.Value;
        }
      }

      public MetaExpando(Expression expression, ExpandoObject value)
        : base(expression, BindingRestrictions.Empty, (object) value)
      {
      }

      private DynamicMetaObject BindGetOrInvokeMember(DynamicMetaObjectBinder binder, string name, bool ignoreCase, DynamicMetaObject fallback, Func<DynamicMetaObject, DynamicMetaObject> fallbackInvoke)
      {
        ExpandoClass @class = this.Value.Class;
        int valueIndex = @class.GetValueIndex(name, ignoreCase, this.Value);
        ParameterExpression parameterExpression = Expression.Parameter(typeof (object), "value");
        Expression test = (Expression) Expression.Call(typeof (RuntimeOps).GetMethod("ExpandoTryGetValue"), this.GetLimitedSelf(), (Expression) Expression.Constant((object) @class, typeof (object)), (Expression) Expression.Constant((object) valueIndex), (Expression) Expression.Constant((object) name), (Expression) Expression.Constant((object) (bool) (ignoreCase ? 1 : 0)), (Expression) parameterExpression);
        DynamicMetaObject dynamicMetaObject = new DynamicMetaObject((Expression) parameterExpression, BindingRestrictions.Empty);
        if (fallbackInvoke != null)
          dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
        DynamicMetaObject succeeds = new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression
        }, new Expression[1]
        {
          (Expression) Expression.Condition(test, dynamicMetaObject.Expression, fallback.Expression, typeof (object))
        }), dynamicMetaObject.Restrictions.Merge(fallback.Restrictions));
        return this.AddDynamicTestAndDefer(binder, this.Value.Class, (ExpandoClass) null, succeeds);
      }

      public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
      {
        ContractUtils.RequiresNotNull((object) binder, "binder");
        return this.BindGetOrInvokeMember((DynamicMetaObjectBinder) binder, binder.Name, binder.IgnoreCase, binder.FallbackGetMember((DynamicMetaObject) this), (Func<DynamicMetaObject, DynamicMetaObject>) null);
      }

      public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
      {
        ContractUtils.RequiresNotNull((object) binder, "binder");
        return this.BindGetOrInvokeMember((DynamicMetaObjectBinder) binder, binder.Name, binder.IgnoreCase, binder.FallbackInvokeMember((DynamicMetaObject) this, args), (Func<DynamicMetaObject, DynamicMetaObject>) (value => binder.FallbackInvoke(value, args, (DynamicMetaObject) null)));
      }

      public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
      {
        ContractUtils.RequiresNotNull((object) binder, "binder");
        ContractUtils.RequiresNotNull((object) value, "value");
        ExpandoClass klass;
        int index;
        ExpandoClass classEnsureIndex = this.GetClassEnsureIndex(binder.Name, binder.IgnoreCase, this.Value, out klass, out index);
        return this.AddDynamicTestAndDefer((DynamicMetaObjectBinder) binder, klass, classEnsureIndex, new DynamicMetaObject((Expression) Expression.Call(typeof (RuntimeOps).GetMethod("ExpandoTrySetValue"), this.GetLimitedSelf(), (Expression) Expression.Constant((object) klass, typeof (object)), (Expression) Expression.Constant((object) index), (Expression) Expression.Convert(value.Expression, typeof (object)), (Expression) Expression.Constant((object) binder.Name), (Expression) Expression.Constant((object) (bool) (binder.IgnoreCase ? 1 : 0))), BindingRestrictions.Empty));
      }

      public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
      {
        ContractUtils.RequiresNotNull((object) binder, "binder");
        Expression expression = (Expression) Expression.Call(typeof (RuntimeOps).GetMethod("ExpandoTryDeleteValue"), this.GetLimitedSelf(), (Expression) Expression.Constant((object) this.Value.Class, typeof (object)), (Expression) Expression.Constant((object) this.Value.Class.GetValueIndex(binder.Name, binder.IgnoreCase, this.Value)), (Expression) Expression.Constant((object) binder.Name), (Expression) Expression.Constant((object) (bool) (binder.IgnoreCase ? 1 : 0)));
        DynamicMetaObject dynamicMetaObject = binder.FallbackDeleteMember((DynamicMetaObject) this);
        DynamicMetaObject succeeds = new DynamicMetaObject((Expression) Expression.IfThen((Expression) Expression.Not(expression), dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
        return this.AddDynamicTestAndDefer((DynamicMetaObjectBinder) binder, this.Value.Class, (ExpandoClass) null, succeeds);
      }

      public override IEnumerable<string> GetDynamicMemberNames()
      {
        ExpandoObject.ExpandoData expandoData = this.Value._data;
        ExpandoClass klass = expandoData.Class;
        for (int i = 0; i < klass.Keys.Length; ++i)
        {
          object val = expandoData[i];
          if (val != ExpandoObject.Uninitialized)
            yield return klass.Keys[i];
        }
      }

      private DynamicMetaObject AddDynamicTestAndDefer(DynamicMetaObjectBinder binder, ExpandoClass klass, ExpandoClass originalClass, DynamicMetaObject succeeds)
      {
        Expression ifTrue = succeeds.Expression;
        if (originalClass != null)
          ifTrue = (Expression) Expression.Block((Expression) Expression.Call((Expression) null, typeof (RuntimeOps).GetMethod("ExpandoPromoteClass"), this.GetLimitedSelf(), (Expression) Expression.Constant((object) originalClass, typeof (object)), (Expression) Expression.Constant((object) klass, typeof (object))), succeeds.Expression);
        return new DynamicMetaObject((Expression) Expression.Condition((Expression) Expression.Call((Expression) null, typeof (RuntimeOps).GetMethod("ExpandoCheckVersion"), this.GetLimitedSelf(), (Expression) Expression.Constant((object) (originalClass ?? klass), typeof (object))), ifTrue, binder.GetUpdateExpression(ifTrue.Type)), this.GetRestrictions().Merge(succeeds.Restrictions));
      }

      private ExpandoClass GetClassEnsureIndex(string name, bool caseInsensitive, ExpandoObject obj, out ExpandoClass klass, out int index)
      {
        ExpandoClass @class = this.Value.Class;
        index = @class.GetValueIndex(name, caseInsensitive, obj);
        if (index == -2)
        {
          klass = @class;
          return (ExpandoClass) null;
        }
        else if (index == -1)
        {
          ExpandoClass newClass = @class.FindNewClass(name);
          klass = newClass;
          index = newClass.GetValueIndexCaseSensitive(name);
          return @class;
        }
        else
        {
          klass = @class;
          return (ExpandoClass) null;
        }
      }

      private Expression GetLimitedSelf()
      {
        if (TypeUtils.AreEquivalent(this.Expression.Type, this.LimitType))
          return this.Expression;
        else
          return (Expression) Expression.Convert(this.Expression, this.LimitType);
      }

      private BindingRestrictions GetRestrictions()
      {
        return BindingRestrictions.GetTypeRestriction((DynamicMetaObject) this);
      }
    }
  }
}
