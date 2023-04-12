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
        private User currentUser;
        List<book> booklist;
        public bookList(User user)
        {
            InitializeComponent();
            booklist = new List<book>
            {
                new book(){Id = "001", Name = "My book Cover",Author="Peter", Description="This is a very good book", ImageLink="bookImage.jpg" },
                new book(){Id = "002", Name = "Intro to Java",Author="Roman", Description="This is a good Java book", ImageLink="bookImage.jpg" }
            };
            bookLists.ItemsSource = booklist;
            currentUser = user;
        }

        private void bookLists_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var bookObj = bookLists.SelectedItem as book;
          //  var bookDetailsPage = new bookDetails();
          //  bookDetailsPage.BindingContext = bookObj;
            Navigation.PushModalAsync(new bookDetails(bookObj,currentUser));
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue;
            List<book> filteredBooks = new List<book>();

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredBooks = booklist.Where(b => b.Name.Contains(searchText) || b.Author.Contains(searchText)).ToList();
            }
            else
            {
                filteredBooks = booklist;
            }

            bookLists.ItemsSource = filteredBooks;
        }
    }
}