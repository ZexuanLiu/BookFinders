using BookFinders.Model;
using BookFindersLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace BookFinders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookDetails : ContentPage
    {
        private List<Comment> commentsList;
        private HttpClient client;
        public bookDetails(book bookObj)
        {
            InitializeComponent();

            bookName.Text = "Title:"+bookObj.Name;
            bookAuthor.Text = "Author:" + bookObj.Author;
            bookDesc.Text =  bookObj.Description;
            bookImage.Source = "bookImage.jpg";

            var handler = new HttpClientHandler();

            // Set the ServerCertificateCustomValidationCallback to a delegate that accepts any certificate
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            commentsList = new List<Comment>();
            client = new HttpClient(handler);
            LoadComments();

        }

        async void LoadComments()
        {
            var response = await client.GetAsync("https://10.0.2.2:7042/api/Comment/getcomments");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<CommentResponse>(content);
                commentsList = comments.data;
                commentListView.ItemsSource = commentsList;
                Debug.WriteLine("da"+comments);
            }
            else
            {
                Debug.WriteLine("it failed");
            }
        }



    }
}