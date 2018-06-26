namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public string CurrentPassword
        {
            get;
            set;
        }

        public string NewPassword
        {
            get;
            set;
        }

        public string Confirm
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ChangePasswordViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();

            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand
        {
            get
            {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(this.CurrentPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar una contraseña.",
                    "Aceptar");
                return;
            }

            if (this.CurrentPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña debe tener al menos sies (6) carácteres.",
                    "Aceptar");
                return;
            }

            if (this.CurrentPassword != MainViewModel.GetInstance().User.Password)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña actual es incorrecta",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar una contraseña.",
                    "Aceptar");
                return;
            }

            if (this.NewPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña debe tener al menos sies (6) carácteres.",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Confirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar la confirmación de la clave.",
                    "Aceptar");
                return;
            }

            if (this.NewPassword != this.Confirm)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña y la confirmación no concuerdan.",
                    "Aceptar");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var request = new ChangePasswordRequest
            {
                CurrentPassword = this.CurrentPassword,
                Email = MainViewModel.GetInstance().User.Email,
                NewPassword = this.NewPassword,
            };

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.ChangePassword(
                apiSecurity,
                "/api",
                "/Users/ChangePassword",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                request);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña no pudo ser cambiada, por favor intenta más tarde.",
                    "Aceptar");
                return;
            }

            MainViewModel.GetInstance().User.Password = this.NewPassword;
            this.dataService.Update(MainViewModel.GetInstance().User);

            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                "Confirmación",
                "La contraseña fue cambiada exitosamente.",
                "Aceptar");
            await App.Navigator.PopAsync();
        }
        #endregion
    }
}