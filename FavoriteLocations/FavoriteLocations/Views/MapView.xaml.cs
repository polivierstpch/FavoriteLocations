using FavoriteLocations.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FavoriteLocations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentPage
    {
        private FavoriteLocation _selectedLocation;
        
        public MapView()
        {
            InitializeComponent();
        }
        
        public void Initialize(FavoriteLocation location)
        {
            _selectedLocation = location;
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _selectedLocation = null;
        }
    }
}