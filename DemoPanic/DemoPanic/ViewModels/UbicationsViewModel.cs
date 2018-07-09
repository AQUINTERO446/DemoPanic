using DemoPanic.Models;
using DemoPanic.Services;
using DemoPanic.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace DemoPanic.ViewModels
{
    public class UbicationsViewModel : BaseViewModel
    {

        #region Constructors
        public UbicationsViewModel()
        {
            instance = this;
        }

        #endregion

        #region Properties
        public ObservableCollection<Pin> Pins
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public void LoadPins(List<Ubication> ubicationList)
        {
            Pins = new ObservableCollection<Pin>();
            foreach (var ubication in ubicationList)
            {
                Pins.Add(new Pin
                {
                    Address = ubication.Address,
                    Label = ubication.Description,
                    Position = new Position(
                        ubication.Latitude,
                        ubication.Longitude),
                    Type = PinType.Place,
                });
            }

        }
        #endregion

        #region Singleton
        private static UbicationsViewModel instance;

        public static UbicationsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new UbicationsViewModel();
            }

            return instance;
        }
        #endregion
    }
}
