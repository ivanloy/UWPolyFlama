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
            listadoCasillas.Add(new Propiedad(-99, ColorPropiedad.MARRON, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad(-99, ColorPropiedad.MARRON, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAPPLE));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION, 1));
            listadoCasillas.Add(new Propiedad(100, ColorPropiedad.CELESTE, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad(100, ColorPropiedad.CELESTE, 1));
            listadoCasillas.Add(new Propiedad(120, ColorPropiedad.CELESTE, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.CARCEL));
            listadoCasillas.Add(new Propiedad(140, ColorPropiedad.ROSA, 1));
            listadoCasillas.Add(new Propiedad(150, ColorPropiedad.SERVICIO, 1));
            listadoCasillas.Add(new Propiedad(140, ColorPropiedad.ROSA, 1));
            listadoCasillas.Add(new Propiedad(160, ColorPropiedad.ROSA, 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION, 1));
            listadoCasillas.Add(new Propiedad(180, ColorPropiedad.NARANJA, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad(180, ColorPropiedad.NARANJA, 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.NARANJA, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.DESCANSO));
            listadoCasillas.Add(new Propiedad(220, ColorPropiedad.ROJO, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad(220, ColorPropiedad.ROJO, 1));
            listadoCasillas.Add(new Propiedad(240, ColorPropiedad.ROJO, 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION, 1));
            listadoCasillas.Add(new Propiedad(260, ColorPropiedad.AMARILLO, 1));
            listadoCasillas.Add(new Propiedad(260, ColorPropiedad.AMARILLO, 1));
            listadoCasillas.Add(new Propiedad(150, ColorPropiedad.SERVICIO, 1));
            listadoCasillas.Add(new Propiedad(280, ColorPropiedad.AMARILLO, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.IRALACARCEL));
            listadoCasillas.Add(new Propiedad(300, ColorPropiedad.VERDE, 1));
            listadoCasillas.Add(new Propiedad(300, ColorPropiedad.VERDE, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad(320, ColorPropiedad.VERDE, 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad(350, ColorPropiedad.AZUL, 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAZURE));
            listadoCasillas.Add(new Propiedad(400, ColorPropiedad.AZUL, 1));

            //Añadimos todos los jugadores a la casilla de Salida
            listadoCasillas[0].listadoJugadores = listadoJugadores;

            return listadoCasillas;
        }

        public static List<Propiedad> generarPropiedadesJugador()
        {
            List<Propiedad> listado = new List<Propiedad>();
            
            listado.Add(new Propiedad(0, ColorPropiedad.MARRON, 1));
            listado.Add(new Propiedad(0, ColorPropiedad.MARRON, 3));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION, 5));
            listado.Add(new Propiedad(0, ColorPropiedad.CELESTE, 6));
            listado.Add(new Propiedad(0, ColorPropiedad.CELESTE, 8));
            listado.Add(new Propiedad(0, ColorPropiedad.CELESTE, 9));
            listado.Add(new Propiedad(0, ColorPropiedad.ROSA, 11));
            listado.Add(new Propiedad(0, ColorPropiedad.SERVICIO, 12));
            listado.Add(new Propiedad(0, ColorPropiedad.ROSA, 13));
            listado.Add(new Propiedad(0, ColorPropiedad.ROSA, 14));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION, 15));
            listado.Add(new Propiedad(0, ColorPropiedad.NARANJA, 16));
            listado.Add(new Propiedad(0, ColorPropiedad.NARANJA, 18));
            listado.Add(new Propiedad(0, ColorPropiedad.NARANJA, 19));
            listado.Add(new Propiedad(0, ColorPropiedad.ROJO, 21));
            listado.Add(new Propiedad(0, ColorPropiedad.ROJO, 23));
            listado.Add(new Propiedad(0, ColorPropiedad.ROJO, 24));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION, 25));
            listado.Add(new Propiedad(0, ColorPropiedad.AMARILLO, 26));
            listado.Add(new Propiedad(0, ColorPropiedad.AMARILLO, 27));
            listado.Add(new Propiedad(0, ColorPropiedad.SERVICIO, 28));
            listado.Add(new Propiedad(0, ColorPropiedad.AMARILLO, 29));
            listado.Add(new Propiedad(0, ColorPropiedad.VERDE, 31));
            listado.Add(new Propiedad(0, ColorPropiedad.VERDE, 32));
            listado.Add(new Propiedad(0, ColorPropiedad.VERDE, 34));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION, 35));
            listado.Add(new Propiedad(0, ColorPropiedad.AZUL, 37));
            listado.Add(new Propiedad(0, ColorPropiedad.AZUL, 39));

            return listado;
        }

		public static int calcularNuevaPosicion(int posicionActual, int movimiento)
		{
			int posicionNueva = posicionActual + movimiento;
            posicionNueva = posicionNueva % 40;

			return posicionNueva;
		}

        public static int calcularNuevoTurno(int turnoActual, int maxJugadores)
        {
            int turnoNuevo = turnoActual + 1;
            if (turnoNuevo == maxJugadores)
                turnoNuevo = 0;

            return turnoNuevo;
        }
    }
}