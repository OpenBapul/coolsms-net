using Xunit;

namespace CoolSmsTests
{
    public class AuthenticationTest
    {
        [Fact]
        public void Should_produce_same_signature_from_specific_parameters()
        {
            var auth = new CoolSms.Authentication(
                "NCS529FF432C2480", 
                "83ECD4D6D7C53AD5B8552209FB4E24BE", 
                "1457514359", 
                "635931111587411589");
            Assert.Equal("VhCyrC6RPheVWCXNWDH17ZkkVD8=", auth.Signature);
        }

        [Fact]
        public void Should_produce_different_signature()
        {
            var auth1 = new CoolSms.Authentication("NCS529FF432C2480", "83ECD4D6D7C53AD5B8552209FB4E24BE");
            var auth2 = new CoolSms.Authentication("NCS529FF432C2480", "83ECD4D6D7C53AD5B8552209FB4E24BE");
            Assert.NotEqual(auth1, auth2);
        }
    }
}
