using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPanic.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        public StartViewModel Start
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
