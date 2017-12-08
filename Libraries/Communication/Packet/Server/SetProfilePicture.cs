namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست ثبت تصویر پروفایل
    /// <para><see cref="Result.Success"/>: درخواست با موفقیت انجام شد</para>
    /// </summary>
    public class SetProfilePicture : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }
    }
}