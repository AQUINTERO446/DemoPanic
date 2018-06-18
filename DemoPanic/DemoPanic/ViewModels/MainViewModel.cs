using DemoPanic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPanic.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public TokenResponse Token { get; set; }
        #endregion

        #region ViewModels
        public StartViewModel Start
        {
            get;
            set;
        }
        public EmergencysViewModel Emergencys
        {
            get;
            set;
        }
        public LoginViewModel Login
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Start = new StartViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
