using System.Net.Http;

namespace CoolSms
{
    /// <summary>
    /// 요청 인터페이스
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// 주어진 인증 정보로 HTTP 요청 메시지를 작성하여 반환합니다.
        /// </summary>
        /// <param name="authentication">인증 정보</param>
        /// <returns>HTTP 요청 메시지</returns>
        HttpRequestMessage GetHttpRequest(Authentication authentication);
    }
}
