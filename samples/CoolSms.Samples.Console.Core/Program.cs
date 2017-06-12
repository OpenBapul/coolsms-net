using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using System.Threading.Tasks;
using System.IO;

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
            var recipient = "0000";
            while (true)
            {
                recipient = RunAsync(configuration, recipient).Result;
            }
        }

        private static async Task<string> RunAsync(IConfiguration configuration, string recipient)
        {
            // 전화번호 입력
            // 테스트 전송: 실제로 발송되지는 않고 CoolSMS 서버에 등록되어 전송된 걸로 기록이 나오는 것.
            System.Console.WriteLine($"전송할 전화번호 (Enter: {recipient}) ");
            var phoneNumber = System.Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = recipient;
            }

            // 옵션 설정.
            // 반드시 CoolSMS의 실제 설정과 동일한 값으로 설정해야 합니다.
            // 그렇지 않으면 SendMessageAsync에서 오류가 발생합니다.
            var client = new SmsApi(new SmsApiOptions
            {
                ApiKey = configuration["coolSms:ApiKey"],
                ApiSecret = configuration["coolSms:ApiSecret"],
                DefaultSenderId = configuration["coolSms:DefaultSenderId"],
            });

            var text = "내용 123123, 내용이 80byte를 넘기면 자동으로 LMS로 전송되며 내용중 앞 부분이 자동으로 제목이 됩니다. 이미지를 첨부하면 자동으로 MMS로 전환됩니다.";
            var request = new SendMessageRequest(phoneNumber, text)
            {
                // 이미지를 첨부하면 자동으로 MMS로 전환됩니다.
                ImageFile = new MemoryStream(File.ReadAllBytes("image.jpg"))
            };

            var result = phoneNumber.Equals("0000")
                ? await client.SendTestMessageAsync(text)
                : await client.SendMessageAsync(request);

            System.Console.WriteLine(
$@"전송 결과:
GroupId: {result.GroupId}
IsSuccess: {result.IsSuccess}
ResultCode: {result.ResultCode}
ResultMessage: {result.ResultMessage}");
            System.Console.ReadKey();
            return phoneNumber;
        }
    }
}
