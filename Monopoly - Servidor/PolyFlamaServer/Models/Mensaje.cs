﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Models
{
    public class Mensaje
    {
        public string texto { get; set; }
        public string color { get; set; }

        public Mensaje(string texto, string color)
        {
            this.texto = texto;
            this.color = color;
        }

        public Mensaje()
        {

        }

    }
}