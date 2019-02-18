using PolyFlamaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Gestora
{
    public class GestoraPartida
    {
        public static Partida generarPartidaNueva()
        {
            Partida partida = new Partida();
            partida.listadoCartasSuerte = generarCartasSuerte();
            partida.listadoCartasComunidad = generarCartasComunidad();
            partida.listadoCasillas = generarCasillas();
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

        private static List<Propiedad> generarPropiedades()
        {
            return new List<Propiedad>();
        }

        private static List<Object> generarCasillas()
        {
            return new List<Object>();
        }

		public static int calcularNuevaPosicion(int posicionActual, int movimiento)
		{
			int posicionNueva = posicionActual + movimiento;
			if (posicionNueva / 35 >= 1)
				posicionNueva -= 35;

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