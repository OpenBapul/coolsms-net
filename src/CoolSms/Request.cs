using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace CoolSms
{
    /// <summary>
    /// CoolSMS 요청을 작성하는 기반 클래스.
    /// </summary>
    public abstract class Request : IRequest
    {
        /// <summary>
        /// 이 요청에 해당하는 HTTP Method를 반환합니다.
        /// </summary>
        [JsonIgnore]
        protected abstract HttpMethod HttpMethod { get; }
        /// <summary>
        /// 이 요청의 end-point URI를 반환합니다.
        /// </summary>
        [JsonIgnore]
        protected abstract Uri RequestUri { get; }

        /// <summary>
        /// 주어진 인증 정보로 HTTP 요청 메시지를 작성하여 반환합니다.
        /// </summary>
        /// <param name="authentication">인증 정보</param>
        /// <returns>HTTP 요청 메시지</returns>
        public abstract HttpRequestMessage GetHttpRequest(Authentication authentication);
    }
}
