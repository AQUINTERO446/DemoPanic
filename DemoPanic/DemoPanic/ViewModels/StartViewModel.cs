namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;
    using System;

    public class StartViewModel
    {
        #region Attributes

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public StartViewModel()
        {
            
        }
        #endregion

        #region Commands
        public ICommand GeneralAlertCommand
        {
            get
            {
                return new RelayCommand(GeneralAlert);
            }
        }

        private async void GeneralAlert()
        {
            MainViewModel.GetInstance().Emergencys = new EmergencysViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage (new EmergencysPage()));
        }

        public ICommand VoiceAlertCommand
        {
            get
            {
                return new RelayCommand(VoiceAlert);
            }
        }

        private async void VoiceAlert()
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Usted a generado una alerta por voz",
                    "Accept");
            return;
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            MainViewModel.GetInstance().Login = new LoginViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
        #endregion
    }
}
