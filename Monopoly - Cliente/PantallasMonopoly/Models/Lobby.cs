using System;
using System.Collections.Generic;
using System.Linq;

namespace PantallasMonopoly.Models
{
    public class Lobby
    {
        public string nombre { get; set; }
        public string contrasena { get; set; }
        public int maxJugadores { get; set; }
        public List<Jugador> listadoJugadores { get; set; }
        public bool partidaEmpezada { get; set; }
        public Partida partida { get; set; }

        public Lobby(string nombre, string contrasena, int maxJugadores, List<Jugador> listadoJugadores, bool partidaEmpezada, Partida partida)
        {
            this.nombre = nombre;
            this.contrasena = contrasena;
            this.maxJugadores = maxJugadores;
            this.listadoJugadores = listadoJugadores;
            this.partidaEmpezada = partidaEmpezada;
            this.partida = partida;
        }

        /*
         * Constructor para hacer más fácil la cración de Lobbies
         * Solo hará falta el nombre, la contraseña, el máximo de jugadores, y el jugador
         * que ha creado el lobby, que se añadirá a la lista.
         */ 
        public Lobby(string nombre, string contrasena, int maxJugadores, Jugador jugadorCreador, Partida partida)
        {
            this.nombre = nombre;
            this.contrasena = contrasena;
            this.maxJugadores = maxJugadores;
            this.listadoJugadores = new List<Jugador>();
            this.listadoJugadores.Add(jugadorCreador);
            this.partidaEmpezada = false;
            this.partida = partida;
        }

        public Lobby()
        {

        }

        public bool tieneContrasena()
        {
            return this.contrasena == "";
        }

        public void quitarContrasena()
        {
            this.contrasena = "";
        }
    }
}