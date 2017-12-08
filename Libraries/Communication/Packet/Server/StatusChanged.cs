namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پیام اطلاع رسانی تغییر حالت کاربر
    /// </summary>
    public class UserStatusChanged : Identifier
    {
        /// <summary>نام کاربر</summary>
        public string Username { get; set; }

        /// <summary>وضعیت جدید کاربر</summary>
        public Status Status { get; set; }
    }
}