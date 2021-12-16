using System;
using System.IO;
using FavoriteLocations.iOS.Services;
using FavoriteLocations.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace FavoriteLocations.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            DependencyService.Register<IAuthService, FirebaseAuthService>();
            
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            Firebase.Core.App.Configure();
            
            const string dbFileName = "favorite_locations.db";
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..",
                "Library");
            var fullPath = Path.Combine(directory, dbFileName);
            
            LoadApplication(new App(fullPath));

            return base.FinishedLaunching(app, options);
        }
    }
}