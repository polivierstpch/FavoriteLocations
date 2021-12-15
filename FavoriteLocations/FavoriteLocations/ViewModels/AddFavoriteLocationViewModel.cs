using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
using SQLite;
using Xamarin.Forms;

namespace FavoriteLocations.ViewModels
{
    public class AddFavoriteLocationViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;
        
        private int _locationId;

        private string _title;
        private string _name;
        private string _address;
        private double _latitude;
        private double _longitude;
        private PickerItem<Category> _selectedCategory;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                RefreshCanExecute();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                SetProperty(ref _address, value);
                RefreshCanExecute();
            }
        }

        public double Latitude
        {
            get => _latitude;
            set
            {
                SetProperty(ref _latitude, value);
                RefreshCanExecute();
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                SetProperty(ref _longitude, value);
                RefreshCanExecute();
            }
        }

        public PickerItem<Category> SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                RefreshCanExecute();
            }
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<PickerItem<Category>> Categories { get; }

        public Command RegisterCommand { get; }

        public AddFavoriteLocationViewModel()
        {
            _alertService = DependencyService.Resolve<IAlertService>();
            Title = "Nouveau Lieu";
            RegisterCommand = new Command(TryRegisterLocation, CanRegisterLocation);
            Categories = new ObservableCollection<PickerItem<Category>>();

            PopulateCategoryPickerItems();
        }

        private bool CanRegisterLocation()
        {
            return !string.IsNullOrEmpty(Name)
                   && !string.IsNullOrEmpty(Address)
                   && SelectedCategory != null
                   && Latitude >= -90 && Latitude <= 90
                   && Longitude >= -90 && Longitude <= 90;
        }

        private async void TryRegisterLocation()
        {
            using (var conn = new SQLiteConnection(App.DbPath))
            {
                if (_locationId != 0)
                {
                    await UpdateLocation(conn);
                    await _alertService.ShowAsync("Info", "Le lieu a bien été modifié.", "Fermer");
                }
                else
                {
                    await InsertNewLocation(conn);
                    await _alertService.ShowAsync("Info", "Le lieu a bien été ajouté à vos lieux favoris.", "Fermer");
                }
            }
            
            await App.Current.MainPage.Navigation.PopAsync();
        }
        
        private void PopulateCategoryPickerItems()
        {
            var categories = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .ToList();

            foreach (var category in categories)
            {
                string displayText;
                switch (category)
                {
                    case Category.Visited:
                        displayText = "Visité";
                        break;
                    case Category.Wished:
                        displayText = "Souhaité";
                        break;
                    case Category.Known:
                        displayText = "Connu";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Categories.Add(PickerItem<Category>.Create(displayText, category));
            }

            SelectedCategory = Categories.FirstOrDefault();
        }

        private void RefreshCanExecute()
        {
            RegisterCommand.ChangeCanExecute();
        }

        public void InitializeWithLocation(FavoriteLocation location)
        {
            Title = "Modifier Lieu";
            
            _locationId = location.Id;
            Name = location.Name;
            Address = location.Address;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            SelectedCategory = Categories.FirstOrDefault(c => c.Value == location.Category);
        }
        
        private async Task UpdateLocation(SQLiteConnection conn)
        {
            var exists = conn
                .Table<FavoriteLocation>()
                .Count(fl => fl.Id == _locationId 
                             && fl.UserIdentifier == Auth.UserIdentifier) == 1;

            if (!exists)
            {
                await _alertService.ShowAsync("Erreur", "Erreur d'obtention de lieu. Veuillez réessayer.", "Fermer");
                return;
            }
            
            var location = conn.Get<FavoriteLocation>(_locationId);

            location.Name = Name;
            location.Address = Address;
            location.Latitude = Latitude;
            location.Longitude = Longitude;
            location.Category = SelectedCategory.Value;

            conn.Update(location);
        }

        private async Task InsertNewLocation(SQLiteConnection conn)
        {
            var exists = conn
                .Table<FavoriteLocation>()
                .Count(fl => fl.Name == Name
                             && fl.Address == Address
                             && fl.UserIdentifier == Auth.UserIdentifier) == 1;

            if (exists && _locationId == 0)
            {
                await _alertService.ShowAsync("Erreur",
                    "Un lieux avec ce nom et cette adresse existe déjà. Veuillez saisir d'autres informations,",
                    "Fermer");
                return;
            }

            var newLocation = new FavoriteLocation
            {
                UserIdentifier = Auth.UserIdentifier,
                Name = Name,
                Address = Address,
                Category = SelectedCategory.Value,
                Latitude = Latitude,
                Longitude = Longitude
            };

            conn.Insert(newLocation);
        }
    }
}