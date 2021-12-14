using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FavoriteLocations.Android.Services;
using FavoriteLocations.Services;
using Xamarin.Forms;
using Environment = System.Environment;

namespace FavoriteLocations.Android
{
    [Activity(Label = "FavoriteLocations", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            DependencyService.Register<IAuthService, FirebaseAuthService>();
            
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            var dbFileName = "favorite_locations.db";
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fullPath = Path.Combine(directory, dbFileName);
            
            LoadApplication(new App(fullPath));
        }
    }
}