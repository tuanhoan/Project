using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Realtime
{
    public class CommentPostRealtime :Hub
    {
        public async Task SendMessage(string user, string message, string avatar)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, avatar);
        }
    }
}
