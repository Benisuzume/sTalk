namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست رفع مسدودی کاربر
    /// </summary>
    public class UnblockUser : Identifier
    {
        /// <summary>نام کاربر جهت رفع مسدودی</summary>
        public string Username { get; set; }
    }
}