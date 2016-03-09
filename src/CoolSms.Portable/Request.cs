using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        /// 이 요청에 파일이 포함된 경우 이름과 파일 정보가 포함된 StreamContent를 반환합니다.
        /// </summary>
        protected virtual IReadOnlyDictionary<string, StreamContent> StreamContents { get; }
            = new Dictionary<string, StreamContent>();

        public virtual HttpRequestMessage GetHttpRequest(Authentication authentication)
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
