using System;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
using SQLite;
using Xamarin.Forms;

namespace FavoriteLocations.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;
        
        private Configuration _configuration; 
        
        private bool _showVisitedLocations;
        private bool _showWishedLocations;
        private bool _showKnownLocations;
        private double _latitudeDegrees;
        private double _longitudeDegrees;

        public bool ShowVisitedLocations
        {
            get => _showVisitedLocations;
            set => SetProperty(ref _showVisitedLocations, value);
        }

        public bool ShowKnownLocations
        {
            get => _showKnownLocations;
            set => SetProperty(ref _showKnownLocations, value);
        }

        public bool ShowWishedLocations
        {
            get => _showWishedLocations;
            set => SetProperty(ref _showWishedLocations, value);
        }

        public double LatitudeDegrees
        {
            get => _latitudeDegrees;
            set => SetProperty(ref _latitudeDegrees, value);
            
        }

        public double LongitudeDegrees
        {
            get => _longitudeDegrees;
            set => SetProperty(ref _longitudeDegrees, value);
            
        }
        
        public ConfigurationViewModel()
        {
            _alertService = DependencyService.Resolve<IAlertService>();
            _configuration = new Configuration();
            
            LoadConfiguration();
        }
        
        public void UpdateConfiguration()
        {
            _configuration.ShowVisitedLocations = ShowVisitedLocations;
            _configuration.ShowKnownLocations = ShowKnownLocations;
            _configuration.ShowWishedLocations = ShowWishedLocations;
            _configuration.LatitudeDegrees = LatitudeDegrees;
            _configuration.LongitudeDegrees = LongitudeDegrees;
            _configuration.UserIdentifier = Auth.UserIdentifier;
            
            using (var conn = new SQLiteConnection(App.DbPath))
            {
                var exists = conn.Table<Configuration>()
                    .Count(c => c.UserIdentifier == Auth.UserIdentifier) == 1;
                
                if (!exists)
                {
                    
                    conn.Insert(_configuration);
                    return;
                }

                conn.Update(_configuration);
            }
        }
        
        private void LoadConfiguration()
        {
            try
            {
                using (var conn = new SQLiteConnection(App.DbPath))
                {
                    _configuration = conn.Get<Configuration>(Auth.UserIdentifier);
                }

                ShowKnownLocations = _configuration.ShowKnownLocations;
                ShowVisitedLocations = _configuration.ShowVisitedLocations;
                ShowWishedLocations = _configuration.ShowWishedLocations;
                LatitudeDegrees = _configuration.LatitudeDegrees;
                LongitudeDegrees = _configuration.LongitudeDegrees;
            }
            catch (InvalidOperationException)
            {
                _alertService.ShowAsync("Erreur", "Votre configuration n'a pas été trouvée. utilisation de la configuration par défaut.", "Fermer");
                
                App.DefaultConfiguration.UserIdentifier = Auth.UserIdentifier;
                _configuration = App.DefaultConfiguration;
            }
        }
    }
}