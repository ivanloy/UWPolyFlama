using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Gestora
{
    public static class LobbyInfo
    {
        //Dictionario de <nombreLobby, Lobby>
        public static ConcurrentDictionary<string, DatosLobby> listadoLobbies = new ConcurrentDictionary<string, DatosLobby>();
    }
}