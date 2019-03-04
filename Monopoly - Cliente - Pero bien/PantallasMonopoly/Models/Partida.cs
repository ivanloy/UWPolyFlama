using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace PantallasMonopoly.Models
{
    public class Partida
    {
        public List<Carta> listadoCartasSuerte { get; set; }
        public List<Carta> listadoCartasComunidad { get; set; }
        public List<Casilla> listadoCasillas { get; set; }
        public int turnoActual { get; set; }
        public int[] arrayDados { get; set; }
        public Uri dado1 {
            get {
                if(arrayDados == null) return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/uno.jpg");
                switch (arrayDados[0])
                {
                    case 1: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/uno.jpg");
                    case 2: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/dos.jpg");
                    case 3: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/tres.jpg");
                    case 4: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/cuatro.jpg");
                    case 5: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/cinco.jpg");
                    case 6: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/seis.jpg");
                    default: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/uno.jpg");
                }
            }
        }
        public Uri dado2 {
            get {
                if (arrayDados == null) return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/uno.jpg");
                switch (arrayDados[1])
                {
                    case 1: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/uno.jpg");
                    case 2: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/dos.jpg");
                    case 3: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/tres.jpg");
                    case 4: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/cuatro.jpg");
                    case 5: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/cinco.jpg");
                    case 6: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/seis.jpg");
                    default: return new Uri("ms-appx://ExamenDI/CustomAssets/Dado/uno.jpg");
                }
            }
        }

        public int nTiradasDobles { get; set; }

        public Partida(List<Carta> listadoCartasSuerte, List<Carta> listadoCartasComunidad, List<Casilla> listadoCasillas, int turnoActual, int[] arrayDados, int nTiradasDobles)
        {
            this.listadoCartasSuerte = listadoCartasSuerte;
            this.listadoCartasComunidad = listadoCartasComunidad;
            this.listadoCasillas = listadoCasillas;
            this.turnoActual = turnoActual;
            this.arrayDados = arrayDados;
            this.nTiradasDobles = nTiradasDobles;
        }

        public Partida()
        {

        }
    }
}