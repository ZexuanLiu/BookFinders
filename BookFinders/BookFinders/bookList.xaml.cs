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
        List<book> booklist;
        private HttpClient client;
        private ObservableCollection<book> booksList;

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
            booksList = new ObservableCollection<book>();

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string searchText = searchBar.Text;
            LoadBooks(searchText);
        }
        public async void LoadBooks(string searchText)
        {
            var response = await client.GetAsync("https://api-ca.hosted.exlibrisgroup.com/primo/v1/search?vid=01OCLS_SHER%3ASHER&tab=Everything&scope=MyInst_and_CI&q=q%3Dany%2Ccontains%2C"+searchText+"&lang=eng&offset=0&limit=8&sort=rank&pcAvailability=true&getMore=0&conVoc=true&inst=01OCLS_SHER&skipDelivery=true&disableSplitFacets=true&apikey=l8xxbd240191e506439380215edab4ec4d85");
           
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                var bookObjLists = JsonConvert.DeserializeObject<BookResponse>(content);

                List<Doc> docs = bookObjLists.docs;
                List<book> books = new List<book>();
                foreach (var doc in docs)
                {
                    PnxSort sort = doc.pnx.sort;
                    PnxSearch search = doc.pnx.search;
                    PnxLinks links = doc.pnx.links;

                    var book = new book
                    {
                        Id = "1",
                        Name = sort.title[0],
                        Author = sort.author[0],
                        Description = search.description[0],
                        ImageLink = await CheckImageValidity(links.thumbnail[0].Replace("$$T", ""))

                    };
                    books.Add(book);
                }

                bookLists.ItemsSource = books;

            }
            else
            {
                Debug.WriteLine("failed to fetch the comment");
            }
        }
        private void bookLists_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var bookObj = bookLists.SelectedItem as book;
          //  var bookDetailsPage = new bookDetails();
          //  bookDetailsPage.BindingContext = bookObj;
            Navigation.PushAsync(new bookDetails(bookObj,currentUser));
        }

      //  private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
      //  {
      //      string searchText = e.NewTextValue;
      //      List<book> filteredBooks = new List<book>();

      //      if (!string.IsNullOrEmpty(searchText))
      //      {
      //          filteredBooks = booklist.Where(b => b.Name.Contains(searchText) || b.Author.Contains(searchText)).ToList();
      //      }
      //      else
      //      {
      //          filteredBooks = booklist;
      //      }

      //      bookLists.ItemsSource = filteredBooks;
      //  }

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
                        return "defaultBook.png";
                    }
                }
                else
                {
                    // request failed invaild url
                    return "defaultBook.png";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "defaultBook.png";
                
            }
        }
    }
}