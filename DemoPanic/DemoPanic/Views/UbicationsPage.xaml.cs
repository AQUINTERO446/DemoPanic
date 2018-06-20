

namespace DemoPanic.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;
    using Xamarin.Forms.Xaml;
    using ViewModels;
    using System;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UbicationsPage : ContentPage
	{
        #region Services
        GeolocatorService geolocatorService;
        #endregion

        #region Constructors
        public UbicationsPage()
        {
            InitializeComponent();

            geolocatorService = new GeolocatorService();

            MoveMapToCurrentPosition();
        }
        #endregion

        #region Methods
        async void MoveMapToCurrentPosition()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    geolocatorService.Latitude,
                    geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(6.5)));
            }

            LoadPins();

        }
        /// <summary>
        /// Load Pins in to maps
        /// </summary>
        private void LoadPins()
        {
            var ubicationsViewModel = UbicationsViewModel.GetInstance();
            ubicationsViewModel.LoadPins();

            foreach (var pin in ubicationsViewModel.Pins)
            {
                MyMap.Pins.Add(pin);
            }
        }
        #endregion

    }
}