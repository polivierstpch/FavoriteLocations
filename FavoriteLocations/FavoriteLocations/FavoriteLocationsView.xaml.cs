using FavoriteLocations.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FavoriteLocations
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteLocationsView : ContentPage
    {
        public FavoriteLocationsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (Resources["Vm"] as FavoriteLocationsViewModel)?.Initialize();
        }
    }
}