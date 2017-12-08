namespace sTalk.Libraries.Communication
{
    public static class Sanitize
    {
        public static string Username(string username)
        {
            // حذف فضای خالی ابتدا و انتهای نام کاربری
            username = username.Trim();
            if (string.IsNullOrEmpty(username))
                return null;

            // بررسی طول نام کاربری
            var len = username.Length;
            if (len < 3 || len > 16)
                return null;

            // بررسی کاراکتر های نام کاربری
            foreach (var ch in username)
            {
                if (!(char.IsLetter(ch) || char.IsNumber(ch) || ch == '_'))
                    return null;
            }

            // فقط یک کاراکتر زیرخط در نام کاربری مجاز است
            if (username.IndexOf('_') != username.LastIndexOf('_'))
                return null;

            // تبدیل نام کاربری به حروف کوچک
            username = username.ToLower();
            return username;
        }

        public static string Hash(string hash)
        {
            // حذف فضای خالی از ابتدا و انتهای مقدار درهم ساز
            hash = hash.Trim();
            if (string.IsNullOrEmpty(hash))
                return null;

            // بررسی طول مقدار درهم ساز
            var len = hash.Length;
            if (len != 32)
                return null;

            // بررسی کاراکتر های مقدار درهم ساز
            foreach (var ch in hash)
            {
                if (!(char.IsLetter(ch) || char.IsNumber(ch)))
                    return null;
            }

            // تبدیل مقدار درهم ساز به حروف کوچک
            hash = hash.ToLower();
            return hash;
        }
    }
}