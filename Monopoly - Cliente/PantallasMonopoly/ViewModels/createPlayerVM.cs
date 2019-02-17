using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class createPlayerVM : clsVMBase
    {

        #region Propiedades privadas

        private String _nickname;
        private List<Ficha> _listadoFichas;
        private Ficha _fichaSeleccionada;

        private DelegateCommand _crearCommand;
        private INavigationService _navigationService;

        #endregion


        #region Propiedades publicas

        public String nickname
        {
            get
            {
                return _nickname;
            }

            set
            {
                _nickname = value;
                NotifyPropertyChanged("nickname");
                _crearCommand.RaiseCanExecuteChanged();
            }
        }

        public List<Ficha> listadoFichas
        {
            get
            {
                return _listadoFichas;
            }

            set
            {
                _listadoFichas = value;
                NotifyPropertyChanged("listadoFichas");
            }
        }

        public Ficha fichaSeleccionada
        {
            get
            {
                return _fichaSeleccionada;
            }

            set
            {
                _fichaSeleccionada = value;
                NotifyPropertyChanged("fichaSeleccionada");
                _crearCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion


        #region Constructores

        public createPlayerVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _nickname = "";
            _listadoFichas = generadorFichas.listadoFichas();         
            _fichaSeleccionada = new Ficha();

        }


        #endregion


        #region Crear command

        public DelegateCommand crearCommand
        {
            get
            {
                _crearCommand = new DelegateCommand(crearCommand_Executed, crearCommand_CanExecute);
                return _crearCommand;
            }
        }

        private bool crearCommand_CanExecute()
        {
            bool sePuedeCrear = false;

            if (!_nickname.Equals("") && _fichaSeleccionada.nombre != null)
            {
                
                sePuedeCrear = true;
            }

            return sePuedeCrear;
        }

        private void crearCommand_Executed()
        {
      
            _navigationService.Navigate(typeof(CreateMenu), new Jugador(_nickname, _fichaSeleccionada));

        }


        #endregion


    }
}
