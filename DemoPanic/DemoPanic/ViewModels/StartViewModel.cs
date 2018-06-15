namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;

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
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Usted a generado una alerta general",
                    "Accept");
            return;
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
