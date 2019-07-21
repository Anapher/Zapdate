#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

namespace Zapdate.Server.Models.Request
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
