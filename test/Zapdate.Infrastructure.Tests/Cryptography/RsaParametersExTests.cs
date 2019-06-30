using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using Xunit;
using Zapdate.Infrastructure.Cryptography;

namespace Zapdate.Infrastructure.Tests.Cryptography
{
    public class RSAParametersExTests
    {
        private readonly RSAParameters _rsaParameters;
        private readonly RSAParameters _publicRsaParameters;

        public RSAParametersExTests()
        {
            _rsaParameters = new RSAParameters
            {
                D = Convert.FromBase64String("mkHJlUE1k/em0rmoYlMbRgXL66TEc4WQAQ8+UMKvwceqzd1fGyteh9awuhqP3VwfPp0gAH/jyhCeDkWEhIPxHoIjmvOF/uoBrqJViWA2Bt/EgEEkSUT6YttO6huCmEgJCRNMV7tWIA6S2iu0BqHQRB08Q1Jw7qfs3dUxmS5VaV/YfIWCXXxuz58dYjUQE8bAiXijcKZ2mvgsEcsslPQDeG8djmvDO5fBFZyLr9Mo1RovdD5aHbvxPY0t0AkZ3OfKx+xbdQIq3h5sTtQVl1xKZdZAJcNxyeWNDIJseiz/5Gh1qvVHd6MFyajbFSYUTCx88R8UszO5nHMZ4U+VbsU0y+HnahC7cUI1wtzXamH0pgf9q25whdRXN5BgyTt/NmiHnDL8yWBzpVIXnSpqodsSoPfQWtfzG9LRaHAZ9SEf80kfTV/4IhW7NEshfiH0lPGUGgnLm8SFwnR9kGXtnOVbR6F2TP88+5onckVJvd85Z3CKa53rlRjh3SaRJS7urwpGb5YpCm44kttfhzZKJVZWkgIdMmuq/HgxMwyosWvd/UAgbVfbR7LJEGhO7EHasjLKtqiyTKGyRZ++2RwFfSRlzdBWlohNbRZfACk3fHqr3k4Jjm8hwggUNkscDvunnvxmAHjmymQShrTDf3HU9UQVHnLOsXt6Ot1TjzaNXXIvJBE="),
                DP = Convert.FromBase64String("w3NEgIYtq7jVI59R0hjnsVSIW4cTuLa21yG6CxZUdSvocNyf9hs2nQ+qna+H5CgIbN+LeR4YTtRWYs7L9x1wiR2HWudHgN6PRFL9a5uqIndWVyBO/NXYypOwO9QY1ZbVz+fEz9gDf4uA4Ggj0cxmiobRu9oG/ZLe5RN8SNpJe5pFO93cB0o+S0q6yVDw6sYWurKhVxbCCunUYVFQwwYMp9i/Jyq/vHnHBQOTxDf72CL76JysRy5U/hxZlWetpD8PEeAPzkyfaaYKR1nq0pBRr0vBLABw9TR4qty724/TC4I6Q/dBqHLLhZrt1U/CdCGfb4cf/N79NkkA/MH6gPgcgw=="),
                DQ = Convert.FromBase64String("Byn2LoCZJVLnNI+ZNVnnjoZCSLw4pqzBDXTUkLugpDgWBPF4QbgdrQXsMfdVODo2k5lCiKzN7Pn/h//fI3rPPFyOISkP+DQDeGCSVnr6i0XmqQ5CtVFChatSqH6hrNfhrcsV01PHP+ZIkYJyLd6covrgIxYummDMzks0I5OcnUk68JZPxl3qZYTsE1WQCABu/NRBzX2HfutsBz9aSSDd6ia57pZE5WJ6USpT5lblqz1L51IzOWvp3OSApOVWcx1FL8KsubCNT6FkgVobutiBkhwh9JxCA5a+pxUcx611tY/KkCeoO4lTv2caVx2Z8qb9VFZGEocxLByAzA+ZsajIcw=="),
                Exponent = Convert.FromBase64String("AQAB"),
                InverseQ = Convert.FromBase64String("KSK2G8Fnv0FmKHfBDBlAEwZu6lIDNz8py9b63zoNV3VWSe9BAjBvLEYfZT+v7x0043X37jFATDWs9Jff6WWqLsU6+9ttlt4ohSvolS6NMBp6PubAjqEV7to3yAoTMYkDOnmaqrE7Tzt3JqDyUN70aClUTogDVFM6F9CdtlsxdmNkuKTvsulSiP5oN19r5KyXs0t9sbDhTyEPsejGxGZjZESb4Wtrx0/pAHNnq2pSfC1X/JEiL8JO8mhaZcjjBAaz0QIQuTg+FBhWlRvm9SsON/3lUwZc9Dt+z35pXa73294knFvYwK+tFZ//O2QttMyhZWVeD+lWr5M/KCq620tpIw=="),
                Modulus = Convert.FromBase64String("25nKV+vzwBnzjo+e/wS5eOYvp0wBpcquBnJIjuXHIEWlxEytuXUfDCbXGUO+BoRrLyh/Zbf/4J3if73obuiQgdS52aW9xcRB44GXYyvLRyUCeBoBb96Q7/5/TqTlCi4NITvMXhaK/vsOToLqPJREz0uQKbi2C+1hK1HJOkFX7UF5DQu3UzTDXVgiWevy+FHKe8TneP4mT6tWBsH/ma7yenykJJ69ENrbcHjVbZZNf2ezvZ0B6dWv7o1l7SGqN9cdYcy5si3dhjO1EZjFOszIeEvWyG7+DM795fjjOGU4IMnwDFbaGiwSfPTLHb732WPEpCJ3GqwaKMa2u3ndbKyKR1X4qj/JyC3PUGnS2dO63SI7Lu0n2QE3qQn1bCKLi30BQ5B30H2R3TgKpuwqp2PTGpOlZDNOqZEKW0IlxLs11XGmRdfGTx4dwHo+E2DSuLuWvmJaMbu+Opb9szlSIibT+s2dvhaD0CakR8oeDc5ZjFicZgPIj7MsoUDdnIFofcQAnTkRoatiravoiVxT+VHZm53JHP8KpYDIGD7hjWf7buesPvUcz8ywpsEq03pMJrdyk4JWkp/r2rDV03iKKyYNG+A240DBVoeksWZm9xtXcrxFPWI9OyOlJM+nr7yuIucAZbY69As/Vd7AYhNUqDGcPGYeMKMLVzJKjDQ7RYLkOjE="),
                P = Convert.FromBase64String("+LLatMQ98MXx9LgwoyljLOYO14X2lcUeV+3Vhu7dY4ELWeoqCIPgxHYJBeQBMPuapfYSJS0nNMEEKnkQDIhJUOdDw3QvVrf+n2RL11j7z4QkUOQw9D/Jw+P2CXSQXlEGO0jk9ETd0QFFAOUOf5I3NzkL5cFRTwN8kA3DylUicHV25bymks6MKagu+vk8iHE2jc/gefQ5zdQHRbzwm78LmvgHCaMZEl+EunE9RN0QPSZd3mm5gln+qQKJ0VqrjDaXZI/Fnpy+rO1Yg8GQFEFK08und1eWiVbJkOEDYFl1yjyT+DGxLZXxD50+14OeqMjw2drjfqpTZklouFEc9Q2kIw=="),
                Q = Convert.FromBase64String("4gw+q6qqFPWWrKfYLro5+4pEpvL7jM6KkHCoT+SPcxYXi5Wn5WxELU7o8Aq+/ZaX05uv4F2AR/iq1Kn8OO/xG3XEzX0+JzFih1a8dnydDrjfs7IS7eCsbLEIuoUi0Pw+U6kuhJK+gxXfrofJzoySn25zXJqF7HPCQs4im4RGnMLFL1xcxWp2gFjwn/CjYcxa3v92K4zBUZWf8aBL+/OOfBK+trAwxCTwjJb+ufdfigp+iRJuotYg4pZQSbQ9rt0n8DTQGU0oU6dF+ijOsUTzYXU5KJO8cAI/k//7qQMBe1D+ZBwmWyd5/SWPnmAF8o66CBX9YyEeW6lXNelKelPTmw==")
            };

            _publicRsaParameters = new RSAParameters
            {
                Modulus = _rsaParameters.Modulus,
                Exponent = _rsaParameters.Exponent
            };
        }

        [Fact]
        public void TestCreateFromRsaParamaters()
        {
            var parametersEx = new RSAParametersEx(_rsaParameters);
            Assert.Equal(_rsaParameters.D, parametersEx.D);
            Assert.Equal(_rsaParameters.DP, parametersEx.DP);
            Assert.Equal(_rsaParameters.DQ, parametersEx.DQ);
            Assert.Equal(_rsaParameters.Exponent, parametersEx.Exponent);
            Assert.Equal(_rsaParameters.InverseQ, parametersEx.InverseQ);
            Assert.Equal(_rsaParameters.Modulus, parametersEx.Modulus);
            Assert.Equal(_rsaParameters.P, parametersEx.P);
            Assert.Equal(_rsaParameters.Q, parametersEx.Q);
        }

        [Fact]
        public void TestCreateFromPublicRsaParameters()
        {
            var parametersEx = new RSAParametersEx(_publicRsaParameters);
            Assert.Equal(_rsaParameters.Modulus, parametersEx.Modulus);
            Assert.Equal(_rsaParameters.Exponent, parametersEx.Exponent);
        }

        [Fact]
        public void TestCreatePublicKey()
        {
            RSAParametersEx rsaParameters = _rsaParameters;

            var publicKey = rsaParameters.ToPublicKey();
            Assert.Null(publicKey.D);
            Assert.Null(publicKey.DP);
            Assert.Null(publicKey.DQ);
            Assert.Null(publicKey.InverseQ);
            Assert.Null(publicKey.P);
            Assert.Null(publicKey.Q);
            Assert.NotNull(publicKey.Modulus);
            Assert.NotNull(publicKey.Exponent);
        }

        [Fact]
        public void TestConvertToRSAParameters()
        {
            RSAParametersEx rsaParametersEx = _rsaParameters;

            var rsaParameters = rsaParametersEx.ToRSAParameters();
            Assert.Equal(rsaParametersEx.D, rsaParameters.D);
            Assert.Equal(rsaParametersEx.DP, rsaParameters.DP);
            Assert.Equal(rsaParametersEx.DQ, rsaParameters.DQ);
            Assert.Equal(rsaParametersEx.Exponent, rsaParameters.Exponent);
            Assert.Equal(rsaParametersEx.InverseQ, rsaParameters.InverseQ);
            Assert.Equal(rsaParametersEx.Modulus, rsaParameters.Modulus);
            Assert.Equal(rsaParametersEx.P, rsaParameters.P);
            Assert.Equal(rsaParametersEx.Q, rsaParameters.Q);
        }

        [Fact]
        public void TestPrivateKeyToStringAndParse()
        {
            RSAParametersEx rsaParametersEx = _rsaParameters;
            var s = rsaParametersEx.ToString();

            Assert.NotNull(s);

            var rsaParameters = RSAParametersEx.Parse(s);
            AssertRsaKeysEqual(rsaParametersEx, rsaParameters);
        }

        [Fact]
        public void TestJsonSerializeDeserialize()
        {
            RSAParametersEx rsaParametersEx = _rsaParameters;

            var jsonString = JsonConvert.SerializeObject(rsaParametersEx);
            var deserialized = JsonConvert.DeserializeObject<RSAParametersEx>(jsonString);

            AssertRsaKeysEqual(rsaParametersEx, deserialized);
        }

        private static void AssertRsaKeysEqual(RSAParametersEx expected, RSAParametersEx actual)
        {
            Assert.Equal(expected.D, actual.D);
            Assert.Equal(expected.DP, actual.DP);
            Assert.Equal(expected.DQ, actual.DQ);
            Assert.Equal(expected.Exponent, actual.Exponent);
            Assert.Equal(expected.InverseQ, actual.InverseQ);
            Assert.Equal(expected.Modulus, actual.Modulus);
            Assert.Equal(expected.P, actual.P);
            Assert.Equal(expected.Q, actual.Q);
        }
    }
}
