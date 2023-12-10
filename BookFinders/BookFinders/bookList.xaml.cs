using BookFinders.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
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
        private HttpClient client;
        List<Book> books = new List<Book>();
        ObservableCollection<Book> observableBooks = new ObservableCollection<Book>();
        private string userSearchText;
        private int offset;
        public bookList(User user)
        {
            InitializeComponent();
            //booklist = new List<book>
            // {
            //     new book(){Id = "001", Name = "My book Cover",Author="Peter", Description="This is a very good book", ImageLink="bookImage.jpg" },
            //     new book(){Id = "002", Name = "Intro to Java",Author="Roman", Description="This is a good Java book", ImageLink="bookImage.jpg" }
            // };
            // bookLists.ItemsSource = booklist;

            
            currentUser = user;
            offset = 0;
           

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
        }

        private async void searchTextComplete(object sender, EventArgs e)
        {
            string searchText = searchBar.Text;
            await LoadBooks(searchText);
            LoadMoreBtn.IsVisible = true;
        }
        public async Task LoadBooks(string searchText)
        {
            observableBooks.Clear();

            userSearchText = searchText;
            var response = await client.GetAsync("http://localhost:5156/api/BookSearch/"+searchText+"/0");
           
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                var bookObjLists = JsonConvert.DeserializeObject<List<Book>>(content);

                foreach (var doc in bookObjLists)
                {

                    var book = new Book
                    {
                        Id = "1"+(doc.Name?.Substring(0, 3) ?? "") + (doc.Author?.Substring(0, 3) ?? ""),
                        Name = doc.Name ?? "Unknown Title",
                        Author = doc.Author ?? "Unknown Author",
                        Description = doc.Description ?? "Unknown Description",
                        ImageLink = doc.ImageLink,
                        LocationCode = doc.LocationCode,
                        LibraryCode = doc.LibraryCode,
                        LocationBookShelfNum = doc.LocationBookShelfNum,
                        LocationBookShelfSide = doc.LocationBookShelfSide,
                        LocationBookShelfRow = doc.LocationBookShelfRow,
                        LocationBookShelfColumn = doc.LocationBookShelfColumn

                    };
                    
                    observableBooks.Add(book);
                }
               
                bookLists.ItemsSource = observableBooks;

            }
            else
            {
                Debug.WriteLine("failed to fetch the books");
            }
        }
        private void bookLists_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var bookObj = bookLists.SelectedItem as Book;
          //  var bookDetailsPage = new bookDetails();
          //  bookDetailsPage.BindingContext = bookObj;
            Navigation.PushAsync(new bookDetails(bookObj,currentUser));
        }
        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async Task<string> CheckImageValidity(string uri)
        {
            try
            {
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));

                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType;
                    if (contentType != null && contentType.MediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        // It is a Image
                        return uri;
                    }
                    else
                    {
                        // not a image
                        return "DefaultBook.png";
                    }
                }
                else
                {
                    // request failed invaild url
                    return "DefaultBook.png";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "DefaultBook.png";
                
            }
        }

        private void searchIcon_Clicked(object sender, EventArgs e)
        {
            searchBar.IsVisible = true;
            searchBarFrame.IsVisible = true;
            closeButton.IsVisible = true;
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            searchBar.IsVisible = false;
            searchBarFrame.IsVisible = false;
            closeButton.IsVisible = false;
        }

        private void LoadMoreButtonClicked(object sender, EventArgs e)
        {
            LoadMoreBooks();
        }
        public async void LoadMoreBooks()
        {
            offset+=10;
            var response = await client.GetAsync("http://localhost:5156/api/BookSearch/"+userSearchText+"/"+offset);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                var bookObjLists = JsonConvert.DeserializeObject<List<Book>>(content);

               
                foreach (var doc in bookObjLists)
                {
                    
                    var book = new Book
                    {
                        Id = "1" + (doc.Name?.Substring(0, 3) ?? "") + (doc.Author?.Substring(0, 3) ?? ""),
                        Name = doc.Name ?? "Unknown Title",
                        Author = doc.Author ?? "Unknown Author",
                        Description = doc.Description ?? "Unknown Description",
                        ImageLink = doc.ImageLink,
                        LocationCode = doc.LocationCode,
                        LibraryCode = doc.LibraryCode,
                        LocationBookShelfNum = doc.LocationBookShelfNum,
                        LocationBookShelfSide = doc.LocationBookShelfSide,
                        LocationBookShelfRow = doc.LocationBookShelfRow,
                        LocationBookShelfColumn = doc.LocationBookShelfColumn
                    };
                   // books.Add(book);
                   observableBooks.Add(book);
                }
                //observableBooks = new ObservableCollection<book>(books);
                bookLists.ItemsSource = observableBooks;

            }
            else
            {
                Debug.WriteLine("failed to fetch the books");
            }

        }

        
    }
}