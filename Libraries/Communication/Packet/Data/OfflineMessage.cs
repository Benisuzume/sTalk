using System;

namespace sTalk.Libraries.Communication.Packet.Data
{
    /// <summary>
    /// پیام خصوصی آفلاین
    /// </summary>
    public class OfflineMessage
    {
        /// <summary>زمان ارسال پیام خصوصی آفلاین</summary>
        public DateTime Time { get; set; }

        /// <summary>نام کاربر ارسال کننده پیام خصوصی آفلاین</summary>
        public string Username { get; set; }

        /// <summary>متن پیام خصوصی آفلاین</summary>
        public string Message { get; set; }
    }
}