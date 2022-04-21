using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using rpswitch.Remote;
namespace rpswitch.Hubs
{
    public class ChatHub : Hub
    {
        //[Authorize]
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task Execute(string deviceId,MethodCode mc)
        {
            await Clients.All.SendAsync("Execute", deviceId, mc);
        }
    }
}