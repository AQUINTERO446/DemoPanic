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

        private void RefreshLocation_Clicked(object sender, EventArgs e)
        {
            RefreshLocation();
        }

        public async void RefreshLocation()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                UserLocationLabel.Text = String.Format(
                    "Lat. : {0:0.000000} / Long. : {1:0.000000}",
                    geolocatorService.Latitude,
                    geolocatorService.Longitude);

            }
        }

        #endregion

    }
}