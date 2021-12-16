using System;
using System.Windows.Input;
using FavoriteLocations.Services;
using Xamarin.Forms;

namespace FavoriteLocations.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                RaisePropertyChanged(nameof(IsInfoValid));
            }
        }

        private string _password;
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
        
        public ICommand CreateAccountCommand { get; }
        public ICommand GoBackToLoginCommand { get; }

        public CreateAccountViewModel()
        {
            _alertService = DependencyService.Resolve<IAlertService>();

            CreateAccountCommand = new Command<bool>(TryCreateAccount, ValidateUserInfo);
            GoBackToLoginCommand = new Command(ConfirmLeaveCreateAccount);
        }

        private bool ValidateUserInfo(bool infoIsValid) => infoIsValid;

        private async void ConfirmLeaveCreateAccount()
        {
            var confirmLeave = await _alertService.ShowConfirmAsync(
                "Attention",
                "Voulez-vous quitter la page sans enregistrer les informations?",
                "Oui",
                "Non");

            if (confirmLeave)
                await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void TryCreateAccount(bool infoIsValid)
        {
            if (!infoIsValid)
            {
                await _alertService.ShowAsync("Erreur", "Veuillez vous assurez que les champs ne sont pas vides.", "Fermer");
                return;
            }

            try
            {
                await Auth.CreateUser(Email, Password);
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception e)
            {
                await _alertService.ShowAsync("Erreur", e.Message, "Fermer");
            }
        }
    }
}