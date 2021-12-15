using System;
using System.Linq;
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
                RefreshCanExecute();
            }
        }
        
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RefreshCanExecute();
            }
        }

        public Command LoginCommand { get; }
        public Command GoToCreateAccountCommand { get; }

        public LoginViewModel()
        {
            _alertService = DependencyService.Resolve<IAlertService>();

            LoginCommand = new Command(TryLogin, EntriesAreValid);
            GoToCreateAccountCommand =
                new Command(() => App.Current.MainPage.Navigation.PushAsync(new CreateAccountView()));
        }

        private bool EntriesAreValid()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        private async void TryLogin()
        {
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
                    .Count(c => c.UserIdentifier == Auth.UserIdentifier) >= 1;

                if (!exists)
                    conn.Insert(App.DefaultConfiguration);
            }
        }
        
        private void RefreshCanExecute()
        {
            LoginCommand.ChangeCanExecute();
        }
    }
}
    