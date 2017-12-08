using sTalk.Libraries.Communication.Packet.Data;
using System.Collections.Generic;

namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست ورود
    /// <para><see cref="Result.Success"/>: با درخواست ورود موافقت شد</para>
    /// <para><see cref="Result.Wrong"/>: گذرواژه درهم سازی شده صحیح نیست</para>
    /// </summary>
    public class LogIn : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>لیست اتاق های گفتگو</summary>
        public List<Room> Rooms { get; set; }

        /// <summary>لیست مخاطبان</summary>
        public List<User> Contacts { get; set; }

        /// <summary>لیست کاربران بلاک شده</summary>
        public List<string> Blocks { get; set; }

        /// <summary>لیست پیام های خصوصی آفلاین</summary>
        public List<OfflineMessage> OfflineMessages { get; set; }
    }
}