using Xunit;

namespace CoolSms.Tests
{
    public class MessageTypeUtilsTest
    {
        [Theory]
        [InlineData("a", 1)]
        [InlineData("가", 2)]
        [InlineData("あ", 2)]
        [InlineData("abcd가나다라!@#$", 16)]
        [InlineData("abcd가나다라!@#$abcd가나다라!@#$abcd가나다라!@#$abcd가나다라!@#$abcd가나다라!@#$abcd가나다라!@#$abcd가나다라!@#$abcd가나다라!@#$", 128)]
        public void Returns_MBCS_text_length(string source, int expected)
        {
            var actual = MessageTypeUtils.GetSmsTextLength(source);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("a", MessageType.SMS)]
        [InlineData("가", MessageType.SMS)]
        [InlineData("abcd가나다라!@#$", MessageType.SMS)]
        [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901", MessageType.LMS)]
        public void Returns_correct_MessageType(string source, MessageType expected)
        {
            var actual = MessageTypeUtils.GetMessageType(source);
            Assert.Equal(expected, actual);
        }
    }
}
