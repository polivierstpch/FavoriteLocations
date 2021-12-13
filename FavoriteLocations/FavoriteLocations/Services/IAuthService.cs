using System.Threading.Tasks;

namespace FavoriteLocations.Services
{
    public interface IAuthService
    {
        bool IsAuthentified { get; }
        string UserIdentifier { get; }
        Task<bool> CreateUser(string email, string password);
        Task<bool> LoginUser(string email, string password);
    }
}