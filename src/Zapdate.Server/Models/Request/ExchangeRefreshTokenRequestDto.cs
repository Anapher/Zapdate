#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

namespace Zapdate.Server.Models.Request
{
    public class ExchangeRefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
