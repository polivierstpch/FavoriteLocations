using System;
using System.Threading.Tasks;
using FavoriteLocations.Services;
using Firebase.Auth;

namespace FavoriteLocations.Android.Services
{
    public class FirebaseAuthService : IAuthService
    {
        private static FirebaseAuth Auth => FirebaseAuth.Instance;

        public bool IsAuthentified => Auth.CurrentUser != null;
        public string UserIdentifier => Auth.CurrentUser.Uid;
        
        public async Task<bool> CreateUser(string email, string password)
        {
            try
            {
                await Auth.CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthUserCollisionException)
            {
                throw new Exception("Un utilisateur avec cette adresse courriel existe déjà.");
            }
            catch (FirebaseAuthInvalidCredentialsException)
            {
                throw new Exception("Informations d'identification invalides.");
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
                await Auth.SignInWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthInvalidUserException)
            {
                throw new Exception("Impossible de trouver votre compte utilisateur.");
            }
            catch (FirebaseAuthInvalidCredentialsException)
            {
                throw new Exception("L'adresse courriel ou le mot de passe est invalide. Veuillez réessayer.");
            }
            catch (Exception)
            {
                throw new Exception("Une erreur est survenue.");
            }
        }
    }
}