using System.Threading.Tasks;

namespace FavoriteLocations.Services
{
    public class AlertService : IAlertService
    {
        public async Task ShowAsync(string title, string message, string closeLabel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, closeLabel);
        }
    }
}