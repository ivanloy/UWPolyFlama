namespace PantallasMonopoly.Models
{
    public class Colores
    {

        public Brush VERDE {get; set;}
        public Brush AZUL {get; set;}
        public Brush MARRON {get; set;}
        public Brush CELESTE {get; set;}
        public Brush ROSA {get; set;}
        public Brush NARANJA {get; set;}
        public Brush ROJO {get; set;}
        public Brush AMARILLO {get; set;}

        public Brush TABLERO {get; set;}

        public Brush PLAYER_1 {get; set;}
        public Brush PLAYER_1B {get; set;}

        public Brush PLAYER_2 {get; set;}
        public Brush PLAYER_2B {get; set;}

        public Brush PLAYER_3 {get; set;}
        public Brush PLAYER_3B {get; set;}

        public Brush PLAYER_4 {get; set;}
        public Brush PLAYER_4B {get; set;}




        public Colores(){

            VERDE = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 1, 176, 84));
            AZUL = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 19, 109, 180));
            MARRON = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 142, 71, 52));
            CELESTE = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 160, 221, 250));
            ROSA = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 216, 34, 143));
            NARANJA = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 251, 131, 42));
            ROJO = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 0, 39));
            AMARILLO = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 251, 241, 48));

            TABLERO = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 199, 228, 203));

            PLAYER_1 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 244, 67, 54));
            PLAYER_1B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 239, 154, 154));

            PLAYER_2 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 33, 150, 243));
            PLAYER_2B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 144, 202, 249));

            PLAYER_3 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 76, 175, 80));
            PLAYER_3B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 165, 214, 167));

            PLAYER_1 = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 235, 59));
            PLAYER_1B = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 245, 157));


        }


    }
}