using System.Linq;

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

    /// <summary>
    /// 메시지 타입 유틸리티
    /// </summary>
    public static class MessageTypeUtils
    {
        /// <summary>
        /// 주어진 텍스트의 문자메시지 길이를 반환합니다.
        /// </summary>
        /// <param name="text">텍스트</param>
        /// <returns>문자메시지의 길이</returns>
        public static int GetSmsTextLength(string text)
            => text.Select(c => (int)c).Select(c => c > 127 ? 2 : 1).Sum();
        /// <summary>
        /// 주어진 텍스트의 문자메시지 길이에 따라 SMS/LMS를 반환합니다.
        /// </summary>
        /// <param name="text">텍스트</param>
        /// <returns>80보다 길면 LMS, 그렇지 않으면 SMS</returns>
        public static MessageType GetMessageType(string text)
            => GetSmsTextLength(text) > 80 ? MessageType.LMS : MessageType.SMS;
    }
}
