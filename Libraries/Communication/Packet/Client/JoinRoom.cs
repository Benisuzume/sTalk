namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست ورود به اتاق گفتگو
    /// </summary>
    public class JoinRoom : Identifier
    {
        /// <summary>شناسه اتاق گفتگو جهت ورود</summary>
        public uint RoomId { get; set; }
    }
}