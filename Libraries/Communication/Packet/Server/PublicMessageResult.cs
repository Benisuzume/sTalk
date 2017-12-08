namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست ارسال پیام عمومی
    /// <para><see cref="Result.Success"/>: پیام با موفقیت ارسال شد</para>
    /// <para><see cref="Result.Useless"/>: در هیچ اتاق گفتگویی نیستید</para>
    /// <para><see cref="Result.Banned"/>: از ارسال پیام در این اتاق گفتگو محروم شده اید<para>
    /// </summary>
    public class PublicMessageResult : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>شناسه منحصر به فرد پیام عمومی</summary>
        public string Guid { get; set; }
    }
}