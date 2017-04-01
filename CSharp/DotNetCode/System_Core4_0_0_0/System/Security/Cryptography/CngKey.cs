// Type: System.Security.Cryptography.CngKey
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CngKey : IDisposable
  {
    private SafeNCryptKeyHandle m_keyHandle;
    private SafeNCryptProviderHandle m_kspHandle;

    public CngAlgorithmGroup AlgorithmGroup
    {
      [SecuritySafeCritical] get
      {
        string propertyAsString = NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_keyHandle, "Algorithm Group", CngPropertyOptions.None);
        if (propertyAsString == null)
          return (CngAlgorithmGroup) null;
        else
          return new CngAlgorithmGroup(propertyAsString);
      }
    }

    public CngAlgorithm Algorithm
    {
      [SecuritySafeCritical] get
      {
        return new CngAlgorithm(NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_keyHandle, "Algorithm Name", CngPropertyOptions.None));
      }
    }

    public CngExportPolicies ExportPolicy
    {
      [SecuritySafeCritical] get
      {
        return (CngExportPolicies) NCryptNative.GetPropertyAsDWord((SafeNCryptHandle) this.m_keyHandle, "Export Policy", CngPropertyOptions.None);
      }
    }

    public SafeNCryptKeyHandle Handle
    {
      [SecurityCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)] get
      {
        return this.m_keyHandle.Duplicate();
      }
    }

    public bool IsEphemeral
    {
      [SecuritySafeCritical] get
      {
        bool foundProperty;
        byte[] property = NCryptNative.GetProperty((SafeNCryptHandle) this.m_keyHandle, "CLR IsEphemeral", CngPropertyOptions.CustomProperty, out foundProperty);
        if (foundProperty && property != null && property.Length == 1)
          return (int) property[0] == 1;
        else
          return false;
      }
      [SecurityCritical] private set
      {
        NCryptNative.SetProperty((SafeNCryptHandle) this.m_keyHandle, "CLR IsEphemeral", new byte[1]
        {
          value ? (byte) 1 : (byte) 0
        }, CngPropertyOptions.CustomProperty);
      }
    }

    public bool IsMachineKey
    {
      [SecuritySafeCritical] get
      {
        return (NCryptNative.GetPropertyAsDWord((SafeNCryptHandle) this.m_keyHandle, "Key Type", CngPropertyOptions.None) & 32) == 32;
      }
    }

    public string KeyName
    {
      [SecuritySafeCritical] get
      {
        if (this.IsEphemeral)
          return (string) null;
        else
          return NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_keyHandle, "Name", CngPropertyOptions.None);
      }
    }

    public int KeySize
    {
      [SecuritySafeCritical] get
      {
        return NCryptNative.GetPropertyAsDWord((SafeNCryptHandle) this.m_keyHandle, "Length", CngPropertyOptions.None);
      }
    }

    public CngKeyUsages KeyUsage
    {
      [SecuritySafeCritical] get
      {
        return (CngKeyUsages) NCryptNative.GetPropertyAsDWord((SafeNCryptHandle) this.m_keyHandle, "Key Usage", CngPropertyOptions.None);
      }
    }

    public IntPtr ParentWindowHandle
    {
      [SecuritySafeCritical] get
      {
        return NCryptNative.GetPropertyAsIntPtr((SafeNCryptHandle) this.m_keyHandle, "HWND Handle", CngPropertyOptions.None);
      }
      [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)] set
      {
        NCryptNative.SetProperty<IntPtr>((SafeNCryptHandle) this.m_keyHandle, "HWND Handle", value, CngPropertyOptions.None);
      }
    }

    public CngProvider Provider
    {
      [SecuritySafeCritical] get
      {
        string propertyAsString = NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_kspHandle, "Name", CngPropertyOptions.None);
        if (propertyAsString == null)
          return (CngProvider) null;
        else
          return new CngProvider(propertyAsString);
      }
    }

    public SafeNCryptProviderHandle ProviderHandle
    {
      [SecurityCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)] get
      {
        return this.m_kspHandle.Duplicate();
      }
    }

    public string UniqueName
    {
      [SecuritySafeCritical] get
      {
        if (this.IsEphemeral)
          return (string) null;
        else
          return NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_keyHandle, "Unique Name", CngPropertyOptions.None);
      }
    }

    public CngUIPolicy UIPolicy
    {
      [SecuritySafeCritical] get
      {
        NCryptNative.NCRYPT_UI_POLICY propertyAsStruct = NCryptNative.GetPropertyAsStruct<NCryptNative.NCRYPT_UI_POLICY>((SafeNCryptHandle) this.m_keyHandle, "UI Policy", CngPropertyOptions.None);
        string propertyAsString = NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_keyHandle, "Use Context", CngPropertyOptions.None);
        return new CngUIPolicy(propertyAsStruct.dwFlags, propertyAsStruct.pszFriendlyName, propertyAsStruct.pszDescription, propertyAsString, propertyAsStruct.pszCreationTitle);
      }
    }

    [SecurityCritical]
    private CngKey(SafeNCryptProviderHandle kspHandle, SafeNCryptKeyHandle keyHandle)
    {
      this.m_keyHandle = keyHandle;
      this.m_kspHandle = kspHandle;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static CngKey Create(CngAlgorithm algorithm)
    {
      return CngKey.Create(algorithm, (string) null);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static CngKey Create(CngAlgorithm algorithm, string keyName)
    {
      return CngKey.Create(algorithm, keyName, (CngKeyCreationParameters) null);
    }

    [SecuritySafeCritical]
    public static CngKey Create(CngAlgorithm algorithm, string keyName, CngKeyCreationParameters creationParameters)
    {
      if (algorithm == (CngAlgorithm) null)
        throw new ArgumentNullException("algorithm");
      if (creationParameters == null)
        creationParameters = new CngKeyCreationParameters();
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      if (keyName != null)
        new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags)
        {
          AccessEntries = {
            new KeyContainerPermissionAccessEntry(keyName, KeyContainerPermissionFlags.Create)
            {
              ProviderName = creationParameters.Provider.Provider
            }
          }
        }.Demand();
      SafeNCryptProviderHandle ncryptProviderHandle = NCryptNative.OpenStorageProvider(creationParameters.Provider.Provider);
      SafeNCryptKeyHandle persistedKey = NCryptNative.CreatePersistedKey(ncryptProviderHandle, algorithm.Algorithm, keyName, creationParameters.KeyCreationOptions);
      CngKey.SetKeyProperties(persistedKey, creationParameters);
      NCryptNative.FinalizeKey(persistedKey);
      CngKey cngKey = new CngKey(ncryptProviderHandle, persistedKey);
      if (keyName == null)
        cngKey.IsEphemeral = true;
      return cngKey;
    }

    [SecuritySafeCritical]
    public void Delete()
    {
      KeyContainerPermission containerPermission = this.BuildKeyContainerPermission(KeyContainerPermissionFlags.Delete);
      if (containerPermission != null)
        containerPermission.Demand();
      NCryptNative.DeleteKey(this.m_keyHandle);
      this.Dispose();
    }

    [SecuritySafeCritical]
    public void Dispose()
    {
      if (this.m_kspHandle != null)
        this.m_kspHandle.Dispose();
      if (this.m_keyHandle == null)
        return;
      this.m_keyHandle.Dispose();
    }

    public static bool Exists(string keyName)
    {
      return CngKey.Exists(keyName, CngProvider.MicrosoftSoftwareKeyStorageProvider);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static bool Exists(string keyName, CngProvider provider)
    {
      return CngKey.Exists(keyName, provider, CngKeyOpenOptions.None);
    }

    [SecuritySafeCritical]
    public static bool Exists(string keyName, CngProvider provider, CngKeyOpenOptions options)
    {
      if (keyName == null)
        throw new ArgumentNullException("keyName");
      if (provider == (CngProvider) null)
        throw new ArgumentNullException("provider");
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      using (SafeNCryptProviderHandle hProvider = NCryptNative.OpenStorageProvider(provider.Provider))
      {
        SafeNCryptKeyHandle phKey = (SafeNCryptKeyHandle) null;
        try
        {
          NCryptNative.ErrorCode errorCode = NCryptNative.UnsafeNativeMethods.NCryptOpenKey(hProvider, out phKey, keyName, 0, options);
          bool flag = errorCode == NCryptNative.ErrorCode.KeyDoesNotExist || errorCode == NCryptNative.ErrorCode.NotFound;
          if (errorCode != NCryptNative.ErrorCode.Success && !flag)
            throw new CryptographicException((int) errorCode);
          else
            return errorCode == NCryptNative.ErrorCode.Success;
        }
        finally
        {
          if (phKey != null)
            phKey.Dispose();
        }
      }
    }

    public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format)
    {
      return CngKey.Import(keyBlob, format, CngProvider.MicrosoftSoftwareKeyStorageProvider);
    }

    [SecuritySafeCritical]
    public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format, CngProvider provider)
    {
      if (keyBlob == null)
        throw new ArgumentNullException("keyBlob");
      if (format == (CngKeyBlobFormat) null)
        throw new ArgumentNullException("format");
      if (provider == (CngProvider) null)
        throw new ArgumentNullException("provider");
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      if (!(format == CngKeyBlobFormat.EccPublicBlob) && !(format == CngKeyBlobFormat.GenericPublicBlob))
        new KeyContainerPermission(KeyContainerPermissionFlags.Import).Demand();
      SafeNCryptProviderHandle ncryptProviderHandle = NCryptNative.OpenStorageProvider(provider.Provider);
      SafeNCryptKeyHandle keyHandle = NCryptNative.ImportKey(ncryptProviderHandle, keyBlob, format.Format);
      return new CngKey(ncryptProviderHandle, keyHandle)
      {
        IsEphemeral = format != CngKeyBlobFormat.OpaqueTransportBlob
      };
    }

    [SecuritySafeCritical]
    public byte[] Export(CngKeyBlobFormat format)
    {
      if (format == (CngKeyBlobFormat) null)
        throw new ArgumentNullException("format");
      KeyContainerPermission containerPermission = this.BuildKeyContainerPermission(KeyContainerPermissionFlags.Export);
      if (containerPermission != null)
        containerPermission.Demand();
      return NCryptNative.ExportKey(this.m_keyHandle, format.Format);
    }

    [SecuritySafeCritical]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public CngProperty GetProperty(string name, CngPropertyOptions options)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      bool foundProperty;
      byte[] property = NCryptNative.GetProperty((SafeNCryptHandle) this.m_keyHandle, name, options, out foundProperty);
      if (!foundProperty)
        throw new CryptographicException(-2146893807);
      else
        return new CngProperty(name, property, options);
    }

    [SecuritySafeCritical]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public bool HasProperty(string name, CngPropertyOptions options)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      bool foundProperty;
      NCryptNative.GetProperty((SafeNCryptHandle) this.m_keyHandle, name, options, out foundProperty);
      return foundProperty;
    }

    public static CngKey Open(string keyName)
    {
      return CngKey.Open(keyName, CngProvider.MicrosoftSoftwareKeyStorageProvider);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static CngKey Open(string keyName, CngProvider provider)
    {
      return CngKey.Open(keyName, provider, CngKeyOpenOptions.None);
    }

    [SecuritySafeCritical]
    public static CngKey Open(string keyName, CngProvider provider, CngKeyOpenOptions openOptions)
    {
      if (keyName == null)
        throw new ArgumentNullException("keyName");
      if (provider == (CngProvider) null)
        throw new ArgumentNullException("provider");
      if (!NCryptNative.NCryptSupported)
        throw new PlatformNotSupportedException(SR.GetString("Cryptography_PlatformNotSupported"));
      new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags)
      {
        AccessEntries = {
          new KeyContainerPermissionAccessEntry(keyName, KeyContainerPermissionFlags.Open)
          {
            ProviderName = provider.Provider
          }
        }
      }.Demand();
      SafeNCryptProviderHandle ncryptProviderHandle = NCryptNative.OpenStorageProvider(provider.Provider);
      SafeNCryptKeyHandle keyHandle = NCryptNative.OpenKey(ncryptProviderHandle, keyName, openOptions);
      return new CngKey(ncryptProviderHandle, keyHandle);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public static CngKey Open(SafeNCryptKeyHandle keyHandle, CngKeyHandleOpenOptions keyHandleOpenOptions)
    {
      if (keyHandle == null)
        throw new ArgumentNullException("keyHandle");
      if (keyHandle.IsClosed || keyHandle.IsInvalid)
        throw new ArgumentException(SR.GetString("Cryptography_OpenInvalidHandle"), "keyHandle");
      SafeNCryptKeyHandle keyHandle1 = keyHandle.Duplicate();
      SafeNCryptProviderHandle kspHandle = new SafeNCryptProviderHandle();
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
      }
      finally
      {
        IntPtr propertyAsIntPtr = NCryptNative.GetPropertyAsIntPtr((SafeNCryptHandle) keyHandle, "Provider Handle", CngPropertyOptions.None);
        kspHandle.SetHandleValue(propertyAsIntPtr);
      }
      CngKey cngKey = (CngKey) null;
      bool flag1 = false;
      try
      {
        cngKey = new CngKey(kspHandle, keyHandle1);
        bool flag2 = (keyHandleOpenOptions & CngKeyHandleOpenOptions.EphemeralKey) == CngKeyHandleOpenOptions.EphemeralKey;
        if (!cngKey.IsEphemeral && flag2)
          cngKey.IsEphemeral = true;
        else if (cngKey.IsEphemeral && !flag2)
          throw new ArgumentException(SR.GetString("Cryptography_OpenEphemeralKeyHandleWithoutEphemeralFlag"), "keyHandleOpenOptions");
        flag1 = true;
      }
      finally
      {
        if (!flag1 && cngKey != null)
          cngKey.Dispose();
      }
      return cngKey;
    }

    [SecuritySafeCritical]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public void SetProperty(CngProperty property)
    {
      NCryptNative.SetProperty((SafeNCryptHandle) this.m_keyHandle, property.Name, property.Value, property.Options);
    }

    [SecuritySafeCritical]
    internal KeyContainerPermission BuildKeyContainerPermission(KeyContainerPermissionFlags flags)
    {
      KeyContainerPermission containerPermission = (KeyContainerPermission) null;
      if (!this.IsEphemeral)
      {
        string keyContainerName = (string) null;
        string str = (string) null;
        try
        {
          keyContainerName = this.KeyName;
          str = NCryptNative.GetPropertyAsString((SafeNCryptHandle) this.m_kspHandle, "Name", CngPropertyOptions.None);
        }
        catch (CryptographicException ex)
        {
        }
        if (keyContainerName != null)
        {
          KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(keyContainerName, flags);
          accessEntry.ProviderName = str;
          containerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
          containerPermission.AccessEntries.Add(accessEntry);
        }
        else
          containerPermission = new KeyContainerPermission(flags);
      }
      return containerPermission;
    }

    [SecurityCritical]
    private static void SetKeyProperties(SafeNCryptKeyHandle keyHandle, CngKeyCreationParameters creationParameters)
    {
      if (creationParameters.ExportPolicy.HasValue)
        NCryptNative.SetProperty((SafeNCryptHandle) keyHandle, "Export Policy", (int) creationParameters.ExportPolicy.Value, CngPropertyOptions.Persist);
      if (creationParameters.KeyUsage.HasValue)
        NCryptNative.SetProperty((SafeNCryptHandle) keyHandle, "Key Usage", (int) creationParameters.KeyUsage.Value, CngPropertyOptions.Persist);
      if (creationParameters.ParentWindowHandle != IntPtr.Zero)
        NCryptNative.SetProperty<IntPtr>((SafeNCryptHandle) keyHandle, "HWND Handle", creationParameters.ParentWindowHandle, CngPropertyOptions.None);
      if (creationParameters.UIPolicy != null)
      {
        NCryptNative.SetProperty<NCryptNative.NCRYPT_UI_POLICY>((SafeNCryptHandle) keyHandle, "UI Policy", new NCryptNative.NCRYPT_UI_POLICY()
        {
          dwVersion = 1,
          dwFlags = creationParameters.UIPolicy.ProtectionLevel,
          pszCreationTitle = creationParameters.UIPolicy.CreationTitle,
          pszFriendlyName = creationParameters.UIPolicy.FriendlyName,
          pszDescription = creationParameters.UIPolicy.Description
        }, CngPropertyOptions.Persist);
        if (creationParameters.UIPolicy.UseContext != null)
          NCryptNative.SetProperty((SafeNCryptHandle) keyHandle, "Use Context", creationParameters.UIPolicy.UseContext, CngPropertyOptions.Persist);
      }
      foreach (CngProperty cngProperty in (Collection<CngProperty>) creationParameters.ParametersNoDemand)
        NCryptNative.SetProperty((SafeNCryptHandle) keyHandle, cngProperty.Name, cngProperty.Value, cngProperty.Options);
    }
  }
}
