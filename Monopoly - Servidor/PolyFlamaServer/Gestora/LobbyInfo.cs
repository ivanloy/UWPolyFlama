using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Gestora
{
    public static class LobbyInfo
    {
        public static ConcurrentDictionary<string, Lobby> listadoLobbies = new ConcurrentDictionary<string, Lobby>();
        public static ConcurrentDictionary<string, int> listadoLobbiesNumeroJugadores = new ConcurrentDictionary<string, int>();
    }
}