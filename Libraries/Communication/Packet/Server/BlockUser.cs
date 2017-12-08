namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست مسدود کردن کاربر
    /// <para><see cref="Result.Success"/>: کاربر مورد نظر مسدود شد</para>
    /// <para><see cref="Result.Useless"/>: کاربر از قبل توسط شما مسدود شده است</para>
    /// <para><see cref="Result.NotFound"/>: کاربر یافت نشد</para>
    /// </summary>
    public class BlockUser : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }
    }
}