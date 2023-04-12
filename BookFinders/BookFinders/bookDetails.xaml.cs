using BookFinders.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace BookFinders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookDetails : ContentPage
    {
        private List<Comment> commentsList;
        private HttpClient client;
        private book bookObject;
        public bookDetails(book bookObj)
        {
            InitializeComponent();

            bookName.Text = "Title:"+bookObj.Name;
            bookAuthor.Text = "Author:" + bookObj.Author;
            bookDesc.Text =  bookObj.Description;
            bookImage.Source = "bookImage.jpg";
            bookObject = bookObj;
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
            }
            else
            {
                Debug.WriteLine("failed to fetch the comment");
            }
        }

        public async Task<string> PostComment(string url, Comment comment)
        {
            var json = JsonConvert.SerializeObject(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("success");
                return responseContent;
            }

            return null;
        }
        private void PostComment(object sender, System.EventArgs e)
        {
            if (commentEditor.Text != "")
            {
                var commentObj = new Comment()
                {
                    UserId = "001",
                    BookId = bookObject.Id,
                    UserName = "Roman",
                    Description = commentEditor.Text

                };
                PostComment("https://10.0.2.2:7042/api/Comment/postcomment", commentObj);
                RefreshPage(bookObject);
            }
            else
            {
                Debug.WriteLine("a");
            }
        }
        public async Task RefreshPage(book bookobj)
        {
            await Navigation.PopAsync();
            await Navigation.PushAsync(new bookDetails(bookobj));
        }
    }
}