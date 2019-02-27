using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace PantallasMonopoly.Models
{
    public class DatosLobby
    {
        public Lobby lobby { get; set; }
        public int numeroJugadores { get; set; }
        //Diccionario de <nombreJugador, ConnectionId>
        public ConcurrentDictionary<string, string> listadoJugadoresConnection { get; set; }

        public DatosLobby()
        {
            this.lobby = new Lobby();
            this.numeroJugadores = 0;
            this.listadoJugadoresConnection = new ConcurrentDictionary<string, string>();
        }

        public DatosLobby(Lobby lobby, int numeroJugadores, ConcurrentDictionary<string, string> listadoJugadoresConnection)
        {
            this.lobby = lobby;
            this.numeroJugadores = numeroJugadores;
            this.listadoJugadoresConnection = listadoJugadoresConnection;
        }
    }
}