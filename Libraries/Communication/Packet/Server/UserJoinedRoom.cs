namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پیام اطلاع رسانی ورود کاربر به اتاق گفتگو
    /// </summary>
    public class UserJoinedRoom : Identifier
    {
        /// <summary>نام کاربر وارد شده به اتاق گفتگو</summary>
        public string Username { get; set; }
    }
}