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
        /// <exception cref="ResponseException">
        /// CoolSMS에서 200 OK 또는 404 Not Found외의 응답 코드를 수신하였을 때 발생합니다.
        /// </exception>
        /// <remarks>
        /// 일반적으로 HTTP에서 404 Not Found는 해당 엔드포인트의 리소스가 존재하지 않을 때를 말합니다.
        /// 그러나 CoolSMS API에서는 목록이 비어 있어도 404가 나오므로 이것은 정상으로 처리하고 null을 반환합니다.
        /// </remarks>
        public virtual async Task<TResponse> RequestAsync<TResponse>(IRequest request)
            where TResponse : class
        {
            var message = request.GetHttpRequest(GetAuthentication());
            var response = await client.SendAsync(message);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TResponse>(json);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                if (error.Code != ResponseCode.NoSuchMessage)
                {
                    throw new ResponseException(response.StatusCode, error.Code, error.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// 주어진 정보로 문자 메시지 전송을 요청하고 결과를 반환합니다.
        /// </summary>
        /// <param name="request">문자 메시지 전송 요청 결과</param>
        /// <returns>전송 요청 결과</returns>
        public async Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request)
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
        
        /// <summary>
        /// 주어진 정보에 해당하는 문자 메시지의 조회 결과를 반환합니다.
        /// </summary>
        /// <param name="request">문자 메시지 조회 조건</param>
        /// <returns>문자 메시지의 조회 결과</returns>
        /// <remarks>
        /// 결과가 없더라도 빈 목록을 포함하는 정보를 반환합니다.
        /// </remarks>
        public async Task<GetMessagesResponse> GetMessagesAsync(GetMessagesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return (await RequestAsync<GetMessagesResponse>(request))
                ?? new GetMessagesResponse
                {
                    PageSize = request.Count,
                    Page = request.Page,
                };
        }

        /// <summary>
        /// 리소스를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            client.Dispose();
        }

        /// <summary>
        /// 오류 응답 정보
        /// </summary>
        public class ErrorResponse
        {
            /// <summary>
            /// 오류 응답 코드
            /// </summary>
            [JsonProperty("code")]
            [JsonConverter(typeof(StringEnumConverter))]
            public ResponseCode Code { get; set; }
            /// <summary>
            /// 오류 메시지
            /// </summary>
            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
}
