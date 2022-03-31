using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ReversiMvcApp.Hubs
{
    public class MoveData
    {
        public string Token { get; set; }
        public string PlayerToken { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }

    }
    public class ReversiHub : Hub
    {
        // private Dictionary<Guid, string> _rooms = new();

        public async Task Join(string gameToken)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameToken);
            await Clients.Group(gameToken).SendAsync("message", $"{Context.ConnectionId} joined {gameToken}");
        }
        public async Task Leave(string gameToken)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameToken);
            await Clients.Group(gameToken).SendAsync("message", $"{Context.ConnectionId} left {gameToken}");
        }

        public async Task Message(string message)
        {
            await Clients.All.SendAsync("message", message);
        }
        public async Task Move(string gameToken)
        {
            await Clients.Group(gameToken).SendAsync("move");
        }
    }
}
