using DemoPanic.Models;
using DemoPanic.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Maps;

namespace DemoPanic.ViewModels
{
    public class UbicationsViewModel
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
        public void LoadPins()
        {
            //Aqui response tendra la lista de Ubicaciones
            var ubications = GetListUbications();

            Pins = new ObservableCollection<Pin>();
            foreach (var ubication in ubications)
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

        private List<Ubication> GetListUbications()
        {
            const double MAXIMUM_LATITUD = 7.142354;
            const double MINIMUM_LATITUD = 7.058251;
            const double MAXIMA_LONGITUDD = -73.076229;
            const double MINIMUM_LONGITUD = -73.180674;

            Random rnd = new Random();

            List<Ubication> list = new List<Ubication>();

            int disponibles = rnd.Next(3, 11);

            for (int i = 1; i <= disponibles; i++)
            {
                list.Add( new Ubication
                    {
                        Address = "Direccion-"+ i,
                        Description = "Descripcion "+ i,
                        Latitude = 
                            rnd.NextDouble() * (MAXIMUM_LATITUD - MINIMUM_LATITUD) + MINIMUM_LATITUD,
                        Longitude = 
                            rnd.NextDouble() * (MAXIMA_LONGITUDD - MINIMUM_LONGITUD) + MINIMUM_LONGITUD,
                        Phone = i + "-666-000",
                        UbicationId = i-1
                    }
                );
            }
            return list;
        }
        /*
        public ICommand RefreshLocationCommand
        {
            get
            {
                return new RelayCommand(RefreshLocation);
            }
        }

        public void RefreshLocation()
        {
            
        }
        */
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
