using System.Threading.Tasks;
using FavoriteLocations.Models;
using FavoriteLocations.Services;
using FavoriteLocations.Views;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FavoriteLocations
{
    public partial class App : Application
    {
        public static string DbPath;
        public static Configuration DefaultConfiguration;
        
        public App()
        {
            InitializeComponent();
            
            DefaultConfiguration  = new Configuration
            {
                ShowVisitedLocations = true,
                ShowKnownLocations = true,
                ShowWishedLocations = true,
                LatitudeDegrees = 0.01,
                LongitudeDegrees = 0.01
            };
            
            DependencyService.Register<IAlertService, AlertService>();
        }

        public App(string dbPath) : this()
        {
            DbPath = dbPath;

            using (var conn = new SQLiteConnection(DbPath))
            {
                conn.CreateTable<FavoriteLocation>();
                conn.CreateTable<Configuration>();
            }
            
            MainPage = new NavigationPage(new LoginView());
        }
        
        public static async Task<PermissionStatus> ValidateAndAskForLocation()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Code iOS
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;
        }
        
        protected override async void OnStart()
        {
            await ValidateAndAskForLocation();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
      
        
    }
}