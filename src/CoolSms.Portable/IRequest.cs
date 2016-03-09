using System.Net.Http;

namespace CoolSms
{
    /// <summary>
    /// 요청 인터페이스
    /// </summary>
    public interface IRequest
    {
        HttpRequestMessage GetHttpRequest(Authentication authentication);
    }
}
