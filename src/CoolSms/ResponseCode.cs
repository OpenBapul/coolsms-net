namespace CoolSms
{
    /// <summary>
    /// CoolSMS API의 오류 코드
    /// </summary>
    /// <see href="http://www.coolsms.co.kr/REST_API#Response"/>
    public enum ResponseCode
    {
        /// <summary>
        /// 200 OK  성공적으로 수행
        /// </summary>
        OK,
        /// <summary>
        /// 403 Forbidden   유효한 API Key가 아님
        /// </summary>
        InvalidAPIKey,
        /// <summary>
        /// 403 Forbidden   생성한 Signature가 일치하지 않음
        /// </summary>
        SignatureDoesNotMatch,
        /// <summary>
        /// 402 Payment Required    잔액이 부족함
        /// </summary>
        NotEnoughBalance,
        /// <summary>
        /// 400 Bad Request 해당 리소스에 접근가능한 METHOD(POST / GET)가 아님
        /// </summary>
        InvalidMethod,
        /// <summary>
        /// 400 Bad Request 메시지타입은 SMS, LMS, MMS 중 하나여야 함
        /// </summary>
        InvalidMessageType,
        /// <summary>
        /// 404 Not Found   해당 메시지가 없음
        /// </summary>
        NoSuchMessage,
        /// <summary>
        /// 403 Forbidden   지원하지 않는 해시알고리즘 MD5, SHA-1
        /// </summary>
        UnknownAlgorithm,
        /// <summary>
        /// 500 Internal Server Error   서버 내부 오류
        /// </summary>
        InternalError,
        /// <summary>
        /// 404 Not Found   존재하지 않는 리소스에 접근
        /// </summary>
        InvalidResource,
        /// <summary>
        /// 403 Forbidden   timestamp 값이 위 아래로 15분을 벗어남
        /// </summary>
        RequestTimeTooSkewed,
        /// <summary>
        /// 403 Forbidden	15분 안에 동일한 signature 값
        /// </summary>
        DuplicatedSignature,
        /// <summary>
        /// 413 Request Entity Too Large    이미지파일 사이즈 300KB 초과
        /// </summary>
        FileSizeTooBig,
        /// <summary>
        /// 400 Bad Request 이미지 미입력
        /// </summary>
        NoImageInput,
        /// <summary>
        /// 400 Bad Request 메시지내용 미입력
        /// </summary>
        NoMessageInput,
        /// <summary>
        /// 413 Request Entity Too Large    입력된 수신번호가 1000개를 넘음
        /// </summary>
        RecipientsTooMany,
        /// <summary>
        /// 403 Forbidden   지원하지 않는 이미지 포맷
        /// </summary>
        ImageTypeNotSupported,
        /// <summary>
        /// 413 Request Entity Too Large    이미지의 해상도가 너무 큼, 2048 x 2048 이하
        /// </summary>
        ImageResolutionSizeTooBig,
    }
}
