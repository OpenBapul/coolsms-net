using System;
using System.Net;

namespace CoolSms
{
    /// <summary>
    /// 응답에서 발생한 예외
    /// </summary>
    public class ResponseException : Exception
    {
        /// <summary>
        /// 주어진 정보로 응답 예외를 초기화합니다.
        /// </summary>
        /// <param name="statusCode">HTTP 응답코드</param>
        /// <param name="responseCode">CoolSMS 오류 코드</param>
        /// <param name="responseMessage">CoolSMS 오류 메시지</param>
        public ResponseException(HttpStatusCode statusCode, ResponseCode responseCode, string responseMessage)
            : base(GetErrorMessage(statusCode, responseCode, responseMessage))
        {
            StatusCode = statusCode;
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
        }

        private static string GetErrorMessage(HttpStatusCode statusCode, ResponseCode responseCode, string responseMessage)
        {
            return $"Unexpected response recieved. HttpStatus:{statusCode}, Code:{responseCode}, Message:{responseMessage}";
        }

        /// <summary>
        /// 실제 HTTP 응답의 상태 코드
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }
        /// <summary>
        /// CoolSMS의 응답 코드
        /// </summary>
        public ResponseCode ResponseCode { get; private set; }
        /// <summary>
        /// CoolSMS의 메시지 내용
        /// </summary>
        public string ResponseMessage { get; private set; }
    }
}
