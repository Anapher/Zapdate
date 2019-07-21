#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

namespace Zapdate.Server.Models.Universal
{
    public class UpdateFileDto
    {
        public string Path { get; set; }
        public string Hash { get; set; }
        public string? Signature { get; set; }
    }
}
