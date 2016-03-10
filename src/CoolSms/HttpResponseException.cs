using System;
using System.Net;

namespace CoolSms
{
    /// <summary>
    /// HTTP 응답 예외
    /// </summary>
    public class HttpResponseException : Exception
    {
        public HttpResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
