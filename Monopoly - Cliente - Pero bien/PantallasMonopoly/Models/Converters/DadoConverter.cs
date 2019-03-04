using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PantallasMonopoly.Models
{
    class DadoConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int dado = (int)value;
            switch (dado)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException(); //Jaja para que quieres eso salu2
        }
    }
}
