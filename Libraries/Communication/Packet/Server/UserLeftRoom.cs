namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پیام اطلاع رسانی خروج کاربر از اتاق گفتگو
    /// </summary>
    public class UserLeftRoom : Identifier
    {
        /// <summary>نام کاربر خارج شده از اتاق گفتگو</summary>
        public string Username { get; set; }
    }
}