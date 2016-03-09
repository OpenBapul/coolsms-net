# coolsms-net
[CoolSMS](http://www.coolsms.co.kr/SMS_API)의 닷넷 클라이언트입니다.

가장 기초적인 문자 메시지 전송과 전송 내역 확인 API만 구현하였습니다.

## QuickStart

### API 초기화
CoolSMS 계정 정보를 통해 [Authentication](http://www.coolsms.co.kr/REST_API#Authentication)을 제공합니다.
```
var api = new SmsApi(new SmsApiOptions
{
    ApiKey = "{your API Key}",
    ApiSecret = "{your API Secret}",
    DefaultSenderId = "{your Sender Id(PhoneNumber)}" // 국내 SMS발신의 경우 필수
});
```
API 클라이언트는 동일한 설정일 경우 하나의 인스턴스를 전역에서 공유해도 문제가 없습니다.


### 문자 메시지 전송
[POST Send API](http://www.coolsms.co.kr/SMS_API#POSTsend) 기능을 구현합니다.
```
var request = new SendMessageRequest("01000000000", "헬로 메시지");
var response = await api.SendMessageAsync(request);
if (response.Code == ResponseCode.OK)
{
  // 성공적인 경우 잠시후 문자 메시지가 도착
}
```
`SendMessageAsync()`는 전송을 '요청'하는 것이며 `OK`가 되더라도 실제로 전송에 성공했는지 여부는 `GetMessageAsync()`로 조회를 해야 알 수 있습니다.

### 문자 메시지의 목록 조회
[GET Sent API](http://www.coolsms.co.kr/SMS_API#GETsent) 기능을 구현합니다.
```
var request = new GetMessageRequest
{
  GroupId = "{SendMessageResponse.GroupId}"
};
var response = await api.GetMessageAsync(request);
if (response.Code == ResponseCode.OK)
{
  // 조회 조건에 해당하는 메시지가 존재함
}
else if (response.Code == ResponseCode.NoSuchMessage)
{
  // 조회 조건에 해당하는 메시지가 하나도 없음(오류가 아님)
}
else
{
  // 오류 처리.
}
```
