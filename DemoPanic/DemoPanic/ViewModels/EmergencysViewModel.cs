namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System;
    using DemoPanic.Interface;
    using DemoPanic.Views;
    using DemoPanic.Services;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using DemoPanic.Models;

    public class EmergencysViewModel
    {
        #region Attributes
        #endregion

        #region Services
        GeolocatorService geolocatorService;
        private ApiService apiService;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public EmergencysViewModel()
        {
            this.apiService = new ApiService();
            geolocatorService = new GeolocatorService();
            this.saveCurrentPosittion();
        }

        private async void saveCurrentPosittion()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude == 0 ||
                geolocatorService.Longitude == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se a logrado ubicar tu localizacion",
                    "Aceptar");
                return;
            }

            var user = MainViewModel.GetInstance().User;
            user.Latitude = geolocatorService.Latitude.ToString("0.000000");
            user.Longitude = geolocatorService.Longitude.ToString("0.000000");

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Put(
                apiSecurity,
                "/api",
                "/Users",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                user);

            if (!response.IsSuccess)
            {
               await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Confirmación",
                "Tu pocicion esta siendo rastreada por nuestros sistemas",
                "Aceptar");

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
            MainViewModel.GetInstance().Ubications = new UbicationsViewModel();

            var ubications = GetUbications(2);

            await App.Navigator.PushAsync(new UbicationsPage());
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
            MainViewModel.GetInstance().Ubications = new UbicationsViewModel();
            await App.Navigator.PushAsync(new UbicationsPage());
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

        private async Task<List<Ubication>> GetUbications(int clientTypeId)
        {
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var user = await this.apiService.GetUsersByClientType(
                apiSecurity,
                "/api",
                "/Users/GetUsersByClientType",
                MainViewModel.GetInstance().Token.TokenType,
                MainViewModel.GetInstance().Token.AccessToken,
                clientTypeId);
            return user;
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
