namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته تحویل پیام عمومی به کاربران حاضر در اتاق گفتگو
    /// </summary>
    public class PublicMessage : Identifier
    {
        /// <summary>نام کاربر ارسال کننده پیام</summary>
        public string Username { get; set; }

        /// <summary>متن پیام عمومی</summary>
        public string Message { get; set; }
    }
}