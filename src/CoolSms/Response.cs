using System.Net;

namespace CoolSms
{
    /// <summary>
    /// CoolSMS의 표준 응답 모델
    /// </summary>
    /// <typeparam name="T">정상적인 응답일 때의 결과 형식</typeparam>
    public class Response<T> where T : class
    {
        /// <summary>
        /// HTTP 상태 코드
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 응답 코드
        /// </summary>
        public ResponseCode Code { get; set; }
        /// <summary>
        /// 정상적인 응답일 때의 결과. 오류가 있는 경우 null입니다.
        /// </summary>
        public T Result { get; set; }
    }
}
