using System.Collections.ObjectModel;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
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
            set
            {
                SetProperty(ref _selectedLocation, value);
                RefreshCanExecute();
            }
        }
        
        public ObservableCollection<FavoriteLocation> Locations { get; set; }

        public Command ModifyLocationCommand { get; }
        public Command GoToLocationOnMapCommand { get; }
        public Command GoToAddLocationCommand { get; }

        public FavoriteLocationsViewModel()
        {
            ModifyLocationCommand = new Command(() => { }, LocationWasSelected);
            GoToLocationOnMapCommand = new Command(() => { }, LocationWasSelected);
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
        
        private bool LocationWasSelected() => SelectedLocation != null;
        
        private void RefreshCanExecute()
        {
            ModifyLocationCommand.ChangeCanExecute();
            GoToLocationOnMapCommand.ChangeCanExecute();
        }
    }
}