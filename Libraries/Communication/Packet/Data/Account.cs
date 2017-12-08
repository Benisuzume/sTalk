namespace sTalk.Libraries.Communication.Packet.Data
{
    /// <summary>
    /// حساب کاربری
    /// </summary>
    public class Account
    {
        /// <summary>شناسه کاربر</summary>
        public uint Id { get; set; }

        /// <summary>نام کاربری</summary>
        public string Username { get; set; }

        /// <summary>گذرواژه کاربر</summary>
        public string Password { get; set; }

        /// <summary>ایمیل کاربر</summary>
        public string Email { get; set; }

        /// <summary>وضعیت محرومیت کاربر</summary>
        public bool IsBanned { get; set; }

        /// <summary>نام مستعار</summary>
        public string Nickname { get; set; }
    }
}