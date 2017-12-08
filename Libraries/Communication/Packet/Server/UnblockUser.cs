namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست رفع مسدودی کاربر
    /// <para><see cref="Result.Success"/>: کاربر مورد نظر رفع مسدودی شد</para>
    /// <para><see cref="Result.Useless"/>: کاربر توسط شما مسدود نشده است</para>
    /// <para><see cref="Result.NotFound"/>: کاربر یافت نشد</para>
    /// </summary>
    public class UnblockUser : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }
    }
}