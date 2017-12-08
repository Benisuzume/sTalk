namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست دریافت تصویر پروفایل کاربر
    /// <para><see cref="Result.Success"/>: درخواست با موفقیت انجام شد</para>
    /// <para><see cref="Result.NotFound"/>: کاربر یافت نشد</para>
    /// <para><see cref="Result.Useless"/>: این کاربر تصویر پروفایل ندارد</para>
    /// </summary>
    public class GetProfilePicture : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>نام کاربری تصویر پروفایل دریافتی</summary>
        public string Username { get; set; }

        /// <summary>تصویر پروفایل کاربر</summary>
        public byte[] Image { get; set; }
    }
}