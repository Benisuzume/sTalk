namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست افزودن مخاطب
    /// </summary>
    public class AddContact : Identifier
    {
        /// <summary>نام کاربر جهت افزودن به لیست مخاطبان</summary>
        public string Username { get; set; }
    }
}