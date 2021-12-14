using FavoriteLocations.Models;
using FavoriteLocations.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FavoriteLocations
{
    public partial class App : Application
    {

        public static string DbPath;
        
        public App()
        {
            InitializeComponent();
            
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

        protected override void OnStart()
        {
            // Handle when your app starts
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