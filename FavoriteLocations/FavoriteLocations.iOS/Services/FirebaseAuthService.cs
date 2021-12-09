using System;
using System.Threading.Tasks;
using FavoriteLocations.Services;
using Foundation;
using Auth = Firebase.Auth.Auth;

namespace FavoriteLocations.iOS.Services
{
    public class FirebaseAuthService : IAuthService
    {
        private static Auth Auth => Auth.DefaultInstance;

        public bool IsAuthentified => Auth.CurrentUser != null;
        public string UserIdentifier => Auth.CurrentUser?.Uid;
        
        public async Task<bool> CreateUser(string email, string password)
        {
            try
            {
                await Auth.CreateUserAsync(email, password);
                return true;
            }
            catch (NSErrorException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception)
            {
                throw new Exception("Une erreur est survenue");
            }
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                await Auth.SignInWithPasswordAsync(email, password);
                return true;
            }
            catch (NSErrorException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception)
            {
                throw new Exception("Une erreur est survenue");
            }
        }

        
    }
}