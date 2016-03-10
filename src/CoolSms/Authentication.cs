using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Text;

namespace CoolSms
{
    /// <summary>
    /// API를 사용하기 위한 인증 정보. 매번 새 인스턴스를 얻어 인증에 사용해야 합니다.
    /// </summary>
    /// <remarks>
    /// timestamp(문자열) + salt(문자열) 를 데이터로, api_secret을 키로 사용하여 HMAC(Hash based message authentication code)을 만듭니다.
    /// 서버에서 api_key로 내부DB에서 api_secret을 찾아 클라이언트와 같은 방법으로 signature를 만들어 클라이언트에서 보내온 signature와 비교하여 같으면 인증이 완료됩니다.
    /// api_secret은 signature를 만들 때만 사용하고 서버에 전송하지 않도록 합니다.api_secret이 외부에 노출되지 않도록 주의합니다.
    /// salt를 매번 다른 문자열로 변경하여 항상 signature가 다른 값으로 생성되게 합니다.
    /// 15분 안에 전송되는 Request의 signature 값은 항상 달라야 합니다.
    /// 또한 timestamp값이 현재시간에서 15분을 벗어나면 서버쪽에서 RequestTimeTooSkewed 오류코드를 리턴합니다.
    /// 여기에서는 해시 알고리즘은 sha-1을 사용하며 인코딩은 base64를 고정으로 사용합니다.
    /// </remarks>
    /// <see cref="http://www.coolsms.co.kr/REST_API#Authentication"/>
    public class Authentication
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Authentication(string apiKey, string apiSecret)
            : this(apiKey, 
                  apiSecret, 
                  ((int)Math.Ceiling((DateTime.UtcNow - UnixEpoch).TotalSeconds)).ToString(), 
                  DateTime.UtcNow.Ticks.ToString())
        {
        }
        public Authentication(string apiKey, string apiSecret, string timestamp, string salt)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }
            if (string.IsNullOrEmpty(apiSecret))
            {
                throw new ArgumentNullException(nameof(apiSecret));
            }
            if (string.IsNullOrEmpty(timestamp))
            {
                throw new ArgumentNullException(nameof(timestamp));
            }
            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException(nameof(salt));
            }
            ApiKey = apiKey;
            Timestamp = timestamp;
            Salt = salt;
            Signature = Hash(apiSecret, Timestamp, Salt);
        }

        private static string Hash(string apiSecret, string timestamp, string salt)
        {
            byte[] keyMaterial = UTF8Encoding.UTF8.GetBytes(apiSecret);
            byte[] data = UTF8Encoding.UTF8.GetBytes(timestamp + salt);

            var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha1);
            CryptographicHash hasher = algorithm.CreateHash(keyMaterial);
            hasher.Append(data);
            var mac = hasher.GetValueAndReset();
            return Convert.ToBase64String(mac);
        }

        /// <summary>
        /// 발급받은 API Key 입력
        /// </summary>
        [JsonProperty("api_key")]
        public string ApiKey { get; private set; }
        /// <summary>
        /// UNIX TIME(1970년 1월 1일 0시부터 초 단위로 카운트된 정수값) php에서는 time() 함수호출로 값을 리턴 받을 수 있다
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; private set; }
        /// <summary>
        /// 5~30 바이트 사이의 랜덤으로 만들어진 문자열
        /// </summary>
        [JsonProperty("salt")]
        public string Salt { get; private set; }
        /// <summary>
        /// timestamp + salt를 데이터로 api_secret을 키로 한 HMAC 해시코드
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; private set; }
        /// <summary>
        /// HMAC 생성 알고리즘, 기본은 md5, sha1
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm = "sha1";
        /// <summary>
        /// signature값의 인코딩 방식(hex와 base64 지원, 기본은 hex)
        /// </summary>
        [JsonProperty("encoding")]
        public string Encoding => "base64";
    }
}
