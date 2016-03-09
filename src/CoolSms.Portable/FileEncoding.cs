using System.Runtime.Serialization;

namespace CoolSms
{
    /// <summary>
    /// 파일 인코딩
    /// </summary>
    public enum FileEncoding
    {
        /// <summary>
        /// 바이너리
        /// </summary>
        [EnumMember(Value = "binary")]
        Binary,
        /// <summary>
        /// Base64
        /// </summary>
        [EnumMember(Value = "base64")]
        Base64,
    }
}
