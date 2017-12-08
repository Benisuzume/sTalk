namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست ارسال پیام خصوصی
    /// <para><see cref="Result.Success"/>: پیام با موفقیت ارسال شد</para>
    /// <para><see cref="Result.NotFound"/>: کاربر یافت نشد</para>
    /// <para><see cref="Result.Banned"/>: کاربر دریافت کننده پیام شما را مسدود کرده است<para>
    /// </summary>
    public class PrivateMessageResult : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>شناسه منحصر به فرد پیام خصوصی</summary>
        public string Guid { get; set; }
    }
}