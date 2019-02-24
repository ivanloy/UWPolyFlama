using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Gestora
{
    public static class LobbyInfo
    {
        //Diccionario de <nombreLobby, Lobby>
        public static ConcurrentDictionary<string, DatosLobby> listadoLobbies = new ConcurrentDictionary<string, DatosLobby>();
        //Diccionario de <connectionID, nombreLobby>
        public static ConcurrentDictionary<string, string> listadoUsuariosCreandoPersonaje = new ConcurrentDictionary<string, string>();
    }
}