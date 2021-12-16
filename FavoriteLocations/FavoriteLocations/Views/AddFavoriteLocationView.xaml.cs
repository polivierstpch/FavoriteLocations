using FavoriteLocations.Models;
using FavoriteLocations.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FavoriteLocations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFavoriteLocationView : ContentPage
    {
        public AddFavoriteLocationView()
        {
            InitializeComponent();
        }
        
        public AddFavoriteLocationView(FavoriteLocation location)
        {
            InitializeComponent();
            (Resources["Vm"] as AddOrModifyFavoriteLocationViewModel)?.InitializeWithLocation(location);
        }
    }
}