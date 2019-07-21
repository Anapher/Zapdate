using Zapdate.Server.Core.Dto.Services;

namespace Zapdate.Server.Core.Interfaces.Services
{
    public interface IAsymmetricKeyParametersFactory
    {
        AsymmetricKeyParameters Create();
    }
}
