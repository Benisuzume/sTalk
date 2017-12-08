namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست دریافت تصویر پروفایل کاربر
    /// </summary>
    public class GetProfilePicture : Identifier
    {
        /// <summary>نام کاربر مورد نظر</summary>
        public string Username { get; set; }
    }
}