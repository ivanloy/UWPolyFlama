using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace PantallasMonopoly.Util
{
    public class Colores
    {

        public Brush VERDE { get; set; }
        public Brush AZUL { get; set; }
        public Brush MARRON { get; set; }
        public Brush CELESTE { get; set; }
        public Brush ROSA { get; set; }
        public Brush NARANJA { get; set; }
        public Brush ROJO { get; set; }
        public Brush AMARILLO { get; set; }

        public Brush TABLERO { get; set; }

        public Brush PLAYER_1 { get; set; }
        public Brush PLAYER_1B { get; set; }
        public Brush PLAYER_1C { get; set; }

        public Brush PLAYER_2 { get; set; }
        public Brush PLAYER_2B { get; set; }
        public Brush PLAYER_2C { get; set; }

        public Brush PLAYER_3 { get; set; }
        public Brush PLAYER_3B { get; set; }
        public Brush PLAYER_3C { get; set; }

        public Brush PLAYER_4 { get; set; }
        public Brush PLAYER_4B { get; set; }
        public Brush PLAYER_4C { get; set; }

        public Colores()
        {

            VERDE = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 1, 176, 84));
            AZUL = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 19, 109, 180));
            MARRON = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 142, 71, 52));
            CELESTE = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 160, 221, 250));
            ROSA = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 216, 34, 143));
            NARANJA = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 251, 131, 42));
            ROJO = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 0, 39));
            AMARILLO = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 251, 241, 48));

            TABLERO = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 199, 228, 203));

            PLAYER_1 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 46, 125, 50));
            PLAYER_1B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 129, 199, 132));
            PLAYER_1C = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 200, 230, 201));

            PLAYER_4 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 198, 40, 40));
            PLAYER_4B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 229, 115, 115));
            PLAYER_4C = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 205, 210));

            PLAYER_3 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 21, 101, 192));
            PLAYER_3B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 100, 181, 246));
            PLAYER_3C = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 179, 229, 252));

            PLAYER_2 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 106, 27, 154));
            PLAYER_2B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 186, 104, 200));
            PLAYER_2C = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 225, 190, 231));


        }


    }
}
