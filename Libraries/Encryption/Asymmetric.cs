using System.Security.Cryptography;
using System.Text;

namespace sTalk.Libraries.Encryption
{
    public class Asymmetric
    {
        private RSACryptoServiceProvider _csp;

        public Asymmetric(string key)
        {
            _csp = new RSACryptoServiceProvider();
            _csp.FromXmlString(key);
        }

        public string Decrypt(byte[] cypherText)
        {
            var bytes = _csp.Decrypt(cypherText, true);
            return Encoding.UTF8.GetString(bytes);
        }

        public byte[] Encrypt(string plainText)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            return _csp.Encrypt(bytes, true);
        }
    }
}