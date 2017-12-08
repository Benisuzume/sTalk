namespace sTalk.Libraries.Communication.Packet.Data
{
    /// <summary>
    /// اتاق گفتگو
    /// </summary>
    public class Room
    {
        /// <summary>شناسه اتاق گفتگو</summary>
        public uint Id { get; set; }

        /// <summary>نام اتاق گفتگو</summary>
        public string Name { get; set; }

        /// <summary>ظرفیت اتاق گفتگو</summary>
        public ushort Capacity { get; set; }

        /// <summary>پیام سیستمی اتاق گفتگو</summary>
        public string Message { get; set; }
    }
}