using BookFinders.Model;
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
        List<User> userList;
        public Login()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };
            userList = new List<User>
            {
                new User(){Id = "ABC001", Name = "PeterLiu",Authorization=Role.Student, Password="123456"},
                new User(){Id = "ABC002", Name = "Roman",Authorization=Role.Librarian, Password="654321"},
            };

        }
        private void login(object sender, EventArgs e)
        {

            foreach(User user in userList)
            {
                if (user.Name == UserEmail.Text && user.Password == Password.Text)
                {

                    Navigation.PushAsync(new bookList(user));
                    return;
                }
               
            }
            DisplayAlert("Login Failed", "Please check you user name and password.", "OK");

        }

        private void Title_Clicked(object sender, EventArgs e)
        {
            string title = $"Snow day tomorrow!";
            string message = $"There is likely going to be a snowday tomorrow, stay safe!";
            notificationManager.SendNotification(title, message);
        }

        private void UserEmailComplete(object sender, EventArgs e)
        {
            Password.IsVisible = true;
            UserEmailLabel.Text = UserEmail.Text;
            UserEmailLabel.IsVisible = true;
            HintLabel.Text = "Enter password";
            UserEmail.IsVisible = false;
            SignIn.IsVisible = true;
            AnotherAccountLabel.IsVisible = false;
            CloseIcon.IsVisible = true;
            AddIcon.IsVisible = false;
            ForgotPassword.IsVisible = true;
        }

        private void CloseIcon_Click(object sender, EventArgs e)
        {
            Password.IsVisible = false;
            UserEmailLabel.Text = "";
            UserEmailLabel.IsVisible = false;
            HintLabel.Text = "Pick an account";
            UserEmail.IsVisible = true;
            SignIn.IsVisible = false;
            AnotherAccountLabel.IsVisible = true;
            CloseIcon.IsVisible = false;
            AddIcon.IsVisible = true;
            ForgotPassword.IsVisible = false;

        }
    }
}