namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست ارسال پیام خصوصی
    /// </summary>
    public class PrivateMessage : Identifier
    {
        /// <summary>شناسه پیام خصوصی</summary>
        public string Guid { get; set; }

        /// <summary>نام کاربر دریافت کننده پیام خصوصی</summary>
        public string Username { get; set; }

        /// <summary>متن پیام خصوصی</summary>
        public string Message { get; set; }
    }
}