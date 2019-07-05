using Zapdate.Models.Universal;

namespace Zapdate.Models.Request
{
    public class CreateUpdatePackageRequestDto : UpdatePackageDto
    {
        public string? AsymmetricKeyPassword { get; set; }
    }
}
