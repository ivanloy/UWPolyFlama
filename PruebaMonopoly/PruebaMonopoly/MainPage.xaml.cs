using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PruebaMonopoly
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            imageCenter2.Rotate(value: 270, centerX: 0.5f, centerY: 0.5f, duration: 0, delay: 0).Start();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minNewSizeOfParentUserControl = Math.Min(e.NewSize.Height, e.NewSize.Width);
            mainGrid.Width = minNewSizeOfParentUserControl;
            mainGrid.Height = minNewSizeOfParentUserControl;
        }

        private async void Image_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Storyboard rotation = new Storyboard();
            Image image = (Image)sender;
            int row = (int)image.GetValue(Grid.RowProperty);
            int column = (int)image.GetValue(Grid.ColumnProperty);

            if (column == 0 || column == 9)
            {
                imageCenter2.Source = image.Source;
                
            }
            else
                imageCenter2.Source = image.Source;
        }

        private void Image_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Image image = (Image)sender;
            //imageCenter.Source = null;
            //imageCenter2.Source = null;
            //image.Scale(duration: 200, delay: 0, centerX: 0.5f, centerY: 0.5f, scaleX: 1f, scaleY: 1f).StartAsync();
            //image.Offset(offsetX: 0f, offsetY: 0f, duration: 200, delay: 200, easingType: EasingType.Linear);

        }
    }
}
