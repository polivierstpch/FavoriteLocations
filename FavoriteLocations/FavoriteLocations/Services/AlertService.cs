using System.Threading.Tasks;

namespace FavoriteLocations.Services
{
    public class AlertService : IAlertService
    {
        public async Task ShowAsync(string title, string message, string closeLabel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, closeLabel);
        }

        public async Task<bool> ShowConfirmAsync(string title, string message, string confirmLabel, string rejectLabel)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, confirmLabel, rejectLabel);
        }
    }
}