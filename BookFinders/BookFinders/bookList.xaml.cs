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
                new book(){Id = "0", Name = "My book Cover",Author="Peter", Description="This is a very good book", ImageLink="bookImage.jpg" },
                new book(){Id = "1", Name = "Intro to Java",Author="Roman", Description="This is a good Java book", ImageLink="bookImage.jpg" }
            };
            bookLists.ItemsSource = booklist;
        }

        private void bookLists_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var bookObj = bookLists.SelectedItem as book;
          //  var bookDetailsPage = new bookDetails();
          //  bookDetailsPage.BindingContext = bookObj;
            Navigation.PushModalAsync(new bookDetails(bookObj));
        }
    }
}