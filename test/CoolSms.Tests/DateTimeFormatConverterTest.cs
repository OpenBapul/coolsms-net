using CoolSms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace CoolSmsTests
{
    public class DateTimeFormatConverterTest
    {
        public class Dummy
        {
            public DateTime Timestamp { get; set; }
        }
        public class DummyWithConverter
        {
            [JsonConverter(typeof(DateTimeFormatConverter), "yyyyMMddHHmm")]
            public DateTime Timestamp { get; set; }
        }

        [Fact]
        public void Serializes_to_expected_format()
        {
            var sut = new DateTimeFormatConverter("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var target = new Dummy
            {
                Timestamp = new DateTime(2016, 1, 12, 15, 20, 59),
            };
            var json = JsonConvert.SerializeObject(
                target, 
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { sut }
                });
            Assert.Equal("{\"Timestamp\":\"201601121520\"}", json);
        }

        [Fact]
        public void Deserializes_to_expected_format()
        {
            var sut = new DateTimeFormatConverter("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var dummy = JsonConvert.DeserializeObject<Dummy>(
                "{\"Timestamp\":\"201601121520\"}",
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { sut }
                });
            var expected = new DateTime(2016, 1, 12, 15, 20, 0);
            Assert.Equal(expected, dummy.Timestamp);
        }

        [Fact]
        public void Serializes_to_expected_format_without_global_settings()
        {
            var target = new DummyWithConverter
            {
                Timestamp = new DateTime(2016, 1, 12, 15, 20, 59),
            };
            var json = JsonConvert.SerializeObject(target);
            Assert.Equal("{\"Timestamp\":\"201601121520\"}", json);
        }

        [Fact]
        public void Deserializes_to_expected_format_without_global_settings()
        {
            var dummy = JsonConvert.DeserializeObject<DummyWithConverter>(
                "{\"Timestamp\":\"201601121520\"}");
            var expected = new DateTime(2016, 1, 12, 15, 20, 0);
            Assert.Equal(expected, dummy.Timestamp);
        }
    }
}
