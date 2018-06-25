namespace DemoPanic.ViewModels
{
    using System.Windows.Input;
    using Domain;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using Xamarin.Forms;

    public class RegisterViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties
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

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Telephone
        {
            get;
            set;
        }

        public string Password
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
        public RegisterViewModel()
        {
            this.apiService = new ApiService();

            this.IsEnabled = true;
            this.ImageSource = "no_image";
        }
        #endregion

        #region Methods
        #endregion 

        #region Commands
        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar nombre(s)",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar apellido(s).",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un email.",
                   "Aceptar");
                return;
            }

            if (!RegexUtilities.IsValidEmail(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un email válido.",
                   "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Telephone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar un teléfono.",
                   "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar una contraseña.",
                   "Aceptar");
                return;
            }

            if (this.Password.Length < 6)
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

            if (this.Password != this.Confirm)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña y la confirmación no concuerdan.",
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
            var user = new User
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Telephone = this.Telephone,
                UserTypeId = 1,
                Password = this.Password,
            };

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Post(
                apiSecurity,
                "/api",
                "/Users",
                user);

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
                "Usuario registrado, ya puedes ingresar con este email y clave asignada.",
               "Aceptar");
            await Application.Current.MainPage.Navigation.PopAsync();
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