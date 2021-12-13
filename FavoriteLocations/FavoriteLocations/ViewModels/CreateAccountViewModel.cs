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
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        public ICommand CreateAccountCommand { get; }
        public ICommand GoBackToLoginCommand { get; }

        public CreateAccountViewModel()
        {
            _alertService = DependencyService.Resolve<IAlertService>();

            CreateAccountCommand = new Command(TryCreateAccount);
            GoBackToLoginCommand = new Command(ConfirmLeaveCreateAccount);
        }

        private bool ValidateUserInfo(out string message)
        {
            if (string.IsNullOrEmpty(Email))
            {
                message = "Veuillez entrer une adresse email valide.";
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                message = "Veuillez entrer un mot de passe avec au moins 1 caractère";
                return false;
            }

            message = string.Empty;
            return true;
        }


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
        
        private async void TryCreateAccount()
        {
            if (!ValidateUserInfo(out var message))
            {
                await _alertService.ShowAsync("Erreur", message, "Fermer");
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