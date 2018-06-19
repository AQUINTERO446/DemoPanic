

//[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace DemoPanic
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Views;
    using DemoPanic.ViewModels;

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
            //var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.Start = new StartViewModel();
            Application.Current.MainPage = new NavigationPage(new MasterPage());
            //this.MainPage = new NavigationPage(new MasterPage());
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
