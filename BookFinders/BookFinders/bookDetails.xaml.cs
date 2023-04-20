using BookFinders.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;


namespace BookFinders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookDetails : INotifyPropertyChanged
    {
        private ObservableCollection<Comment> commentsList;
        private HttpClient client;
        private book bookObject;
        private User userObj;
        private bool isLiked = false;
        
        public bookDetails(book bookObj, User currentUser)
        {
            InitializeComponent();

            bookName.Text = "Title:"+bookObj.Name;
            bookAuthor.Text = "Author:" + bookObj.Author;
            bookDesc.Text =  bookObj.Description;
            bookImage.Source = "bookImage.jpg";

            bookObject = bookObj;
            userObj = currentUser;
            var handler = new HttpClientHandler();

            // Set the ServerCertificateCustomValidationCallback to a delegate that accepts any certificate
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            commentsList = new ObservableCollection<Comment>();
            client = new HttpClient(handler);

            LoadComments(bookObj.Id);
               

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            

            if (userObj.Authorization != Role.Student)
            {
                foreach (var cell in commentListView.TemplatedItems)
                {
                    var deleteBtn = cell.FindByName<ImageButton>("deleteBtn");
                    if (deleteBtn != null)
                    {
                        deleteBtn.IsVisible = true;
                    }
                    else {
                        Debug.WriteLine("you are student!");
                    }
                }
            }
            foreach (var cell in commentListView.TemplatedItems)
            {
                var editBtn = cell.FindByName<ImageButton>("editBtn");
                if (editBtn != null)
                {
                    if (userObj.Id == editBtn.CommandParameter.ToString()) { 
                        editBtn.IsVisible = true;
                    }
                }
                else
                {
                    Debug.WriteLine("button not found");
                }
            }
        }
        public async void LoadComments(string bookId)
        {
            var response = await client.GetAsync("https://10.0.2.2:7042/api/Comment/getcomments/"+ bookId);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<CommentResponse>(content);
                commentsList = comments.data;
                commentListView.ItemsSource = commentsList;
                OnAppearing();
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
        public async Task<bool> addThumpsUp(string commentId)
        {
            var response = await client.GetAsync("https://10.0.2.2:7042/api/Comment/addthumbsUp/"+ commentId);
            if (response.IsSuccessStatusCode)
            {
                LoadComments(bookObject.Id);
                return true;
            }
            else
            {
                Debug.WriteLine("failed to fetch the comment");
                return false;
            }
        }
        public async Task<bool> subThumpsUp(string commentId)
        {
            var response = await client.GetAsync("https://10.0.2.2:7042/api/Comment/subthumbsUp/" + commentId);
            if (response.IsSuccessStatusCode)
            {
                LoadComments(bookObject.Id);
                return true;
            }
            else
            {
                Debug.WriteLine("failed to fetch the comment");
                return false;
            }
        }
        public async Task<bool> deleteComment(string commentId)
        {
            var response = await client.DeleteAsync("https://10.0.2.2:7042/api/Comment/removeComment/" + commentId);
            if (response.IsSuccessStatusCode)
            {
                LoadComments(bookObject.Id);
                return true;
            }
            else
            {
                Debug.WriteLine("failed to remove the comment");
                return false;
            }
        }
        private async void PostComment(object sender, System.EventArgs e)
        {
            
            if (commentEditor.Text != "")
            {
                var commentObj = new Comment()
                {
                    UserId = userObj.Id,
                    BookId = bookObject.Id,
                    UserName = userObj.Name,
                    Description = commentEditor.Text

                };
                await PostComment("https://10.0.2.2:7042/api/Comment/postcomment", commentObj);
                LoadComments(bookObject.Id);

                
            }
            else
            {
                Debug.WriteLine("comment post failed");
            }
        }

        private async void likeBtnClicked(object sender, EventArgs e)
        {
            var ListItem = sender as ImageButton;
            var currentId = ListItem.CommandParameter.ToString();
            if (isLiked == false)
            {
                await addThumpsUp(currentId);
                isLiked = true;
            }
            else
            {
                
                await subThumpsUp(currentId);
                isLiked = false;
            }
        }

        private async void deleteBtnClicked(object sender, EventArgs e)
        {
            var ListItem = sender as ImageButton;
            var currentId = ListItem.CommandParameter.ToString();
            bool result = await DisplayAlert("Confirm Delete", "Are you sure you want to remove this comment?", "Yes", "No");

            if (result == true)
            {
                await deleteComment(currentId);
                
            }
            
        }

        private void editComment(object sender, EventArgs e)
        {

            ImageButton button = (ImageButton)sender;
           
            StackLayout layout = (StackLayout)button.FindByName("commentArea");

            layout.IsVisible = true;
        }

        private async void submitEditComment(object sender, EventArgs e)
        {
            var ListItem = sender as Button;
            var currentId = ListItem.CommandParameter.ToString();
            foreach (var cell in commentListView.TemplatedItems)
            {
                var textToEdit = cell.FindByName<Editor>("editcommentEditor");
                if (textToEdit != null)
                {
                   if(textToEdit.Text != null)
                    {


                        await editCommentAsync($"https://10.0.2.2:7042/api/Comment/editComment/{currentId}/{textToEdit.Text}");
                        //Debug.WriteLine($"https://10.0.2.2:7042/api/Comment/editComment/{currentId}/{textToEdit.Text}");
                    }

                }
                else
                {
                    Debug.WriteLine("textToEdit not found");
                }
            }
        }
        public async Task editCommentAsync(string url)
        {
            
            var content = new StringContent("updateComment", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url,content);
            if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    LoadComments(bookObject.Id);

                }
                else
                {
                    // Handle error response
                    Debug.WriteLine("failed to edit the comment");
                }
            
        }
        private void closeBtnClicked(object sender, EventArgs e)
        {

            ImageButton button = (ImageButton)sender;

            StackLayout layout = (StackLayout)button.FindByName("commentArea");

            layout.IsVisible = false;
        }
    }
}