// Type: System.Security.Cryptography.AesManaged
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Security.Cryptography
{
  public sealed class AesManaged : Aes
  {
    private RijndaelManaged m_rijndael;

    public override int FeedbackSize
    {
      get
      {
        return this.m_rijndael.FeedbackSize;
      }
      set
      {
        this.m_rijndael.FeedbackSize = value;
      }
    }

    public override byte[] IV
    {
      get
      {
        return this.m_rijndael.IV;
      }
      set
      {
        this.m_rijndael.IV = value;
      }
    }

    public override byte[] Key
    {
      get
      {
        return this.m_rijndael.Key;
      }
      set
      {
        this.m_rijndael.Key = value;
      }
    }

    public override int KeySize
    {
      get
      {
        return this.m_rijndael.KeySize;
      }
      set
      {
        this.m_rijndael.KeySize = value;
      }
    }

    public override CipherMode Mode
    {
      get
      {
        return this.m_rijndael.Mode;
      }
      set
      {
        if (value == CipherMode.CFB || value == CipherMode.OFB)
          throw new CryptographicException(SR.GetString("Cryptography_InvalidCipherMode"));
        this.m_rijndael.Mode = value;
      }
    }

    public override PaddingMode Padding
    {
      get
      {
        return this.m_rijndael.Padding;
      }
      set
      {
        this.m_rijndael.Padding = value;
      }
    }

    public AesManaged()
    {
      if (CryptoConfig.AllowOnlyFipsAlgorithms)
        throw new InvalidOperationException(SR.GetString("Cryptography_NonCompliantFIPSAlgorithm"));
      this.m_rijndael = new RijndaelManaged();
      this.m_rijndael.BlockSize = this.BlockSize;
      this.m_rijndael.KeySize = this.KeySize;
    }

    public override ICryptoTransform CreateDecryptor()
    {
      return this.m_rijndael.CreateDecryptor();
    }

    public override ICryptoTransform CreateDecryptor(byte[] key, byte[] iv)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      if (!this.ValidKeySize(key.Length * 8))
        throw new ArgumentException(SR.GetString("Cryptography_InvalidKeySize"), "key");
      if (iv != null && iv.Length * 8 != this.BlockSizeValue)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidIVSize"), "iv");
      else
        return this.m_rijndael.CreateDecryptor(key, iv);
    }

    public override ICryptoTransform CreateEncryptor()
    {
      return this.m_rijndael.CreateEncryptor();
    }

    public override ICryptoTransform CreateEncryptor(byte[] key, byte[] iv)
    {
      if (key == null)
        throw new ArgumentNullException("key");
      if (!this.ValidKeySize(key.Length * 8))
        throw new ArgumentException(SR.GetString("Cryptography_InvalidKeySize"), "key");
      if (iv != null && iv.Length * 8 != this.BlockSizeValue)
        throw new ArgumentException(SR.GetString("Cryptography_InvalidIVSize"), "iv");
      else
        return this.m_rijndael.CreateEncryptor(key, iv);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        this.m_rijndael.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public override void GenerateIV()
    {
      this.m_rijndael.GenerateIV();
    }

    public override void GenerateKey()
    {
      this.m_rijndael.GenerateKey();
    }
  }
}
