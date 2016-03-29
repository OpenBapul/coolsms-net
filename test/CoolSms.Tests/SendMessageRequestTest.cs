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

        [Theory]
        [InlineData("a", MessageType.SMS)]
        [InlineData("가", MessageType.SMS)]
        [InlineData("abcd가나다라!@#$", MessageType.SMS)]
        [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901", MessageType.LMS)]
        public void Type_should_be_set(string text, MessageType expected)
        {
            var request = new SendMessageRequest("1234", text);
            Assert.Equal(expected, request.Type);
        }

        [Fact]
        public void Type_can_be_changed()
        {
            var request = new SendMessageRequest("1234", "1234");
            request.Type = MessageType.LMS;
            Assert.Equal(MessageType.LMS, request.Type);
        }
    }
}
