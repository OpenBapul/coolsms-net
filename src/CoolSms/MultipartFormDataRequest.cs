using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CoolSms
{
    /// <summary>
    /// multipart/form-data로 인코딩하는 요청
    /// </summary>
    public abstract class MultipartFormDataRequest : Request
    {
        /// <summary>
        /// 이 요청에 파일이 포함된 경우 이름과 파일 정보가 포함된 StreamContent를 반환합니다.
        /// </summary>
#if NET40
        protected virtual IDictionary<string, StreamContent> StreamContents { get; }
            = new Dictionary<string, StreamContent>();
#else
        protected virtual IReadOnlyDictionary<string, StreamContent> StreamContents { get; }
            = new Dictionary<string, StreamContent>();
#endif

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

            var message = new HttpRequestMessage(HttpMethod, RequestUri);
            var content = new MultipartFormDataContent();
            foreach (var item in authPayload)
            {
                if (item.Value.Type != JTokenType.Null)
                {
                    content.Add(new StringContent(item.Value.Value<string>(), Encoding.UTF8), item.Key);
                }
            }
            foreach (var item in payload)
            {
                if (item.Value.Type != JTokenType.Null)
                {
                    content.Add(new StringContent(item.Value.Value<string>(), Encoding.UTF8), item.Key);
                }
            }
            foreach (var item in StreamContents)
            {
                content.Add(item.Value, item.Key);
            }
            message.Content = content;
            return message;
        }
    }
}
