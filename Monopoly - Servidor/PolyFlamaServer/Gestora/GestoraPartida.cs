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
            partida.listadoPropiedades = generarPropiedades();
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

        private static List<Casilla> generarCasillas()
        {
            return new List<Casilla>();
        }
    }
}