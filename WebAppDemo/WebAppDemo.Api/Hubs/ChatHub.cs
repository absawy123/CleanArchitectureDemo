using Microsoft.AspNetCore.SignalR;

namespace WebAppDemo.Api.Hubs
{
    public class ChatHub :Hub
    {
        [HubMethodName("sendMessage")]
        public async Task SendMessageAsync(string user, string message) => 
            await Clients.All.SendAsync("NewMessage",user,message);
        
    }
}
