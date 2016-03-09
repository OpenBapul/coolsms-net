using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoolSms
{
    /// <summary>
    /// CoolSMS의 SMS API 클라이언트
    /// </summary>
    public class SmsApi : IDisposable
    {
        private readonly HttpClient client;
        private readonly SmsApiOptions options;

        /// <summary>
        /// 주어진 옵션으로 API 클라이언트를 초기화합니다.
        /// </summary>
        /// <remarks>
        /// ApiKey와 ApiSecret을 반드시 설정해야 합니다.
        /// </remarks>
        /// <param name="options">SMS API 옵션</param>
        public SmsApi(SmsApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                throw new ArgumentException("ApiKey cannot be null or empty.", nameof(options));
            }
            if (string.IsNullOrEmpty(options.ApiSecret))
            {
                throw new ArgumentException("ApiSecret cannot be null or empty.", nameof(options));
            }

            client = new HttpClient();
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.ExpectContinue = false;
            this.options = options;
        }

        /// <summary>
        /// 현재 클라이언트의 옵션을 반환합니다.
        /// </summary>
        protected SmsApiOptions Options { get { return options; } }

        /// <summary>
        /// 현재 클라이언트에 대한 인증 정보를 반환합니다.
        /// </summary>
        /// <returns>인증 정보</returns>
        protected virtual Authentication GetAuthentication()
            => new Authentication(options.ApiKey, options.ApiSecret);

        /// <summary>
        /// 주어진 요청을 전송하고 응답을 지정한 형식으로 반환합니다.
        /// </summary>
        /// <typeparam name="TResponse">응답 형식</typeparam>
        /// <param name="request">요청</param>
        /// <returns>응답 결과</returns>
        /// <exception cref="HttpResponseException">
        /// CoolSMS에서 200 OK외의 응답 코드를 수신하였을 때 발생합니다.
        /// </exception>
        public virtual async Task<Response<TResponse>> RequestAsync<TResponse>(IRequest request)
            where TResponse : class
        {
            var message = request.GetHttpRequest(GetAuthentication());
            var response = await client.SendAsync(message);
            var json = await response.Content.ReadAsStringAsync();

            var result = new Response<TResponse>
            {
                StatusCode = response.StatusCode,
            };

            if (response.StatusCode == HttpStatusCode.OK)
            {
                result.Result = JsonConvert.DeserializeObject<TResponse>(json);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                result.Code = error.Code;
            }
            return result;
        }

        /// <summary>
        /// 주어진 정보로 문자 메시지 전송을 요청하고 결과를 반환합니다.
        /// </summary>
        /// <param name="request">문자 메시지 전송 요청 정보</param>
        /// <returns>전송 요청 결과</returns>
        public async Task<Response<SendMessageResponse>> SendMessageAsync(SendMessageRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (string.IsNullOrEmpty(request.From))
            {
                request.From = options.DefaultSenderId;
            }
            return await RequestAsync<SendMessageResponse>(request);
        }

        private class GetState
        {
            public GetMessagesResponse Response { get; set; }
        }

        /// <summary>
        /// 주어진 정보에 해당하는 문자 메시지의 목록 정보를 반환합니다.
        /// </summary>
        /// <param name="request">문자 메시지 조회 조건</param>
        /// <returns>문자 메시지의 목록 정보</returns>
        public async Task<Response<GetMessagesResponse>> GetMessagesAsync(GetMessagesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return await RequestAsync<GetMessagesResponse>(request);
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public class ErrorResponse
        {
            [JsonProperty("code")]
            [JsonConverter(typeof(StringEnumConverter))]
            public ResponseCode Code { get; set; }
        }
    }
}
