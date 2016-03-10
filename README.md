# coolsms-net
[CoolSMS](http://www.coolsms.co.kr/SMS_API)의 닷넷 클라이언트입니다.

가장 기초적인 문자 메시지 전송과 전송 내역 확인 API만 구현하였습니다.

테스트 케이스가 심히 미흡합니다. 아직 프로덕션 레벨은 아니니 본인의 환경에서 충분히 테스트 바랍니다.

## Status

[![Build status](https://ci.appveyor.com/api/projects/status/vrmer2hympk7f5kd/branch/master?svg=true)](https://ci.appveyor.com/project/incombine/coolsms-net/branch/master)

## QuickStart

### Nuget
> `Install-Package CoolSms`

### API 초기화
CoolSMS 계정 정보를 통해 [Authentication](http://www.coolsms.co.kr/REST_API#Authentication)을 제공합니다.
```CSharp
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
```CSharp
var request = new SendMessageRequest("01000000000", "헬로 메시지");
var result = await api.SendMessageAsync(request);
// 성공적인 경우 잠시후 문자 메시지가 도착
// result는 반드시 null이 아니며 result.GroupId가 설정되어 있어야 함.
// 200 OK 외에는 `ResponseException`예외가 발생
```
`SendMessageAsync()`는 전송을 '요청'하는 것이며 `OK`가 되더라도 실제로 전송에 성공했는지 여부는 `GetMessageAsync()`로 조회를 해야 알 수 있습니다.

다음과 같은 숏컷 확장 메서드도 준비되어 있습니다.
```
var result = await api.SendMessageAsync("01000000000", "헬로 뭐시기");
```

### 문자 메시지의 목록 조회
[GET Sent API](http://www.coolsms.co.kr/SMS_API#GETsent) 기능을 구현합니다.
```CSharp
var request = new GetMessageRequest
{
  GroupId = "{SendMessageResponse.GroupId}"
};
var result = await api.GetMessageAsync(request);
// 마찬가지로 정상적인 경우 null이 아니며,
// 오류가 발생한 경우 예외를 던집니다.
```
`SendMessageAsync()`로 전송한 직후 `GetMessageAsync()`를 해보면 결과가 존재하지 않을 가능성이 있습니다.
따라서 전송 결과를 업데이트할 때에는 임의의 타임아웃 시간을 정해놓고 주기적으로 재시도를 해야합니다.

다음과 같이 `GroupId`에 대한 숏컷 확장 메서드도 준비되어 있습니다.
```
var result = await api.GetMessagesAsync("group-id-in-the-result");
```

### 테스트 전송
`Mode`를 `test`로 설정하여 실제로 통신사를 거쳐서 전송하지 않고 시뮬레이션만 수행합니다. 수신자는 자동으로 `01000000000`로 설정됩니다.
```CSharp
var request = SendMessageRequest.CraeteTest("테스트메시지");
var result = await api.SendMessageAsync(request);
```
또는
```
var result = await api.SendTestMessageAsync(request);
```
테스트 전송도 `GetMessageAsync()`로 조회를 할 수 있습니다.
