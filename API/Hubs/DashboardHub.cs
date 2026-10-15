using Microsoft.AspNetCore.SignalR;

namespace WIMSFP.Hubs;

public class DashboardHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}