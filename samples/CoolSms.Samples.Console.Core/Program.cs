using Microsoft.Extensions.Configuration;
using System.Text;

namespace CoolSms.Samples.Console.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Platform codepage 지원
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .Build();

            // 옵션 설정.
            // 반드시 CoolSMS의 실제 설정과 동일한 값으로 설정해야 합니다.
            // 그렇지 않으면 SendMessageAsync에서 오류가 발생합니다.
            var client = new SmsApi(new SmsApiOptions
            {
                ApiKey = configuration["coolSms:ApiKey"],
                ApiSecret = configuration["coolSms:ApiSecret"],
                DefaultSenderId = configuration["coolSms:DefaultSenderId"],
            });

            // 테스트 전송.
            // 실제로 발송되지는 않고 CoolSMS 서버에 등록되어 전송된 걸로 기록이 나오는 것.
            var result = client.SendTestMessageAsync("내용 123123, 내용이 80byte를 넘기면 자동으로 LMS로 전송되며 내용중 앞 부분이 자동으로 제목이 됩니다.").Result;

            // 실제 전송.
            //var result = client.SendMessageAsync(
            //    to: "000-000-0000",
            //    text: "내용 123123, 내용이 80byte를 넘기면 자동으로 LMS로 전송되며 내용중 앞 부분이 자동으로 제목이 됩니다.").Result;

            System.Console.WriteLine(
$@"전송 결과:
GroupId: {result.GroupId}
IsSuccess: {result.IsSuccess}
ResultCode: {result.ResultCode}
ResultMessage: {result.ResultMessage}");
            System.Console.ReadKey();
        }
    }
}
