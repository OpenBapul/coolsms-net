namespace CoolSms
{
    /// <summary>
    /// CoolSMS 결과 코드
    /// </summary>
    public class ResultCode
    {
        /// <summary>
        /// 주어진 코드에 대한 메시지를 반환합니다.
        /// </summary>
        /// <param name="code">결과 코드</param>
        /// <returns>메시지</returns>
        public static string GetMessage(string code)
        {
            switch (code)
            {
                case "00": return "정상(전송완료)";
                case "10": return "잘못된 번호";
                case "11": return "상위 서비스망 스팸 인식됨";
                case "12": return "상위 서버 오류";
                case "13": return "잘못된 필드값";
                case "20": return "등록된 계정이 아니거나 패스워드가 틀림";
                case "21": return "존재하지 않는 메시지 ID";
                case "30": return "가능한 전송 잔량이 없음";
                case "31": return "전송할 수 없음";
                case "32": return "가입자 없음";
                case "40": return "전송시간 초과";
                case "41": return "단말기 busy";
                case "42": return "음영지역";
                case "43": return "단말기 Power off";
                case "44": return "단말기 메시지 저장갯수 초과";
                case "45": return "단말기 일시 서비스 정지";
                case "46": return "기타 단말기 문제";
                case "47": return "착신거절";
                case "48": return "Unkown error";
                case "49": return "Format Error";
                case "50": return "SMS서비스 불가 단말기";
                case "51": return "착신측의 호불가 상태";
                case "52": return "이통사 서버 운영자 삭제";
                case "53": return "서버 메시지 Que Full";
                case "54": return "SPAM";
                case "55": return "SPAM, nospam.or.kr 에 등록된 번호";
                case "56": return "전송실패(무선망단)";
                case "57": return "전송실패(무선망->단말기단)";
                case "58": return "전송경로 없음";
                case "60": return "예약취소";
                case "70": return "허가되지 않은 IP주소";
                case "81": return "게이트웨이 연결 실패";
                case "82": return "이미지 미입력";
                case "83": return "수신번호 미입력";
                case "84": return "발신번호 미입력";
                case "85": return "메시지 내용 미입력";
                case "86": return "미지원 이미지 타입";
                case "99": return "전송대기";
                default: return "Unkown error";
            }
        }
    }
}
