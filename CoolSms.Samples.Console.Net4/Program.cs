namespace CoolSms.Samples.Console.Net4
{
    class Program
    {
        static void Main(string[] args)
        {
            // 옵션 설정.
            // 반드시 CoolSMS의 실제 설정과 동일한 값으로 설정해야 합니다.
            // 그렇지 않으면 SendMessageAsync에서 오류가 발생합니다.
            var client = new SmsApi(new SmsApiOptions
            {
                ApiKey = "{api key}",
                ApiSecret = "{api secret}",
                DefaultSenderId = "{default sender phonenumber}",
            });

            // 테스트 전송.
            // 실제로 발송되지는 않고 CoolSMS 서버에 등록되어 전송된 걸로 기록이 나오는 것.
            //var result = client.SendTestMessageAsync("테스트 메시지").Result;

            // 실제 전송.
            var result = client.SendMessageAsync("{phonenumber}", "테스트 메시지").Result;

            System.Console.WriteLine(
$@"result>
GroupId: {result.GroupId}
IsSuccess: {result.IsSuccess}
ResultCode: {result.ResultCode}
ResultMessage: {result.ResultMessage}");
            System.Console.ReadKey();
        }
    }
}
