using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace CoolSms
{
    /// <summary>
    /// 쿼리 스트링으로 파라미터를 인코딩하는 요청
    /// </summary>
    public abstract class QueryStringRequest : Request
    {
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
            uriBuilder.Query = string.Join("&", query.Select(q => $"{q.Key}={WebUtility.UrlEncode(q.Value)}"));
            return new HttpRequestMessage(HttpMethod, uriBuilder.Uri);
        }
    }
}
