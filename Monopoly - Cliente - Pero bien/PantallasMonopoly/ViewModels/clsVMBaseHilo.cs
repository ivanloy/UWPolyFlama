using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.ViewModels
{
    public abstract class clsVMBaseHilo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void NotifyPropertyChanged(string propertyName = null)
        {
            await Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }
    }
}
