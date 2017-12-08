namespace sTalk.Libraries.Communication.Packet.Client
{
    /// <summary>
    /// بسته درخواست ورود
    /// </summary>
    public class LogIn : Identifier
    {
        /// <summary>حاصل درهم سازی گذرواژه و ژتون</summary>
        public string Hash { get; set; }
    }
}