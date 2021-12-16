using System;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
using FavoriteLocations.Views;
using SQLite;
using Xamarin.Forms;

namespace FavoriteLocations.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;

        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                RaisePropertyChanged(nameof(IsInfoValid));
            }
        }
        
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(nameof(IsInfoValid));
            }
        }
        
        public bool IsInfoValid => !string.IsNullOrEmpty(Email) 
                                   && !string.IsNullOrEmpty(Password);

        public Command LoginCommand { get; }
        public Command GoToCreateAccountCommand { get; }

        public LoginViewModel()
        {
            _alertService = DependencyService.Resolve<IAlertService>();

            LoginCommand = new Command<bool>(TryLogin, EntriesAreValid);
            GoToCreateAccountCommand = new Command(
                () => App.Current.MainPage.Navigation.PushAsync(new CreateAccountView()));
        }

        private bool EntriesAreValid(bool isInfoValid) => isInfoValid;

        private async void TryLogin(bool isInfoValid)
        {
            if (!isInfoValid)
            {
                await _alertService.ShowAsync("Erreur", "Veuillez vous assurez que les champs ne sont pas vides.",
                    "Fermer");
                return;
            }
            
            try
            {
                await Auth.LoginUser(Email, Password);
                CreateDefaultUserConfiguration();
                await App.Current.MainPage.Navigation.PushAsync(new MainView());
            }
            catch (Exception e)
            {
                await _alertService.ShowAsync("Erreur", e.Message, "Fermer");
            }
        }

        private void CreateDefaultUserConfiguration()
        {
            using (var conn = new SQLiteConnection(App.DbPath))
            {
                var exists = conn.Table<Configuration>()
                    .Count(c => c.UserIdentifier == Auth.UserIdentifier) == 1;

                if (!exists)
                {
                    App.DefaultConfiguration.UserIdentifier = Auth.UserIdentifier;
                    conn.Insert(App.DefaultConfiguration);
                }
            }
        }
    }
}
    