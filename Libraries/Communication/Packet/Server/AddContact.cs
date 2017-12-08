using sTalk.Libraries.Communication.Packet.Data;

namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست افزدون مخاطب
    /// <para><see cref="Result.Success"/>: کاربر به لیست مخاطبان شما افزوده شد</para>
    /// <para><see cref="Result.Useless"/>: کاربر از قبل در لیست مخاطبان شما وجود دارد</para>
    /// <para><see cref="Result.NotFound"/>: کاربر یافت نشد</para>
    /// <para><see cref="Result.Banned"/>: حساب کاربری شما توسط این کاربر بلاک شده است</para>
    /// </summary>
    public class AddContact : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>کاربر اضافه شده به لیست مخاطبان شما</summary>
        public User User { get; set; }
    }
}