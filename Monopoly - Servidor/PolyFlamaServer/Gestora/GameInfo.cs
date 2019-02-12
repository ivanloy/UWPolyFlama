using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Gestora
{
    public static class GameInfo
    {
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, int>> listadoTiradasIniciales;
    }
}