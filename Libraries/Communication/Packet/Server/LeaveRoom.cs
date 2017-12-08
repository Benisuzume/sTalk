namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست خروج از اتاق گفتگو
    /// <para><see cref="Result.Success"/>: از اتاق گفتگو خارج شدید</para>
    /// <para><see cref="Result.Useless"/>: در هیچ اتاق گفتگویی نیستید</para>
    /// </summary>
    public class LeaveRoom : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }
    }
}