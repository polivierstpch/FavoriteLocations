using System.Threading.Tasks;
using Xamarin.Forms;

namespace FavoriteLocations.Services
{
    public static class Auth
    {
        private static readonly IAuthService AuthService = DependencyService.Resolve<IAuthService>();
        
        public static bool IsAuthentified => AuthService.IsAuthentified;
        public static string UserIdentifier => AuthService.UserIdentifier;
        
        public static async Task<bool> CreateUser(string email, string password) 
            => await AuthService.CreateUser(email, password);
     
        public static async Task<bool> LoginUser(string email, string password) 
            => await AuthService.LoginUser(email, password);
    }
}