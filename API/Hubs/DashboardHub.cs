using Microsoft.AspNetCore.SignalR;

namespace WIMSFP.Hubs;

public class DashboardHub : Hub
{
    public const string DataChangedFlag = "dataChanged";

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public static Task NotifyDataChangedAsync(IHubContext<DashboardHub> hubContext, string message)
    {
        return hubContext.Clients.All.SendAsync(DataChangedFlag, message);
    }
}
