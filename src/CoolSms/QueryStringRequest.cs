using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CoolSms
{
    /// <summary>
    /// 쿼리 스트링으로 파라미터를 인코딩하는 요청
    /// </summary>
    public abstract class QueryStringRequest : Request
    {
        /// <summary>
        /// 주어진 인증 정보로 HTTP 요청 메시지를 작성하여 반환합니다.
        /// </summary>
        /// <param name="authentication">인증 정보</param>
        /// <returns>HTTP 요청 메시지</returns>
        public override HttpRequestMessage GetHttpRequest(Authentication authentication)
        {
            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            var authPayload = JObject.FromObject(authentication);
            var payload = JObject.FromObject(this);
            var query = new Dictionary<string, string>();

            var content = new MultipartFormDataContent();
            foreach (var item in authPayload)
            {
                if (item.Value.Type != JTokenType.Null)
                {
                    query[item.Key] = item.Value.Value<string>();
                }
            }
            foreach (var item in payload)
            {
                if (item.Value.Type != JTokenType.Null)
                {
                    query[item.Key] = item.Value.Value<string>();
                }
            }
            
            var uriBuilder = new UriBuilder(RequestUri);
            uriBuilder.Query = string.Join("&", query.Select(q => $"{q.Key}={UrlEncode(q.Value)}"));
            return new HttpRequestMessage(HttpMethod, uriBuilder.Uri);
        }

        private string UrlEncode(string value)
        {
            return System.Net.WebUtility.UrlEncode(value);
        }
    }
}
