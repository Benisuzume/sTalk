namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست حذف مخاطب
    /// <para><see cref="Result.Success"/>: کاربر از لیست مخاطبان شما حذف شد</para>
    /// <para><see cref="Result.Useless"/>: کاربر در لیست مخاطبان شما وجود ندارد</para>
    /// <para><see cref="Result.NotFound"/>: کاربر یافت نشد</para>
    /// </summary>
    public class DeleteContact : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }
    }
}