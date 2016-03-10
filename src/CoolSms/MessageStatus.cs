namespace CoolSms
{
    /// <summary>
    /// 메시지의 전송 상태
    /// </summary>
    public enum MessageStatus
    {
        /// <summary>
        /// 대기중
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 이통사로 전송중
        /// </summary>
        Pending = 1,
        /// <summary>
        /// 이통사로부터 리포트 도착
        /// </summary>
        /// <remarks>
        /// 단순히 모든 처리가 완료되었음을 의미합니다. 
        /// 성공/실패 여부는 개별 result_code를 확인해야 합니다.
        /// </remarks>
        Complete = 2,
    }
}
