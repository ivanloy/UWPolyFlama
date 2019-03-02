using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Models
{
    public class CasillaDardos
    {
        public bool isBalloon { get; set; }
        public bool isPopped { get; set; }
        public string image { get; set; }  //Image can be null, balloon or popped
        public string background { get; set; }

        public CasillaDardos()
        {
            isBalloon = false;
            isPopped = false;
            image = "null";
            background = "Transparent";
        }

        public CasillaDardos(bool isBalloon, bool isPopped)
        {
            this.isBalloon = isBalloon;
            this.isPopped = isPopped;
            this.image = isBalloon ? (isPopped ? "popped" : "balloon") : "null";
            this.background = "Transparent";
        }

        public void popBalloon(string color)
        {
            this.isPopped = true;
            this.image = "popped";
            this.background = color;
        }
    }
}