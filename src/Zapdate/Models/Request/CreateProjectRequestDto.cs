#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

using Zapdate.Core.Dto.UseCaseRequests;

namespace Zapdate.Models.Request
{
    public class CreateProjectRequestDto
    {
        public string ProjectName { get; set; }
        public KeyStorage RsaKeyStorage { get; set; }
        public string? RsaKeyPassword { get; set; }
    }
}
