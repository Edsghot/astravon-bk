using Microsoft.AspNetCore.SignalR;

namespace Astravon.HUb;

public class AstravonHub: Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
}