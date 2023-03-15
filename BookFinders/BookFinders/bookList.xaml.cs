using BookFinders.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookFinders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookList : ContentPage
    {
        public bookList()
        {
            InitializeComponent();
            List<book> booklist = new List<book>
            {
                new book(){Name = "My book Cover",Author="Peter", Description="This is a very good book", ImageLink="bookImage.jpg" },
                new book(){Name = "Intro to Java",Author="Roman", Description="This is a good Java book", ImageLink="bookImage.jpg" }
            };
            bookLists.ItemsSource = booklist;
        }

        private void bookLists_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var book = bookLists.SelectedItem as book;
            Navigation.PushModalAsync(new bookDetails());
        }
    }
}