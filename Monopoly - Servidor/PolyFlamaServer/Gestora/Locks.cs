using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Gestora
{
    public class Locks
    {
        //Diccionario de <nombreLobby, lock>
        public static readonly Dictionary<string, object> lockUnirSalir = new Dictionary<string, object>();
        //Diccionario de <nombreLobby, lock>
        public static readonly Dictionary<string, object> lockChatLobby = new Dictionary<string, object>();
        public static readonly object lockCrear = new object();
        public static readonly object lockDisconnect = new object();
        public static readonly object lockChatGlobal = new object();
    }
}