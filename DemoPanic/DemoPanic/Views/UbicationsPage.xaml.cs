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

        #region Constructors
        public UbicationsPage()
        {
            InitializeComponent();
            MoveMapToCurrentPosition();
        }
        #endregion

        #region Properties
        
        #endregion

        #region Methods
        void MoveMapToCurrentPosition()
        {
            var user = MainViewModel.GetInstance().User;
            if (user.Longitude != null && user.Latitude != null)
            {
                var position = new Position(
                    Convert.ToDouble(user.Latitude),
                    Convert.ToDouble(user.Longitude));
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(6.5)));
            }
            //de lo contrario espere por que el metodo de saveCurrentPosittion() aun no termina

            LoadPins();
        }
        /// <summary>
        /// Load Pins in to maps
        /// </summary>
        private void LoadPins()
        {
            var ubicationsViewModel = UbicationsViewModel.GetInstance();

            foreach (var pin in ubicationsViewModel.Pins)
            {
                MyMap.Pins.Add(pin);
            }
        }
        
        #endregion

    }
}