using CoolSms;
using System.Threading.Tasks;
using Xunit;

namespace CoolSmsTests
{
    public class SmsApiIntegratedTest
    {
        private const string ApiKey = "{your API Key}";
        private const string ApiSecret = "{your API Secret}";
        private const string DefaultSenderId = "{your Sender Id(PhoneNumber)}";

        private SmsApi GetSut()
        {
            return new SmsApi(new SmsApiOptions
            {
                ApiKey = ApiKey,
                ApiSecret = ApiSecret,
                DefaultSenderId = DefaultSenderId
            });
        }

        //[Fact]
        public async Task Test_mode()
        {
            var sut = GetSut();
            var request = SendMessageRequest.CraeteTest("공부해라. 두 번 해라. abcd가나다あえい");
            var result = await sut.SendMessageAsync(request);
            Assert.NotNull(result.GroupId);
        }
        
        //[Fact]
        public async Task Test_get()
        {
            var sut = GetSut();
            var sendRequest = SendMessageRequest.CraeteTest("공부해라. 두 번 해라. abcd가나다あえい");
            var sendResult = await sut.SendMessageAsync(sendRequest);

            var getRequest = new GetMessagesRequest
            {
                GroupId = sendResult.GroupId
            };
            var getResult = await sut.GetMessagesAsync(getRequest);
            // send가 정상적으로 등록되었어도 곧바로 조회하면 404 Not Found를 반환할 가능성이 있음.
            // 일관성 있는 테스트 불가능.
            Assert.NotNull(getResult);
        }
    }
}
