using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ReversiMvcApp.Hubs
{
    public class ReversiHub : Hub
    {
        public async Task Move(int row, int col, string color)
        {
            await Clients.All.SendAsync("move", row, col, color);
        }
    }
}
