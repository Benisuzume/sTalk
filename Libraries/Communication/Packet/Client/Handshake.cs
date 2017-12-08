namespace sTalk.Libraries.Communication.Packet.Client
{
    public class Handshake : Identifier
    {
        /// <summary>کلید رمزنگاری</summary>
        public byte[] Key { get; set; }

        /// <summary>بردار اولیه</summary>
        public byte[] IV { get; set; }

        /// <summary>نام کاربری</summary>
        public string Username { get; set; }
    }
}