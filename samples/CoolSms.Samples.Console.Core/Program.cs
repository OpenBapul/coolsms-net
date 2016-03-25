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
                ApiKey = "NCS56DE317B1631F",
                ApiSecret = "6B50864BA7AFCD707BB4292FD1685C9E",
                DefaultSenderId = "025189909",
            });

            // 테스트 전송.
            // 실제로 발송되지는 않고 CoolSMS 서버에 등록되어 전송된 걸로 기록이 나오는 것.
            //var result = client.SendTestMessageAsync("[바풀공부방]입금안내\n상품명:테스트\n금액:1000\n은행:기업은행\n가상계좌:123412341234\n예금주:주식회사바풀").Result;

            // 실제 전송.
            var result = client.SendMessageAsync("01091678130", "입금안내\n상품명:테스트\n금액:1000\n은행:기업은행\n가상계좌:123412341234\n예금주:주식회사바풀").Result;

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
