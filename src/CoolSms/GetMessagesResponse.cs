using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolSms
{
    /// <summary>
    /// 조회한 문자 메시지의 목록.
    /// </summary>
    /// <remarks>
    /// 제대로 문서화되어 있지 않아 각 필드의 의미를 정확히 파악하기 어렵습니다.
    /// </remarks>
    /// <see cref="http://www.coolsms.co.kr/SMS_API#GETsent"/>
    public class GetMessagesResponse
    {
        /// <summary>
        /// 조회된 총 개수
        /// </summary>
        [JsonProperty(PropertyName = "total_count")]
        public int Total { get; set; }
        /// <summary>
        /// 페이지당 메시지 수
        /// </summary>
        [JsonProperty(PropertyName = "list_count")]
        public int PageSize { get; set; }
        /// <summary>
        /// 페이지 번호
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }
        /// <summary>
        /// 현재 페이지의 메시지 목록
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<MessageResponse> Messages { get; set; } = Enumerable.Empty<MessageResponse>();

        /// <summary>
        /// 메시지 하나의 정보
        /// </summary>
        public class MessageResponse
        {
            /// <summary>
            /// 메시지의 타입
            /// </summary>
            [JsonProperty(PropertyName = "type")]
            public MessageType Type { get; set; }
            /// <summary>
            /// ???어떤 시각을 말하는건지 확실히 알 수 없음.
            /// 포맷도 정확히 명시하지 않았으나, 예제상으로는 yyyy-MM-dd HH:mm:ss 로 보임.
            /// CoolSMS에 접수된 시각? 통신사에 접수된 시각? 사용자가 수신한 시각?
            /// </summary>
            [JsonProperty(PropertyName = "accepted_time")]
            [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd HH:mm:ss")]
            public DateTime? AcceptedTime { get; set; }
            /// <summary>
            /// 수신자의 전화번호.
            /// </summary>
            [JsonProperty(PropertyName = "recipient_number")]
            public string Recipient { get; set; }
            /// <summary>
            /// 그룹(요청) ID
            /// </summary>
            [JsonProperty(PropertyName = "group_id")]
            public string GroupId { get; set; }
            /// <summary>
            /// 메시지 ID
            /// </summary>
            [JsonProperty(PropertyName = "message_id")]
            public string MessageId { get; set; }
            /// <summary>
            /// 메시지의 전송 상태
            /// </summary>
            [JsonProperty(PropertyName = "status")]
            public MessageStatus Status { get; set; }
            /// <summary>
            /// 메시지의 전송 결과 코드
            /// </summary>
            [JsonProperty(PropertyName = "result_code")]
            public string ResultCode { get; set; }
            /// <summary>
            /// 메시지의 전송 결과 메시지
            /// </summary>
            [JsonProperty(PropertyName = "result_message")]
            public string ResultMessage { get; set; }
            /// <summary>
            /// ???어떤 시각을 말하는건지 확실히 알 수 없음.
            /// 포맷도 정확히 명시하지 않았으나, 예제상으로는 yyyyMMddHHmmss 으로 보임.
            /// CoolSMS에 접수된 시각? 통신사에 접수된 시각? 사용자가 수신한 시각?
            /// </summary>
            [JsonProperty(PropertyName = "sent_time")]
            [JsonConverter(typeof(DateTimeFormatConverter), "yyyyMMddHHmmss")]
            public DateTime? SentTime { get; set; }
            /// <summary>
            /// 메시지 내용
            /// </summary>
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            /// <summary>
            /// 통신사(SKT, LGT, KT 등)
            /// </summary>
            [JsonProperty(PropertyName = "carrier")]
            public string Carrier { get; set; }
            /// <summary>
            /// ??? 의미를 알 수 없는 필드
            /// </summary>
            [JsonProperty(PropertyName = "scheduled_time")]
            public string ScheduledTime { get; set; }
        }
    }
}
