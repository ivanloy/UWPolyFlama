using PolyFlamaServer.Models;
using PolyFlamaServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Gestora
{
    public class GestoraPartida
    {
        public static Partida generarPartidaNueva(List<Jugador> listadoJugadores)
        {
            Partida partida = new Partida();
            partida.listadoCartasSuerte = generarCartasSuerte();
            partida.listadoCartasComunidad = generarCartasComunidad();
            partida.listadoCasillas = generarCasillas(listadoJugadores);
            partida.turnoActual = 0;
            partida.arrayDados = new int[] {1, 1};
            partida.nTiradasDobles = 0;

            return partida;
        }

        private static List<Carta> generarCartasComunidad()
        {
            return new List<Carta>();
        }

        private static List<Carta> generarCartasSuerte()
        {
            return new List<Carta>();
        }

        private static List<Casilla> generarCasillas(List<Jugador> listadoJugadores)
        {
            List<Casilla> listadoCasillas = new List<Casilla>();
            
            listadoCasillas.Add(new Casilla(TipoCasilla.SALIDA));
            listadoCasillas.Add(new Propiedad("Podría Hacerse Peor", -99, ColorPropiedad.MARRON, 1, 2));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad("La gran M", -99, ColorPropiedad.MARRON, 3, 4));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAPPLE));
            listadoCasillas.Add(new Propiedad("Geany", 200, ColorPropiedad.ESTACION, 5, 25));
            listadoCasillas.Add(new Propiedad("Queenston", 100, ColorPropiedad.CELESTE, 6, 6));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad("Copiar con flow", 100, ColorPropiedad.CELESTE, 8, 6));
            listadoCasillas.Add(new Propiedad("Tienda D. Sombreros", 120, ColorPropiedad.CELESTE, 9, 8));
            listadoCasillas.Add(new Casilla(TipoCasilla.CARCEL));
            listadoCasillas.Add(new Propiedad("Cuqui Clicker", 140, ColorPropiedad.ROSA, 11, 10));
            listadoCasillas.Add(new Propiedad("Azure", 150, ColorPropiedad.SERVICIO, 12, 4));
            listadoCasillas.Add(new Propiedad("Gorgeanime List", 140, ColorPropiedad.ROSA, 13, 10));
            listadoCasillas.Add(new Propiedad("Overjuacho", 160, ColorPropiedad.ROSA, 14, 12));
            listadoCasillas.Add(new Propiedad("Net Beans", 200, ColorPropiedad.ESTACION, 15, 25));
            listadoCasillas.Add(new Propiedad("Chollometro", 180, ColorPropiedad.NARANJA, 16, 14));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad("GitHub", 180, ColorPropiedad.NARANJA, 18, 14));
            listadoCasillas.Add(new Propiedad("Cianurerías Kudo", 200, ColorPropiedad.NARANJA, 19, 16));
            listadoCasillas.Add(new Casilla(TipoCasilla.DESCANSO));
            listadoCasillas.Add(new Propiedad("Compiladoras Dora", 220, ColorPropiedad.ROJO, 21, 18));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad("Discordia", 220, ColorPropiedad.ROJO, 23, 18));
            listadoCasillas.Add(new Propiedad("Handroid", 240, ColorPropiedad.ROJO, 24, 18));
            listadoCasillas.Add(new Propiedad("Eclipse", 200, ColorPropiedad.ESTACION, 25, 25));
            listadoCasillas.Add(new Propiedad("UwUntu", 260, ColorPropiedad.AMARILLO, 26, 22));
            listadoCasillas.Add(new Propiedad("Del coite", 260, ColorPropiedad.AMARILLO, 27, 22));
            listadoCasillas.Add(new Propiedad("AWS", 150, ColorPropiedad.SERVICIO, 28, 4));
            listadoCasillas.Add(new Propiedad("Dual Monitors", 280, ColorPropiedad.AMARILLO, 29, 24));
            listadoCasillas.Add(new Casilla(TipoCasilla.IRALACARCEL));
            listadoCasillas.Add(new Propiedad("FG Componentess", 300, ColorPropiedad.VERDE, 31, 26));
            listadoCasillas.Add(new Propiedad("Chaomi", 300, ColorPropiedad.VERDE, 32, 26));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad("Polvazo", 320, ColorPropiedad.VERDE, 34, 28));
            listadoCasillas.Add(new Propiedad("IntelIJ", 200, ColorPropiedad.ESTACION, 35, 25));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad("YouTwich", 350, ColorPropiedad.AZUL, 37, 35));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAZURE));
            listadoCasillas.Add(new Propiedad("Haba", 400, ColorPropiedad.AZUL, 39, 50));
            
            //Añadimos todos los jugadores a la casilla de Salida
            listadoCasillas[0].listadoJugadores = listadoJugadores;

            return listadoCasillas;
        }

        public static List<Propiedad> generarPropiedadesJugador()
        {
            List<Propiedad> listado = new List<Propiedad>();
            
            listado.Add(new Propiedad("Podría Hacerse Peor", -99, ColorPropiedad.MARRON, 1, 2));
            listado.Add(new Propiedad("La gran M", -99, ColorPropiedad.MARRON, 3, 4));
            listado.Add(new Propiedad("Geany", 100, ColorPropiedad.CELESTE, 6, 6));
            listado.Add(new Propiedad("Queenston", 100, ColorPropiedad.CELESTE, 8, 6));
            listado.Add(new Propiedad("Copiar con flow", 120, ColorPropiedad.CELESTE, 9, 8));
            listado.Add(new Propiedad("Cuqui Clicker", 140, ColorPropiedad.ROSA, 11, 10));
            listado.Add(new Propiedad("Gorgeanime List", 140, ColorPropiedad.ROSA, 13, 10));
            listado.Add(new Propiedad("Overjuacho", 160, ColorPropiedad.ROSA, 14, 12));
            listado.Add(new Propiedad("Chollometro", 180, ColorPropiedad.NARANJA, 16, 14));
            listado.Add(new Propiedad("GitHub", 180, ColorPropiedad.NARANJA, 18, 14));
            listado.Add(new Propiedad("Cianurerías Kudo", 200, ColorPropiedad.NARANJA, 19, 16));
            listado.Add(new Propiedad("Compiladoras Dora", 220, ColorPropiedad.ROJO, 21, 18));
            listado.Add(new Propiedad("Discordia", 220, ColorPropiedad.ROJO, 23, 18));
            listado.Add(new Propiedad("Handroid", 240, ColorPropiedad.ROJO, 24, 20));
            listado.Add(new Propiedad("UwUntu", 260, ColorPropiedad.AMARILLO, 26, 22));
            listado.Add(new Propiedad("Del coite", 260, ColorPropiedad.AMARILLO, 27, 22));
            listado.Add(new Propiedad("Dual Monitors", 280, ColorPropiedad.AMARILLO, 29, 24));
            listado.Add(new Propiedad("FG Componentess", 300, ColorPropiedad.VERDE, 31, 26));
            listado.Add(new Propiedad("Chaomi", 300, ColorPropiedad.VERDE, 32, 26));
            listado.Add(new Propiedad("Polvazo", 320, ColorPropiedad.VERDE, 34, 28));
            listado.Add(new Propiedad("YouTwich", 350, ColorPropiedad.AZUL, 37, 35));
            listado.Add(new Propiedad("Haba", 400, ColorPropiedad.AZUL, 39, 50));
            listado.Add(new Propiedad("Geany", 200, ColorPropiedad.ESTACION, 5, 25));
            listado.Add(new Propiedad("Net Beans", 200, ColorPropiedad.ESTACION, 15, 25));
            listado.Add(new Propiedad("Eclipse", 200, ColorPropiedad.ESTACION, 25, 25));
            listado.Add(new Propiedad("IntelIJ", 200, ColorPropiedad.ESTACION, 35, 25));
            listado.Add(new Propiedad("Azure", 150, ColorPropiedad.SERVICIO, 12, 4));
            listado.Add(new Propiedad("AWS", 150, ColorPropiedad.SERVICIO, 28, 4));

            return listado;
        }

		public static int calcularNuevaPosicion(int posicionActual, int movimiento)
		{
			int posicionNueva = posicionActual + movimiento;
            posicionNueva = posicionNueva % 40;

			return posicionNueva;
		}

        //Devuelve el index del jugador de listadoJugadores cuyo turno sea el siguiente
        //Si devuelve el mismo turno con el que empezó, significa que solo queda 1 persona con dinero > 0, el ganador
        public static int calcularNuevoTurno(Lobby lobby)
        {
            int turnoActual = lobby.partida.turnoActual;
            int maxJugadores = lobby.maxJugadores;
            int turnoNuevo = turnoActual;
            bool seHaRepetido = false;
            List<int> listadoPerdedores = new List<int>();

            for(int i = 0; i < lobby.listadoJugadores.Count; i++)
            {
                if (lobby.listadoJugadores[i].dinero <= 0)
                    listadoPerdedores.Add(i);
            }

            do
            {
                turnoNuevo++;
                if (turnoNuevo == maxJugadores)
                    turnoNuevo = 0;

                //Si se encuentra con que el turnoNuevo es igual que el turnoActual, es porque solo queda 1 jugador con dinero > 0, es decir, el ganador
                //y el bucle se encuentra en un loop infinito
                if (turnoNuevo == turnoActual)
                    seHaRepetido = true;
            } while (listadoPerdedores.Contains(turnoNuevo) && !seHaRepetido);

            return turnoNuevo;
        }
    }
}