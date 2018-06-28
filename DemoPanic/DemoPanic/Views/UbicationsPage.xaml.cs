namespace DemoPanic.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;
    using Xamarin.Forms.Xaml;
    using ViewModels;
    using System;
    using System.Globalization;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UbicationsPage : ContentPage
	{

        #region Constructors
        public UbicationsPage()
        {
            InitializeComponent();
            MoveMapToCurrentPosition();
        }
        #endregion

        #region Methods
        void MoveMapToCurrentPosition()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            var user = MainViewModel.GetInstance().User;
            var position = new Position(
                    Convert.ToDouble(user.Latitude, provider),
                    Convert.ToDouble(user.Longitude, provider));
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(6.5)));
            
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