

//[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace DemoPanic
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Views;
    using DemoPanic.ViewModels;
    using DemoPanic.Helpers;

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
            MainViewModel.GetInstance().Login = new LoginViewModel();
            this.MainPage = new NavigationPage(new LoginPage());

            //if (string.IsNullOrEmpty(Settings.Token))
            //{
            //    MainViewModel.GetInstance().Login = new LoginViewModel();
            //    this.MainPage = new NavigationPage(new LoginPage());
            //}
            //else
            //{
            //    var mainViewModel = MainViewModel.GetInstance();
            //    mainViewModel.Token = Settings.Token;
            //    mainViewModel.TokenType = Settings.TokenType;
            //    mainViewModel.Start = new StartViewModel();
            //    Application.Current.MainPage = new MasterPage();
            //}
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
