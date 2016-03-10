using System.Threading.Tasks;

namespace CoolSms
{
    /// <summary>
    /// SmsApi의 일반적인 확장 메서드 모음
    /// </summary>
    public static class SmsApiExtensions
    {
        /// <summary>
        /// 주어진 정보로 SMS 전송을 요청하고 요청한 결과를 반환합니다.
        /// </summary>
        /// <param name="to">숫자로만 이루어진 전화번호, 콤마로 여러 개를 지정할 수 있습니다.</param>
        /// <param name="text">전송할 메시지. 80 바이트를 초과하면 LMS로 전송합니다.</param>
        /// <returns>요청 결과</returns>
        public static async Task<Response<SendMessageResponse>> SendMessageAsync(this SmsApi api, string to, string text)
        {
            return await api.SendMessageAsync(new SendMessageRequest(to, text));
        }

        /// <summary>
        /// 주어진 그룹 ID에 해당하는 전송 기록을 반환합니다.
        /// 발견되지 않았으면 null을 반환합니다.
        /// </summary>
        /// <remarks>
        /// SendMessageAsync로 전송한 직후 이 메서드를 호출하였을 때 전송 기록이 발견되지 않을 수 있습니다.
        /// 임의의 초과 시간을 고려하여 재시도가 필요합니다.
        /// </remarks>
        /// <param name="groupId">전송 요청할 때 받은 전송 그룹 ID</param>
        /// <returns>전송 기록(메시지의 목록)</returns>
        public static async Task<Response<GetMessagesResponse>> GetMessageAsync(this SmsApi api, string groupId)
        {
            return await api.GetMessagesAsync(new GetMessagesRequest
            {
                GroupId = groupId
            });
        }
    }
}
