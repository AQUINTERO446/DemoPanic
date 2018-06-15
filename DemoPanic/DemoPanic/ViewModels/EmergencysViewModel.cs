namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System;
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
            if (await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Desea llamar a la ambulancia",
                    "Si",
                    "No"))
            {
                OnCall("*111");
            }
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
            if (await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Desea llamar a la Familia",
                    "Si",
                    "No"))
            {
                OnCall("*321");
            }
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
            if (await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Desea llamar a la policia",
                    "Si",
                    "No"))
            {
                OnCall("*123");
            }
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
            if (await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Desea llamar al contacto de emergencia",
                    "Si",
                    "No"))
            {
                OnCall("*611");
            }
            return;
        }
        #endregion

        #region Methods
        async void OnCall(string number)
        {
            var dialer = DependencyService.Get<IDialer>();
            if (dialer != null)
                await dialer.DialAsync(number);
        }
        #endregion
    }
}
