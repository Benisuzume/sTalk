using System.Security.Cryptography;
using System.Text;

namespace sTalk.Libraries.Encryption
{
    public static class Hash
    {
        private static MD5 _md5;

        static Hash()
        {
            _md5 = MD5.Create();
        }

        public static string CalculateMD5(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = _md5.ComputeHash(bytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
    }
}