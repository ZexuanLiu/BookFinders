using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookFinders
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            // Use the dependency service to get a platform-specific implementation and initialize it.
            DependencyService.Get<INotificationManager>().Initialize();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new Login());

            //https://github.com/jfversluis/XFFCMPushNotificationsSample
            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

