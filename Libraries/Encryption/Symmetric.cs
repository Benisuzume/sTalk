using System.IO;
using System.Security.Cryptography;

namespace sTalk.Libraries.Encryption
{
    public class Symmetric
    {
        private Aes _aes;

        public Symmetric()
        {
            _aes = Aes.Create();
        }

        public Symmetric(byte[] key, byte[] iv)
        {
            _aes = Aes.Create();

            _aes.Key = key;
            _aes.IV = iv;
        }

        public byte[] Key
        {
            get
            {
                return _aes.Key;
            }
        }

        public byte[] IV
        {
            get
            {
                return _aes.IV;
            }
        }

        public string Decrypt(byte[] cipherText)
        {
            using (var ms = new MemoryStream(cipherText))
            {
                using (var cs = new CryptoStream(ms, _aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public byte[] Encrypt(string plainText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return ms.ToArray();
                }
            }
        }
    }
}