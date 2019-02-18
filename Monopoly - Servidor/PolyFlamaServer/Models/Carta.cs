using PolyFlamaServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Models
{
    public class Carta
    {
        public string texto { get; set; }
        /*
         * Explicación de efecto:
         * El string de efecto va a estar compuesto de dos partes:
         * "{efecto}:{valor}"
         * 
         * De esta forma se puede simplificar, por ejemplo:
         * "perderdinero:400" (Esto haría que el jugador que ha sacado la carta pierda 400$)
         * "ganardinero:200" (Hacer que el jugador gane 200$)
         * "movercasilla:20" (Hacer que el jugador se mueva a la casilla 20)
         */
        public EfectosEnum efecto { get; set; }
        public static readonly Uri imagenDetras = new Uri("");
        public TipoCarta tipo { get; set; }

        public Carta(string texto, EfectosEnum efecto, TipoCarta tipo)
        {
            this.texto = texto;
            this.efecto = efecto;
            this.tipo = tipo;
        }

        public Carta()
        {

        }

    }
}