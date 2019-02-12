﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Gestora
{
    public static class LobbyInfo
    {
        public static ConcurrentDictionary<string, Lobby> listadoLobbies;
        public static ConcurrentDictionary<string, int> listadoLobbiesNumeroJugadores;
    }
}