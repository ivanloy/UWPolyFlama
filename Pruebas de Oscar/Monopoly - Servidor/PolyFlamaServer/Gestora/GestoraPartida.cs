using PolyFlamaServer.Models;
using PolyFlamaServer.Models.Enums;
using System;
using System.Collections.Generic;
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
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.MARRON));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.MARRON));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAPPLE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ESTACION));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.CELESTE));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.CELESTE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.CELESTE));
            listadoCasillas.Add(new Casilla(TipoCasilla.CARCEL));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ROSA));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.SERVICIO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ROSA));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ROSA));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ESTACION));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.NARANJA));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.NARANJA));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.NARANJA));
            listadoCasillas.Add(new Casilla(TipoCasilla.DESCANSO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ROJO));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ROJO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ROJO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ESTACION));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.AMARILLO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.AMARILLO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.SERVICIO));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.AMARILLO));
            listadoCasillas.Add(new Casilla(TipoCasilla.IRALACARCEL));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.VERDE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.VERDE));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.VERDE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.ESTACION));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.AZUL));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAZURE));
            listadoCasillas.Add(new Propiedad(0, ColorPropiedad.AZUL));

            //Añadimos todos los jugadores a la casilla de Salida
            listadoCasillas[0].listadoJugadores = listadoJugadores;

            return listadoCasillas;
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