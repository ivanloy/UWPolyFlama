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
            partida.listadoCasillas = new List<Casilla>(); //generarCasillas(listadoJugadores);
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
            /*List<Casilla> listadoCasillas = new List<Casilla>();
            
            listadoCasillas.Add(new Casilla(TipoCasilla.SALIDA.ToString()));
            listadoCasillas.Add(new Propiedad(-99, ColorPropiedad.MARRON.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD.ToString()));
            listadoCasillas.Add(new Propiedad(-99, ColorPropiedad.MARRON.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAPPLE.ToString()));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION.ToString(), 1));
            listadoCasillas.Add(new Propiedad(100, ColorPropiedad.CELESTE.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE.ToString()));
            listadoCasillas.Add(new Propiedad(100, ColorPropiedad.CELESTE.ToString(), 1));
            listadoCasillas.Add(new Propiedad(120, ColorPropiedad.CELESTE.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.CARCEL.ToString()));
            listadoCasillas.Add(new Propiedad(140, ColorPropiedad.ROSA.ToString(), 1));
            listadoCasillas.Add(new Propiedad(150, ColorPropiedad.SERVICIO.ToString(), 1));
            listadoCasillas.Add(new Propiedad(140, ColorPropiedad.ROSA.ToString(), 1));
            listadoCasillas.Add(new Propiedad(160, ColorPropiedad.ROSA.ToString(), 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION.ToString(), 1));
            listadoCasillas.Add(new Propiedad(180, ColorPropiedad.NARANJA.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD.ToString()));
            listadoCasillas.Add(new Propiedad(180, ColorPropiedad.NARANJA.ToString(), 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.NARANJA.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.DESCANSO.ToString()));
            listadoCasillas.Add(new Propiedad(220, ColorPropiedad.ROJO.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE.ToString()));
            listadoCasillas.Add(new Propiedad(220, ColorPropiedad.ROJO.ToString(), 1));
            listadoCasillas.Add(new Propiedad(240, ColorPropiedad.ROJO.ToString(), 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION.ToString(), 1));
            listadoCasillas.Add(new Propiedad(260, ColorPropiedad.AMARILLO.ToString(), 1));
            listadoCasillas.Add(new Propiedad(260, ColorPropiedad.AMARILLO.ToString(), 1));
            listadoCasillas.Add(new Propiedad(150, ColorPropiedad.SERVICIO.ToString(), 1));
            listadoCasillas.Add(new Propiedad(280, ColorPropiedad.AMARILLO.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.IRALACARCEL.ToString()));
            listadoCasillas.Add(new Propiedad(300, ColorPropiedad.VERDE.ToString(), 1));
            listadoCasillas.Add(new Propiedad(300, ColorPropiedad.VERDE.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.COMUNIDAD.ToString()));
            listadoCasillas.Add(new Propiedad(320, ColorPropiedad.VERDE.ToString(), 1));
            listadoCasillas.Add(new Propiedad(200, ColorPropiedad.ESTACION.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.SUERTE.ToString()));
            listadoCasillas.Add(new Propiedad(350, ColorPropiedad.AZUL.ToString(), 1));
            listadoCasillas.Add(new Casilla(TipoCasilla.IMPUESTOAZURE.ToString()));
            listadoCasillas.Add(new Propiedad(400, ColorPropiedad.AZUL.ToString(), 1));
            
            //Añadimos todos los jugadores a la casilla de Salida
            listadoCasillas[0].listadoJugadores = listadoJugadores;*/

            return null;
        }

        public static List<Propiedad> generarPropiedadesJugador()
        {
            List<Propiedad> listado = new List<Propiedad>();
            
            listado.Add(new Propiedad(0, ColorPropiedad.MARRON.ToString(), 1));
            listado.Add(new Propiedad(0, ColorPropiedad.MARRON.ToString(), 3));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION.ToString(), 5));
            listado.Add(new Propiedad(0, ColorPropiedad.CELESTE.ToString(), 6));
            listado.Add(new Propiedad(0, ColorPropiedad.CELESTE.ToString(), 8));
            listado.Add(new Propiedad(0, ColorPropiedad.CELESTE.ToString(), 9));
            listado.Add(new Propiedad(0, ColorPropiedad.ROSA.ToString(), 11));
            listado.Add(new Propiedad(0, ColorPropiedad.SERVICIO.ToString(), 12));
            listado.Add(new Propiedad(0, ColorPropiedad.ROSA.ToString(), 13));
            listado.Add(new Propiedad(0, ColorPropiedad.ROSA.ToString(), 14));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION.ToString(), 15));
            listado.Add(new Propiedad(0, ColorPropiedad.NARANJA.ToString(), 16));
            listado.Add(new Propiedad(0, ColorPropiedad.NARANJA.ToString(), 18));
            listado.Add(new Propiedad(0, ColorPropiedad.NARANJA.ToString(), 19));
            listado.Add(new Propiedad(0, ColorPropiedad.ROJO.ToString(), 21));
            listado.Add(new Propiedad(0, ColorPropiedad.ROJO.ToString(), 23));
            listado.Add(new Propiedad(0, ColorPropiedad.ROJO.ToString(), 24));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION.ToString(), 25));
            listado.Add(new Propiedad(0, ColorPropiedad.AMARILLO.ToString(), 26));
            listado.Add(new Propiedad(0, ColorPropiedad.AMARILLO.ToString(), 27));
            listado.Add(new Propiedad(0, ColorPropiedad.SERVICIO.ToString(), 28));
            listado.Add(new Propiedad(0, ColorPropiedad.AMARILLO.ToString(), 29));
            listado.Add(new Propiedad(0, ColorPropiedad.VERDE.ToString(), 31));
            listado.Add(new Propiedad(0, ColorPropiedad.VERDE.ToString(), 32));
            listado.Add(new Propiedad(0, ColorPropiedad.VERDE.ToString(), 34));
            listado.Add(new Propiedad(0, ColorPropiedad.ESTACION.ToString(), 35));
            listado.Add(new Propiedad(0, ColorPropiedad.AZUL.ToString(), 37));
            listado.Add(new Propiedad(0, ColorPropiedad.AZUL.ToString(), 39));

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