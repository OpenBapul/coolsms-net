using Newtonsoft.Json;

namespace CoolSms
{
    /// <summary>
    /// 전송 접수 결과.
    /// 실제 SMS 전송 여부와 무관한 전송 요청의 접수 결과를 의미합니다.
    /// </summary>
    /// <see href="http://www.coolsms.co.kr/SMS_API#Response"/>
    public class SendMessageResponse
    {
        /// <summary>
        /// 전송 접수 결과를 초기화합니다.
        /// </summary>
        /// <param name="groupId">그룹 ID</param>
        /// <param name="successCount">전송한 개수</param>
        /// <param name="errorCount">오류 개수</param>
        /// <param name="resultCode">결과 코드</param>
        /// <param name="resultMessage">결과 메시지</param>
        public SendMessageResponse(
            string groupId, 
            int successCount, 
            int errorCount, 
            string resultCode, 
            string resultMessage)
        {
            GroupId = groupId;
            SuccessCount = successCount;
            ErrorCount = errorCount;
            ResultCode = resultCode;
            ResultMessage = resultMessage;
        }

        /// <summary>
        /// 전송 요청에 대한 그룹 ID
        /// </summary>
        [JsonProperty(PropertyName = "group_id")]
        public string GroupId { get; private set; }
        /// <summary>
        /// 요청한 수신자 중 등록에 성공한 개수
        /// </summary>
        [JsonProperty(PropertyName = "success_count")]
        public int SuccessCount { get; private set; }
        /// <summary>
        /// 요청한 수신자 중 등록에 실패한 개수
        /// </summary>
        [JsonProperty(PropertyName = "error_count")]
        public int ErrorCount { get; private set; }
        /// <summary>
        /// 등록 결과
        /// </summary>
        [JsonProperty(PropertyName = "result_code")]
        public string ResultCode { get; private set; }
        /// <summary>
        /// 등록 결과 메시지
        /// </summary>
        [JsonProperty(PropertyName = "result_message")]
        public string ResultMessage { get; private set; }
        /// <summary>
        /// 등록에 성공했는지 여부
        /// </summary>
        public bool IsSuccess => ResultCode == "00";
    }
}
