#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

using Zapdate.Server.Core.Dto.UseCaseRequests;

namespace Zapdate.Server.Models.Request
{
    public class CreateProjectRequestDto
    {
        public string Name { get; set; }
        public KeyStorage RsaKeyStorage { get; set; }
        public string? RsaKeyPassword { get; set; }
    }
}
