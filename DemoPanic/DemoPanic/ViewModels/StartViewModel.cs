namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

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
            await Application.Current.MainPage.Navigation.PushAsync(new EmergencysPage());
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


        #endregion
    }
}
