using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;
using Zapdate.Core.Dto.Services;
using Zapdate.Core.Interfaces.Services;

namespace Zapdate.Infrastructure.Cryptography
{
    public class AsymmetricKeyFactory : IAsymmetricKeyParametersFactory
    {
        public static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private const int KeySize = 4096;

        public AsymmetricKeyParameters Create()
        {
            using (var rsa = RSA.Create(KeySize))
            {
                var privateKey = rsa.ExportParameters(true);
                var publicKey = rsa.ExportParameters(false);

                return new AsymmetricKeyParameters(ToJson(publicKey), ToJson(privateKey));
            }
        }

        public static RSAParameters Deserialize(string key)
        {
            return JsonConvert.DeserializeObject<RSAParametersEx>(key);
        }

        private static string ToJson(RSAParametersEx parameters)
        {
            return JsonConvert.SerializeObject(parameters, _jsonSettings);
        }
    }
}
