using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookFinders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        //public ICommand TestPushNotificationBind => new Command(Title_Clicked);

        INotificationManager notificationManager;

        public Login()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };
            
        }
        private void login(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new bookList());
        }

        private void Title_Clicked(object sender, EventArgs e)
        {
            string title = $"Snow day tomorrow!";
            string message = $"There is likely going to be a snowday tomorrow, stay safe!";
            notificationManager.SendNotification(title, message);
        }
    }
}