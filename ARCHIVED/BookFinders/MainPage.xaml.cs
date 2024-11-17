using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BookFinders
{
    public partial class MainPage : ContentPage
    {
        INotificationManager notificationManager;

        public MainPage()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IARImplmentation>().LaunchAR();
        }
    }
}

