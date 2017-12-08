namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست ثبت تصویر پروفایل
    /// </summary>
    public class SetProfilePicture : Identifier
    {
        /// <summary>تصویر پروفایل</summary>
        public byte[] Image { get; set; }
    }
}