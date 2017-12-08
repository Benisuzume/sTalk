namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست حذف مخاطب
    /// </summary>
    public class DeleteContact : Identifier
    {
        /// <summary>نام کاربر جهت حذف از لیست مخاطبان</summary>
        public string Username { get; set; }
    }
}