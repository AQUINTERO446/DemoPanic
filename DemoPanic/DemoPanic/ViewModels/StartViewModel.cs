using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPanic.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        #region Attributes
        #endregion

        #region Properties
        public string WelcomeMessage { get; set; }
        #endregion

        #region Constructors
        public StartViewModel()
        {
            this.WelcomeMessage = "Que accion desea realizar";
        }
        #endregion

        #region Commands
        #endregion
    }
}
