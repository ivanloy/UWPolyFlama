using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Gestora
{
    public static class GameInfo
    {
        //Dictionary de <nombreLobby, cantidad de usuarios conectados>
        public static ConcurrentDictionary<string, int> conexionesEstablecidas = new ConcurrentDictionary<string, int>();
    }
}