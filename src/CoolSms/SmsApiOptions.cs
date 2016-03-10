namespace CoolSms
{
    /// <summary>
    /// SMS API의 설정
    /// </summary>
    /// <see cref="https://www.coolsms.co.kr/index.php?mid=service_setup&act=dispSmsconfigCredentials"/>
    public class SmsApiOptions
    {
        /// <summary>
        /// API Key.
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// API Secret.
        /// </summary>
        public string ApiSecret { get; set; }
        /// <summary>
        /// 기본값으로 사용할 발송자 번호.
        /// </summary>
        public string DefaultSenderId { get; set; }
    }
}
