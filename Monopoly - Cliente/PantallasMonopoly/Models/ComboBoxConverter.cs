using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PantallasMonopoly.Models
{
    public class ComboBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {

            ComboBoxItem item = (ComboBoxItem) value;

            TextBlock text = (TextBlock) item.Content;

            int i;

            Int32.TryParse(text.Text.ToString(), out i);

            return i;
        }
    }
}
