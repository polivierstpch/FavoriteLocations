using System.Collections.ObjectModel;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
using FavoriteLocations.Views;
using SQLite;
using Xamarin.Forms;

namespace FavoriteLocations.ViewModels
{
    public class FavoriteLocationsViewModel : BaseViewModel
    {
        private FavoriteLocation _selectedLocation;
        
        public FavoriteLocation SelectedLocation
        {
            get => _selectedLocation;
            set => SetProperty(ref _selectedLocation, value);
            
        }
        
        public ObservableCollection<FavoriteLocation> Locations { get; set; }

        public Command ModifyLocationCommand { get; }
        public Command GoToLocationOnMapCommand { get; }
        public Command GoToAddLocationCommand { get; }

        public FavoriteLocationsViewModel()
        {
            ModifyLocationCommand = new Command<FavoriteLocation>(location => 
                App.Current.MainPage.Navigation.PushAsync(new AddFavoriteLocationView(location)),
                LocationWasSelected);
            GoToLocationOnMapCommand = new Command<FavoriteLocation>(NavigateToMapView, LocationWasSelected);
            GoToAddLocationCommand = new Command(() =>
                App.Current.MainPage.Navigation.PushAsync(new AddFavoriteLocationView()));

            Locations = new ObservableCollection<FavoriteLocation>();
        }

        public void Initialize()
        {
            SelectedLocation = null;
            Locations.Clear();
            
            using (var conn = new SQLiteConnection(App.DbPath))
            {
                var userLocations = conn.Table<FavoriteLocation>()
                    .Where(fl => fl.UserIdentifier == Auth.UserIdentifier)
                    .ToList();

                userLocations.ForEach(fl => Locations.Add(fl));
            }
        }

        private void NavigateToMapView(FavoriteLocation location)
        {
            if (!((App.Current.MainPage as NavigationPage)?.CurrentPage is TabbedPage tabbedPage)) 
                return;
            
            if (!(tabbedPage.Children[1] is MapView mapView))
                return;
            
            mapView.Initialize(location);
            tabbedPage.CurrentPage = tabbedPage.Children[1];
        }
        
        private bool LocationWasSelected(FavoriteLocation location) => location != null;
        
    }
}