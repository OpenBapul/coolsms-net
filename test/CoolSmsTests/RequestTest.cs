using CoolSms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace CoolSmsTests
{
    public class RequestTest
    {
        public class RequestImpl : Request
        {
            protected override HttpMethod HttpMethod => HttpMethod.Post;
            protected override Uri RequestUri => new Uri("http://api.dummy.com");
            [JsonProperty("something")]
            public string Something { get; set; }
            [JsonIgnore]
            public Stream File { get; set; }

            protected override IReadOnlyDictionary<string, StreamContent> StreamContents
            {
                get
                {
                    if (File == null)
                    {
                        return base.StreamContents;
                    }
                    return new Dictionary<string, StreamContent>
                    {
                        { "afile", new StreamContent(File) }
                    };
                }
            }
        }

        [Fact]
        public void Returns_propriate_message()
        {
            var auth = new Authentication("abc", "def");
            var sut = new RequestImpl
            {
                Something = "somesome"
            };
            var actual = sut.GetHttpRequest(auth);

            var contents = actual.Content as MultipartFormDataContent;
            var stringContents = contents
                .OfType<StringContent>()
                .ToDictionary(
                    c => c.Headers.ContentDisposition.Name, 
                    c => c.ReadAsStringAsync().Result);

            Assert.Equal(HttpMethod.Post, actual.Method);
            Assert.Equal(new Uri("http://api.dummy.com"), actual.RequestUri);
            Assert.Equal(auth.Algorithm, stringContents["algorithm"]);
            Assert.Equal(auth.ApiKey, stringContents["api_key"]);
            Assert.Equal(auth.Encoding, stringContents["encoding"]);
            Assert.Equal(auth.Salt, stringContents["salt"]);
            Assert.Equal(auth.Signature, stringContents["signature"]);
            Assert.Equal(auth.Timestamp, stringContents["timestamp"]);
            Assert.Equal("somesome", stringContents["something"]);
        }

        [Fact]
        public void Returns_message_with_file()
        {
            var auth = new Authentication("abc", "def");
            var sourceStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes("테스트"));
            sourceStream.Seek(0, SeekOrigin.Begin);
            var sut = new RequestImpl
            {
                File = sourceStream
            };
            var actual = sut.GetHttpRequest(auth);

            var contents = actual.Content as MultipartFormDataContent;
            var streamContents = contents
                .OfType<StreamContent>()
                .ToDictionary(
                    c => c.Headers.ContentDisposition.Name,
                    c => c.ReadAsStreamAsync().Result);
            var stream = streamContents["afile"];
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            Assert.Equal("테스트", Encoding.UTF8.GetString(buffer));
        }
    }
}
