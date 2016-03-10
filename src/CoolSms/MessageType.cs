namespace CoolSms
{
    /// <summary>
    /// 메시지의 종류
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 80 bytes
        /// </summary>
        SMS,
        /// <summary>
        /// 2000 bytes
        /// </summary>
        LMS,
        /// <summary>
        /// 2000 bytes + image
        /// </summary>
        MMS,
    }
}
