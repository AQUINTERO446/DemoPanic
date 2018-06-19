using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using DemoPanic.Views;

namespace DemoPanic.ViewModels
{
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

        private async void Navigate()
        {
            if (this.PageName == "WorkerPage")
            {
                MainViewModel.GetInstance().Worker = new WorkerViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new WorkerPage()));
            }
            else if (this.PageName == "SettingsPage")
            {
                MainViewModel.GetInstance().Settings = new SettingsViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new SettingsPage()));
            }
            else if (this.PageName == "StartPage")
            {
                //MainViewModel.GetInstance().Start = new StartViewModel();
                //await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new StartPage()));
            }
        }
        #endregion
    }
}
