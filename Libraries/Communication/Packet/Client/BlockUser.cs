namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست مسدود کردن کاربر
    /// </summary>
    public class BlockUser : Identifier
    {
        /// <summary>نام کاربر جهت مسدود کردن</summary>
        public string Username { get; set; }
    }
}