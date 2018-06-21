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
            if (this.PageName == "WorkerPage")
            {
                //Application.Current.MainPage = new NavigationPage(
                  //  new WorkerPage());
            }
            else if (this.PageName == "SettingsPage")
            {
                //Application.Current.MainPage = new NavigationPage(
                    //new SettingsPage());
            }
            else if (this.PageName == "LoginPage")
            {
                Settings.Token = string.Empty;
                Settings.TokenType = string.Empty;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = string.Empty;
                mainViewModel.TokenType = string.Empty;
                mainViewModel.Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(
                    new LoginPage());
            }
        }
        #endregion
    }
}
