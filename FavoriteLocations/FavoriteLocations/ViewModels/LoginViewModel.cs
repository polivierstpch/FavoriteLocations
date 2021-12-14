using System;
using System.Windows.Input;
using FavoriteLocations.Services;
using Xamarin.Forms;

namespace FavoriteLocations.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                (LoginCommand as Command).ChangeCanExecute();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                (LoginCommand as Command).ChangeCanExecute();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToCreateAccountCommand { get; }

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
                await App.Current.MainPage.Navigation.PushAsync(new MainView());
            }
            catch (Exception e)
            {
                await _alertService.ShowAsync("Erreur", e.Message, "Fermer");
            }
        }
    }
}
    