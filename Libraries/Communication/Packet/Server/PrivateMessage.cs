using sTalk.Libraries.Communication.Packet.Data;

namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته تحویل پیام خصوصی به کاربر دریافت کننده
    /// </summary>
    public class PrivateMessage : Identifier
    {
        /// <summary>نام کاربر ارسال کننده پیام</summary>
        public string Username { get; set; }

        /// <summary>متن پیام خصوصی</summary>
        public string Message { get; set; }
    }
}