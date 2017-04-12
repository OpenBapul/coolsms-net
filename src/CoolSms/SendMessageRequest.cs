using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;

namespace CoolSms
{
    /// <summary>
    /// SMS 전송 요청
    /// </summary>
    /// <remarks>
    /// 인코딩은 항상 utf8을 사용합니다.
    /// </remarks>
    /// <see href="http://www.coolsms.co.kr/SMS_API#Parameters"/>
    public class SendMessageRequest : MultipartFormDataRequest
    {
        /// <summary>
        /// 요청 주소(v1.5)
        /// </summary>
        public const string ResourceUrl = "https://api.coolsms.co.kr/sms/1.5/send";
        /// <summary>
        /// 테스트시 수신 번호
        /// </summary>
        public const string TestRecipient = "01000000000";

        private static readonly int MaximumTextBytes = 2000;
        private static readonly int MaximumRecepients = 1000;
        private static readonly char[] CommaSeparator = new[] { ',' };

        /// <summary>
        /// 테스트 모드 요청을 반환합니다.
        /// </summary>
        /// <param name="text">전송할 텍스트</param>
        /// <returns>메시지 전송 요청</returns>
        public static SendMessageRequest CraeteTest(string text)
        {
            var request = new SendMessageRequest(TestRecipient, text);
            request.IsTestMode = true;
            return request;
        }

        /// <summary>
        /// 주어진 정보로 초기화합니다.
        /// </summary>
        /// <param name="to">받는 사람 전화번호(, 구분 문자열 허용)</param>
        /// <param name="text">내용(80바이트 이상은 LMS)</param>
        public SendMessageRequest(string to, string text) : this(to, text, null)
        {
        }
        /// <summary>
        /// 주어진 정보로 초기화합니다.
        /// </summary>
        /// <param name="to">받는 사람 전화번호(, 구분 문자열 허용)</param>
        /// <param name="text">내용(80바이트 이상은 LMS)</param>
        /// <param name="from">보내는 사람 전화번호(반드시 발신자 등록이 되어 있어야 함)</param>
        public SendMessageRequest(string to, string text, string from)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException(nameof(to));
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (MessageTypeUtils.GetSmsTextLength(Text) > MaximumTextBytes)
            {
                throw new ArgumentOutOfRangeException(nameof(text), $"text should not be longer than {MaximumTextBytes} bytes.");
            }
            var recipeients = to.Split(CommaSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (recipeients.Length < 1 || recipeients.Length > MaximumRecepients)
            {
                throw new ArgumentOutOfRangeException(nameof(to), $"recepients should not be more than {MaximumRecepients}.");
            }

            To = to;
            Text = text;
            From = from;
            Type = string.IsNullOrEmpty(Text)
                ? MessageType.SMS
                : MessageTypeUtils.GetMessageType(Text);
        }

        /// <summary>
        /// HTTP 메서드
        /// </summary>
        protected override HttpMethod HttpMethod { get; } = HttpMethod.Post;
        /// <summary>
        /// CoolSMS send API end-point
        /// </summary>
        protected override Uri RequestUri { get; } = new Uri(ResourceUrl, UriKind.Absolute);
        /// <summary>
        /// 이미지 첨부 스트림
        /// </summary>
        protected override IReadOnlyDictionary<string, StreamContent> StreamContents
        {
            get
            {
                var value = new Dictionary<string, StreamContent>();
                foreach (var item in base.StreamContents)
                {
                    value.Add(item.Key, item.Value);
                }
                if (ImageFile != null)
                {
                    value.Add("image", new StreamContent(ImageFile));
                }
                return value;
            }
        }

        /// <summary>
        /// 수신번호 입력 콤마(,)로 구분된 수신번호 입력가능 예) 01012345678,01023456789,01034567890
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public string To { get; private set; }
        /// <summary>
        /// 발신번호 예) 0212345678
        /// 2015/10/16 발신번호 사전등록제에 의해 반드시 등록된 번호만 허용됩니다. (해외문자 제외)
        /// </summary>
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }
        /// <summary>
        /// 문자내용
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; private set; }
        /// <summary>
        /// SMS(80바이트), LMS(장문 2,000바이트), MMS(장문+이미지) 입력 없으면 SMS가 기본 국가코드가 KR이 아니면 SMS로 강제
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }
        /// <summary>
        /// 지원형식 : 300KB 이하의 JPEG, PNG, GIF 형식의 파일 2048x2048 픽셀이하
        /// </summary>
        [JsonIgnore]
        [JsonProperty(PropertyName = "image")]
        public Stream ImageFile { get; set; }
        /// <summary>
        /// 지원형식 : 300KB 이하의 JPEG, PNG, GIF 형식의 파일 2048x2048 픽셀이하
        /// </summary>
        [JsonProperty(PropertyName = "image_encoding")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FileEncoding ImageFileEncoding { get; set; }
        /// <summary>
        /// 참조내용(이름)
        /// </summary>
        [JsonProperty(PropertyName = "refname")]
        public string ReferenceName { get; set; }
        /// <summary>
        /// 한국: 82, 일본: 81, 중국: 86, 미국: 1, 기타 등등(기본 한국)
        /// </summary>
        /// <see href="http://countrycode.org"/> 참고
        [JsonProperty(PropertyName = "country")]
        public string CountryCode { get; set; }
        /// <summary>
        /// 예약시간을 YYYYMMDDHHMISS 포맷으로 입력(입력 없거나 지난날짜를 입력하면 바로 전송) 예) 20131216090510 (2013년 12월 16일 9시 5분 10초에 발송되도록 예약)
        /// </summary>
        /// <remarks>
        /// KST 기준
        /// </remarks>
        [JsonProperty(PropertyName = "datetime")]
        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime? SendAt { get; set; }
        /// <summary>
        /// LMS, MMS 일때 제목(40바이트)
        /// 만약 LMS, MMS일 때 지정되지 않을 경우 본문중 40바이트가 제목으로 사용됩니다.
        /// </summary>
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
        /// <summary>
        /// 솔루션 제공 수수료를 정산받을 솔루션 등록키
        /// </summary>
        [JsonProperty(PropertyName = "srk")]
        public string SolutionRegistrationKey { get; set; }
        /// <summary>
        /// test로 입력할 경우 CARRIER 시뮬레이터로 시뮬레이션됨 수신번호를 반드시 01000000000 으로 테스트하세요.예약필드 datetime는 무시됨 결과값은 60 잔액에서 실제 차감되며 다음날 새벽에 재충전됨
        /// </summary>
        [JsonProperty(PropertyName = "mode")]
        public string Mode => IsTestMode ? "test" : null;
        /// <summary>
        /// 테스트 모드인지 여부
        /// </summary>
        [JsonIgnore]
        public bool IsTestMode { get; set; }
        /// <summary>
        /// JSON 포맷의 개별 메시지를 담을 수 있음
        /// </summary>
        [JsonProperty(PropertyName = "extension")]
        public string ExtensionJson => JsonConvert.SerializeObject(Extensions);
        /// <summary>
        /// 확장 정보
        /// </summary>
        [JsonIgnore]
        public IEnumerable<SendMessageExtension> Extensions { get; set; }
        /// <summary>
        /// 0~20 사이의 값으로 전송지연 시간을 줄 수 있음, 1은 약 1초(기본값은 0)
        /// </summary>
        [JsonProperty(PropertyName = "delay")]
        public int Delay { get; set; }
        /// <summary>
        /// 누리고푸시를 사용하더라도 SMS로 발송되도록 강제
        /// </summary>
        [JsonProperty(PropertyName = "force_sms")]
        public bool ForceSms { get; set; }
        /// <summary>
        /// 클라이언트의 OS 및 플랫폼 버전) CentOS 6.6
        /// </summary>
        [JsonProperty(PropertyName = "os_platform")]
        public string OsPlatform { get; set; }
        /// <summary>
        /// 개발 프로그래밍 언어 예) PHP 5.3.3
        /// </summary>
        [JsonProperty(PropertyName = "dev_lang")]
        public string DevelopmentLanguage { get; set; }
        /// <summary>
        /// SDK 버전 예) PHP SDK 1.5
        /// </summary>
        [JsonProperty(PropertyName = "sdk_version")]
        public string SdkVersion { get; set; }
        /// <summary>
        /// 어플리케이션 버전 예) Purplebook 4.1
        /// </summary>
        [JsonProperty(PropertyName = "app_version")]
        public string AppVersion { get; set; }


        /// <summary>
        /// 메시지 전송 확장 포맷
        /// </summary>
        /// <remarks>
        /// type, country, to, from, datetime, text, subject, delay 가 올 수 있고 입력하지 않는 필드는 상위(기본 필드) 값을 상속 받아 사용됩니다. 단, to는 상속받지 아니하고 필수 입력 사항입니다.
        /// </remarks>
        /// <see href="http://www.coolsms.co.kr/SMS_API#Parameters"/>
        public class SendMessageExtension
        {
            /// <summary>
            /// 메시지 전송 확장 포맷을 초기화합니다.
            /// </summary>
            /// <param name="to">받는 사람 전화번호</param>
            public SendMessageExtension(string to) : this(to, null) { }
            /// <summary>
            /// 메시지 전송 확장 포맷을 초기화합니다.
            /// </summary>
            /// <param name="to">받는 사람 전화번호</param>
            /// <param name="text">보낼 내용</param>
            public SendMessageExtension(string to, string text)
            {
                if (string.IsNullOrEmpty(to))
                {
                    throw new ArgumentNullException(nameof(to));
                }
                if (string.IsNullOrEmpty(text) == false
                    && MessageTypeUtils.GetSmsTextLength(text) > MaximumTextBytes)
                {
                    throw new ArgumentOutOfRangeException(nameof(text), $"text should not be longer than {MaximumTextBytes} bytes.");
                }
                To = to;
                Text = text;
                Type = string.IsNullOrEmpty(Text)
                ? (MessageType?)null
                : MessageTypeUtils.GetMessageType(Text);
            }

            /// <summary>
            /// 수신번호 입력 콤마(,)로 구분된 수신번호 입력가능 예) 01012345678,01023456789,01034567890
            /// </summary>
            [JsonProperty(PropertyName = "to")]
            public string To { get; private set; }
            /// <summary>
            /// 발신번호 예) 0212345678
            /// 2015/10/16 발신번호 사전등록제에 의해 반드시 등록된 번호만 허용됩니다. (해외문자 제외)
            /// </summary>
            [JsonProperty(PropertyName = "from")]
            public string From { get; set; }
            /// <summary>
            /// 문자내용
            /// </summary>
            [JsonProperty(PropertyName = "text")]
            public string Text { get; private set; }
            /// <summary>
            /// SMS(80바이트), LMS(장문 2,000바이트), MMS(장문+이미지) 입력 없으면 SMS가 기본 국가코드가 KR이 아니면 SMS로 강제
            /// </summary>
            [JsonProperty(PropertyName = "type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public MessageType? Type { get; set; }
            /// <summary>
            /// 한국: 82, 일본: 81, 중국: 86, 미국: 1, 기타 등등(기본 한국)
            /// </summary>
            /// <see href="http://countrycode.org"/> 참고
            [JsonProperty(PropertyName = "country")]
            public string CountryCode { get; set; } = "82";
            /// <summary>
            /// 예약시간을 YYYYMMDDHHMISS 포맷으로 입력(입력 없거나 지난날짜를 입력하면 바로 전송) 예) 20131216090510 (2013년 12월 16일 9시 5분 10초에 발송되도록 예약)
            /// </summary>
            /// <remarks>
            /// KST 기준
            /// </remarks>
            [JsonProperty(PropertyName = "datetime")]
            [JsonConverter(typeof(DateTimeFormatConverter))]
            public DateTime? SendAt { get; set; }
            /// <summary>
            /// LMS, MMS 일때 제목(40바이트)
            /// </summary>
            [JsonProperty(PropertyName = "subject")]
            public string Subject { get; set; }
        }
    }
}
