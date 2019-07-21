using Zapdate.Server.Models.Universal;

namespace Zapdate.Server.Models.Request
{
    public class CreateUpdatePackageRequestDto : UpdatePackageDto
    {
        public string? AsymmetricKeyPassword { get; set; }
    }
}
