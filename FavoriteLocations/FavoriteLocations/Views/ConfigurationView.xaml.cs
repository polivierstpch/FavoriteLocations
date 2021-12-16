using FavoriteLocations.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FavoriteLocations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationView : ContentPage
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (Resources["Vm"] as ConfigurationViewModel)?.UpdateConfiguration();
        }
    }
}