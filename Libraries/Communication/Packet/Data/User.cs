namespace sTalk.Libraries.Communication.Packet.Data
{
    /// <summary>
    /// کاربر
    /// </summary>
    public class User
    {
        /// <summary>شناسه کاربر</summary>
        public uint Id { get; set; }

        /// <summary>نام کابری</summary>
        public string Name { get; set; }

        /// <summary>وضعیف کاربر</summary>
        public Status Status { get; set; }

        /// <summary>نام مستعار</summary>
        public string Nickname { get; set; }

        public static explicit operator User(Account account)
        {
            return new User()
            {
                Id = account.Id,
                Name = account.Username,
                Nickname = account.Nickname
            };
        }
    }
}