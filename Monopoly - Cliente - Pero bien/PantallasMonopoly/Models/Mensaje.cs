using System;
using System.Collections.Generic;
using System.Linq;


namespace PantallasMonopoly.Models
{
    public class Mensaje
    {
        public string texto {
            get {
                return bobbainator(_texto);
            }
            set {
                _texto = bobbainator(value);
            }
        }
        private string _texto;
        public string color { get; set; }

        public Mensaje(string texto, string color)
        {
            this._texto = texto;
            this.color = color;
        }

        public string bobbainator(string s)
        {
            string r = s.Replace("pene", "bobba");
            r = r.Replace("Me gustaria comer uwu", "Do not worry about me");
            r = r.Replace("SignalR", "Puta mierda");
            r = r.Replace("UWP", "Puta mierda");
            r = r.Replace("Microsoft", "Puta mierda");
            r = r.Replace("owo uwu owo uwu owo uwu owo uwu owo uwu owo uwu owo", "A person who does not want to be able to find himself in a state of emergency does not have to be able to cope");
            r = r.Replace("owo uwu owo uwu", "No one can afford it");
            r = r.Replace("uwu owo uwu", "hiking a traveler");
            r = r.Replace("owo uwu owo", "human trafficking");
            r = r.Replace("uwu owo", "borrow money");
            r = r.Replace("owo uwu", "cheapest");
            r = r.Replace("dick", "bobba");
            r = r.Replace("owo", "rent");
            r = r.Replace("ewe", "what");
            r = r.Replace("u.u", "bobba");
            r = r.Replace("oscar", "bobba");
            r = r.Replace("Fernando apruebanos", "bobba");
            return r;
        }
        
        public Mensaje()
        {

        }

    }
}