using Microsoft.AspNetCore.SignalR;

namespace Astravon.HUb;

public class PostHub : Hub
{
    public async Task SendUpdate()
    {
        await Clients.All.SendAsync("RefreshPosts");
    }
}