namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست ارسال پیام عمومی در اتاق گفتگو
    /// </summary>
    public class PublicMessage : Identifier
    {
        /// <summary>شناسه منحصر به فرد پیام عمومی</summary>
        public string Guid { get; set; }

        /// <summary>متن پیام عمومی</summary>
        public string Message { get; set; }
    }
}