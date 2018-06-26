namespace DemoPanic.ViewModels
{
    using DemoPanic.Helpers;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MyProfileViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Propiedades
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public UserLocal User
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public MyProfileViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.User = MainViewModel.GetInstance().User;
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar nombre(s)",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar apellido(s).",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un email.",
                   "Aceptar");
                return;
            }

            if (!RegexUtilities.IsValidEmail(this.User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un email válido.",
                   "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.User.Telephone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un teléfono.",
                   "Aceptar");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    checkConnetion.Message,
                   "Aceptar");
                return;
            }
            /*

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }
            */
            var userDomain = Converter.ToUserDomain(this.User);
            
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Put(
                apiSecurity,
                "/api",
                "/Users",
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token,
                this.User);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                   "Aceptar");
                return;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                "Confirmación",
                "Usuario modificado exitosamente.",
               "Aceptar");

            var userApi = await this.apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token,
                this.User.Email);

            var userLocal = Converter.ToUserLocal(userApi);

            MainViewModel.GetInstance().User = userLocal;
            this.dataService.Update(userLocal);

            await App.Navigator.PopAsync();
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    "¿De donde quieres tomar la imagen?",
                    "Cancelar",
                    null,
                    "Desde la galería",
                    "Desde la cámara");

                if (source == "Cancelar")
                {
                    this.file = null;
                    return;
                }

                if (source == "Desde la cámara")
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
        #endregion
    }
}
