using System.Threading.Tasks;

namespace FavoriteLocations.Services
{
    public interface IAlertService
    {
        Task ShowAsync(string title, string message, string closeLabel);
    }
}