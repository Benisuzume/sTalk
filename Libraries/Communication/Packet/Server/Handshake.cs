namespace sTalk.Libraries.Communication.Packet.Server
{
    /// <summary>
    /// بسته پاسخ به درخواست آشنایی
    /// <para><see cref="Result.Success"/>: با درخواست آشنایی موافقت شد</para>
    /// <para><see cref="Result.NotFound"/>: نام کاربری در پایگاه داده وجود ندارد</para>
    /// <para><see cref="Result.Banned"/>: حساب کاربری شما از ورود به سرور محروم شده است</para>
    /// </summary>
    public class Handshake : Identifier
    {
        /// <summary>نتیجه درخواست</summary>
        public Result Result { get; set; }

        /// <summary>ژتون درهم سازی گذرواژه</summary>
        public string Token { get; set; }
    }
}