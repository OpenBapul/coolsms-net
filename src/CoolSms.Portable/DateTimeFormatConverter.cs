using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;
using System.Globalization;

namespace CoolSms
{
    /// <summary>
    /// 커스텀 날짜/시간 포맷 컨버터.
    /// 기본 포맷은 yyyyMMddHHmmss 입니다.
    /// </summary>
    public class DateTimeFormatConverter : DateTimeConverterBase
    {
        public DateTimeFormatConverter() 
            : this("yyyyMMddHHmmss", CultureInfo.InvariantCulture) { }
        public DateTimeFormatConverter(string format) 
            : this(format, CultureInfo.InvariantCulture) { }
        public DateTimeFormatConverter(string format, CultureInfo cultureInfo)
        {
            Format = format;
            CultureInfo = cultureInfo;
        }

        /// <summary>
        /// 날짜/시간 포맷
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 포맷 표기 컬쳐 정보
        /// </summary>
        public CultureInfo CultureInfo { get; set; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            var value = reader.Value.ToString();
            if (value == string.Empty)
            {
                return null;
            }
            DateTime output;
            if (DateTime.TryParseExact(value, Format, CultureInfo.DateTimeFormat, DateTimeStyles.None, out output))
            {
                return output;
            }
            if (objectType.Equals(typeof(Nullable<DateTime>)))
            {
                return null;
            }
            return output;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(((DateTime)value).ToString(Format));
            }
        }
    }
}
