

//[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace DemoPanic
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Views;
    using DemoPanic.ViewModels;
    using DemoPanic.Helpers;
    using DemoPanic.Services;
    using DemoPanic.Models;

    public partial class App : Application
	{
        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }
        #endregion

        #region Constructors
        public App()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(Settings.Token))
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var dataService = new DataService();
                var user = dataService.First<UserLocal>(false);
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = Settings.Token;
                mainViewModel.TokenType = Settings.TokenType;
                mainViewModel.User = user;
                mainViewModel.Start = new StartViewModel();
                Application.Current.MainPage = new MasterPage();
            }
        }
        #endregion

        #region Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        } 
        #endregion
    }
}
