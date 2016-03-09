using CoolSms;
using System.Net.Http;
using Xunit;

namespace CoolSmsTests
{
    public class SendMessageRequestTest
    {
        [Fact]
        public void Method_should_be_Post()
        {
            var sut = new SendMessageRequest("01191678130", "test테스트");
            var auth = new Authentication("abc", "def");
            var actual = sut.GetHttpRequest(auth);
            Assert.Equal(HttpMethod.Post, actual.Method);
        }

        [Fact]
        public void Uri_should_be_send_uri()
        {
            var sut = new SendMessageRequest("01191678130", "test테스트");
            var auth = new Authentication("abc", "def");
            var actual = sut.GetHttpRequest(auth);
            Assert.Equal(SendMessageRequest.ResourceUrl, actual.RequestUri.ToString());
        }
    }
}
