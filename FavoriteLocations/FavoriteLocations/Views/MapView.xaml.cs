using System;
using System.Collections.Generic;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FavoriteLocations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentPage
    {
        private readonly IGeolocator _locator = CrossGeolocator.Current;
        private Configuration _configuration;

        private FavoriteLocation _selectedLocation;
        
        public MapView()
        {
            InitializeComponent();
            LoadConfiguration();
            GetLocation();
        }

        public void LoadWithLocation(FavoriteLocation location)
        {
            _selectedLocation = location;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            LocationsMap.Pins.Clear();
            
            StartLocalization();
            LoadConfiguration();
            
            LoadFavoriteLocationsOnMap();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            _selectedLocation = null;
            StopLocalization();
        }


        private void LoadFavoriteLocationsOnMap()
        {
            if (_selectedLocation != null)
            {
                LoadSelectedLocationOnMap();
                return;
            }

            var locations = new List<FavoriteLocation>();

            using (var conn = new SQLiteConnection(App.DbPath))
            {
                if (_configuration.ShowKnownLocations)
                    AddLocationsFromCategory(conn, locations, Category.Known);
                if (_configuration.ShowVisitedLocations)
                    AddLocationsFromCategory(conn, locations, Category.Visited);
                if (_configuration.ShowWishedLocations)
                    AddLocationsFromCategory(conn, locations, Category.Wished);
            }

            foreach (var location in locations)
            {
                try
                {
                    AddPinForLocationOnMap(location);
                }
                catch (Exception)
                {
                }
            }

        }

        private void LoadSelectedLocationOnMap()
        {
            if (_selectedLocation == null)
                return;
            
            try
            {
                AddPinForLocationOnMap(_selectedLocation);
                CenterMapToLocation(_selectedLocation.Latitude, _selectedLocation.Longitude);
            }
            catch (Exception)
            {
            }
        }
        
        private void AddLocationsFromCategory(SQLiteConnection conn, List<FavoriteLocation> locations, Category category)
        {
            var locationsToAdd = conn.Table<FavoriteLocation>()
                .Where(fl => fl.UserIdentifier == Auth.UserIdentifier && fl.Category == category)
                .ToList();
            
            locations.AddRange(locationsToAdd);
        }
        
        private void AddPinForLocationOnMap(FavoriteLocation location)
        {
            var pinPosition = new Xamarin.Forms.Maps.Position(location.Latitude, location.Longitude);
            var pin = new Pin
            {
                Position = pinPosition,
                Label = location.Name,
                Address = location.Address,
                Type = PinType.Place
            };
            LocationsMap.Pins.Add(pin);
        }
        
        private async void GetLocation()
        {
            var status = await App.ValidateAndAskForLocation();

            if (status == PermissionStatus.Granted)
            {
                var location = await Geolocation.GetLocationAsync();
                CenterMapToLocation(location.Latitude, location.Longitude);
            }
        }
        
        private void LoadConfiguration()
        {
            using (var conn = new SQLiteConnection(App.DbPath))
            {
                var config = conn.Table<Configuration>()
                    .FirstOrDefault(c => c.UserIdentifier == Auth.UserIdentifier);

                if (config == null)
                {
                    DisplayAlert("Erreur",
                        "Votre configuration n'a pas été trouvée. la configuration par défaut sera chargée.", "Fermer");
                    _configuration = App.DefaultConfiguration;
                    return;
                }
                
                _configuration = config;
            }
        }

        private void CenterMapToLocation(double latitude, double longitude)
        {
            var center = new Xamarin.Forms.Maps.Position(latitude, longitude);
            var span = new MapSpan(center, _configuration.LatitudeDegrees, _configuration.LongitudeDegrees);
            LocationsMap.MoveToRegion(span);
        }
        
        private void LocatorPositionChanged(object sender, PositionEventArgs e)
        {
            CenterMapToLocation(e.Position.Latitude, e.Position.Longitude);
        }
        
        private async void StartLocalization()
        {
            await _locator.StartListeningAsync(new TimeSpan(0, 0, 5), 50);
        }

        private async void StopLocalization()
        {
            await _locator.StopListeningAsync();
        }
    }
}
