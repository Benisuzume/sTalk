using sTalk.Libraries.Communication.Packet.Data;
using System.Collections.Generic;

namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست ورود به اتاق گفتگو
    /// <para><see cref="Result.Success"/>: به اتاق گفتگو وارد شدید</para>
    /// <para><see cref="Result.Useless"/>: در اتاق گفتگوی دیگری هستید</para>
    /// <para><see cref="Result.NotFound"/>: اتاق گفتگو یافت نشد</para>
    /// <para><see cref="Result.Banned"/>: حساب کاربری شما از ورود به این اتاق گفتگو محروم شده است</para>
    /// <para><see cref="Result.Full"/>: ظرفیت اتاق گفتگو تکمیل است</para>
    /// </summary>
    public class JoinRoom : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>پیام سیستمی اتاق گفتگو</summary>
        public string Message { get; set; }

        /// <summary>لیست کاربران حاضر در اتاق جستجو</summary>
        public List<string> UserList { get; set; }
    }
}