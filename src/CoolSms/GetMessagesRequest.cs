using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace CoolSms
{
    /// <summary>
    /// 발송된 문자메시지의 목록을 가져옵니다. 
    /// </summary>
    /// <see href="http://www.coolsms.co.kr/SMS_API#GETsent"/>
    public class GetMessagesRequest : QueryStringRequest
    {
        /// <summary>
        /// CoolSMS sent API end-point
        /// </summary>
        public const string ResourceUrl = "https://api.coolsms.co.kr/sms/1.5/sent";

        /// <summary>
        /// HttpMethod
        /// </summary>
        protected override HttpMethod HttpMethod { get; } = HttpMethod.Get;
        /// <summary>
        /// API Uri
        /// </summary>
        protected override Uri RequestUri { get; } = new Uri(ResourceUrl, UriKind.Absolute);

        /// <summary>
        /// 기본값 20이며 20개의 목록을 받을 수 있음. 40입력시 40개의 목록이 리턴
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; } = 20;
        /// <summary>
        /// 1부터 시작하는 페이지값
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; } = 1;
        /// <summary>
        /// 수신번호로 검색
        /// </summary>
        [JsonProperty(PropertyName = "rcpt")]
        public string Recepient { get; set; }
        /// <summary>
        /// 검색 시작일시 접수 날짜와 시간으로 검색 YYYY-MM-DD HH:MI:SS 포맷의 날짜와 시간
        /// </summary>
        /// <remarks>
        /// KST 기준
        /// </remarks>
        [JsonProperty(PropertyName = "start")]
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime? DateTimeFrom { get; set; }
        /// <summary>
        /// 검색 종료일시 접수 날짜와 시간으로 검색 YYYY-MM-DD HH:MI:SS 포맷의 날짜와 시간
        /// </summary>
        /// <remarks>
        /// KST 기준
        /// </remarks>
        [JsonProperty(PropertyName = "end")]
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime? DateTimeTo { get; set; }
        /// <summary>
        /// 메시지 상태 값으로 검색
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public MessageStatus? Status { get; set; }
        /// <summary>
        /// 전송결과 값으로 검색
        /// </summary>
        [JsonProperty(PropertyName = "result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// 입력된 전송결과 값 이외의 건들만 조회
        /// </summary>
        [JsonProperty(PropertyName = "notin_resultcode")]
        public string ExcludeResultCode { get; set; }
        /// <summary>
        /// 메시지ID
        /// </summary>
        [JsonProperty(PropertyName = "mid")]
        public string MessageId { get; set; }
        /// <summary>
        /// 그룹(요청)ID
        /// </summary>
        [JsonProperty(PropertyName = "gid")]
        public string GroupId { get; set; }
    }
}
