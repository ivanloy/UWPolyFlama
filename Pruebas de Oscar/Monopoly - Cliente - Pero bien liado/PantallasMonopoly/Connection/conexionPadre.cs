﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.Connection
{
    public class conexionPadre
    {

        private static HubConnection _conn;
        private static IHubProxy _proxy;

     

        public static IHubProxy proxy
        {

            get
            {
                if (_proxy == null) {

                    _conn = new HubConnection("http://polyflama.azurewebsites.net/");
                    _proxy = _conn.CreateHubProxy("LobbyHub");
                    _conn.Start().Wait();
                }

                return _proxy;
            }

        }


        public static void close()
        {
            if (_conn != null && _conn.State == ConnectionState.Connected) {

                _conn.Stop();
                _proxy = null;
            }

          
        }


    }
}
