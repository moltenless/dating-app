using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Others.SendAsync("UserOnline",
            Context.User?.FindFirstValue(ClaimTypes.Email));
            
        //await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Others.SendAsync("UserOffline",
            Context.User?.FindFirstValue(ClaimTypes.Email));

        await base.OnDisconnectedAsync(exception);
    }
}
