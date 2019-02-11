﻿using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using System;

[assembly: OwinStartup(typeof(PolyFlamaServer.Startup))]

namespace PolyFlamaServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //Marcar un usuario como desconectado tras 5 segundos
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
        }
    }
}