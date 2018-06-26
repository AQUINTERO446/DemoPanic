namespace DemoPanic.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;
    using Helpers;

    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion
        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            App.Master.IsPresented = false;
            if (this.PageName == "WorkerPage")
            {
                MainViewModel.GetInstance().Worker = new WorkerViewModel();
                App.Navigator.PushAsync(new WorkerPage());
            }
            else if (this.PageName == "SettingsPage")
            {
                MainViewModel.GetInstance().Settings = new SettingsViewModel();
                App.Navigator.PushAsync(new SettingsPage());
            }
            else if (this.PageName == "LoginPage")
            {
                Settings.IsRemembered = "false";
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = null;
                mainViewModel.User = null;
                mainViewModel.Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(
                    new LoginPage());
            }
            else if (this.PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                App.Navigator.PushAsync(new MyProfilePage());
            }
        }
        #endregion
    }
}
