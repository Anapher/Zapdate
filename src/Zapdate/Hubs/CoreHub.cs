using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Zapdate.Hubs
{
    [Authorize]
    public class CoreHub : Hub
    {
    }
}
