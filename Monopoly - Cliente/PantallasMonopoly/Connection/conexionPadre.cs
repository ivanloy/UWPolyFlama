using Microsoft.AspNet.SignalR.Client;
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


        public static HubConnection conn
        {

            get
            {

                if (_conn == null)
                {

                    _conn = new HubConnection("http://polyflama.azurewebsites.net/");
                    
                }

                return _conn;
            }          

        }


        public static IHubProxy proxy
        {

            get
            {
                if (_proxy == null) {

                    _proxy = conn.CreateHubProxy("LobbyHub");
                    _conn.Start();
                }

                return _proxy;
            }

        }


    }
}
