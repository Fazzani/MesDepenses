using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using XBMCPluginData.Controllers;

namespace XBMCPluginData.Hubs
{
    public class LoggerHub : Hub
    {
        public void Send(LogMessage message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(message.Level, message.Source, message.Message);
        }
    }
}