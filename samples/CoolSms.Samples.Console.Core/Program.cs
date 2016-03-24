namespace CoolSms.Samples.Console.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 옵션 설정.
            // 반드시 CoolSMS의 실제 설정과 동일한 값으로 설정해야 합니다.
            // 그렇지 않으면 SendMessageAsync에서 오류가 발생합니다.
            var client = new SmsApi(new SmsApiOptions
            {
                ApiKey = "{your CoolSMS api key}",
                ApiSecret = "{your CoolSMS api secret}",
                DefaultSenderId = "{your sender id(phonenumber)}",
            });

            // 테스트 전송.
            // 실제로 발송되지는 않고 CoolSMS 서버에 등록되어 전송된 걸로 기록이 나오는 것.
            var result = client.SendTestMessageAsync("가가가가").Result;

            // 실제 전송.
            //var result = client.SendMessageAsync("{recipient number}", "가가가가").Result;

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
