namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class EmergencysViewModel
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public EmergencysViewModel()
        {

        }
        #endregion

        #region Commands
        public ICommand AmbulanceAlertCommand
        {
            get
            {
                return new RelayCommand(AmbulanceAlert);
            }
        }

        private async void AmbulanceAlert()
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Usted a generado una alerta de ambulancia",
                    "Accept");
            return;
        }

        public ICommand FamilyAlertCommand
        {
            get
            {
                return new RelayCommand(FamilyAlert);
            }
        }

        private async void FamilyAlert()
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Usted a generado una alerta para su familia",
                    "Accept");
            return;
        }

        public ICommand PoliceAlertCommand
        {
            get
            {
                return new RelayCommand(PoliceAlert);
            }
        }

        private async void PoliceAlert()
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Usted a generado una alerta para la policia",
                    "Accept");
            return;
        }

        public ICommand PrincipalContactAlertCommand
        {
            get
            {
                return new RelayCommand(PrincipalContactAlert);
            }
        }

        private async void PrincipalContactAlert()
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Usted a generado una alerta para el contacto de emergencia",
                    "Accept");
            return;
        }
        #endregion
    }
}
