using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Zapdate.Server.Hubs
{
    [Authorize]
    public class CoreHub : Hub
    {
    }
}
