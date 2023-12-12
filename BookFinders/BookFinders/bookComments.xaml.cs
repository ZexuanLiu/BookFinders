using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using BookFinders.Model;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BookFinders
{	
	public partial class bookComments : ContentPage
	{
        private Book bookObject;
        private User userObj;
        private HttpClient client;
        private ObservableCollection<Comment> commentsList;

        public bookComments (Book bookObj, User currentUser)
		{
			InitializeComponent ();
            bookObject = bookObj;
            userObj = currentUser;
            var handler = new HttpClientHandler();

            // Set the ServerCertificateCustomValidationCallback to a delegate that accepts any certificate
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
            commentsList = new ObservableCollection<Comment>();
            LoadComments(bookObject.Id);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (userObj.Authorization != Role.Student)
            {
                foreach (var cell in commentListView.TemplatedItems)
                {
                    var deleteBtn = cell.FindByName<ImageButton>("DeleteCommentBtn");
                    if (deleteBtn != null)
                    {
                        
                        deleteBtn.IsVisible = true;
                       
                    }
                    else
                    {
                        Debug.WriteLine("deleteBtn not exist");
                    }
                }
            }
            else
            {
                Debug.WriteLine("you are student!");
            }

        }
        public async void LoadComments(string bookId)
        {
            var response = await client.GetAsync("http://api.krutikov.openstack.fast.sheridanc.on.ca/api/Comment/getcomments/" + bookId);
            //var response = await client.GetAsync("http://10.0.2.2:5156/api/Comment/getcomments/" + bookId);
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

        void OnCloseButtonTapped(System.Object sender, System.EventArgs e)
        {
            searchBar.IsVisible = false;
            searchBarFrame.IsVisible = false;
            closeButton.IsVisible = false;
        }

        void searchIcon_Clicked(System.Object sender, System.EventArgs e)
        {
            searchBar.IsVisible = true;
            searchBarFrame.IsVisible = true;
            closeButton.IsVisible = true;
        }

        void AddCommentsBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new postComment(bookObject, userObj));
        }

        void comments_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }

        async void DeleteCommentBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            var ListItem = sender as ImageButton;
            var currentId = ListItem.CommandParameter.ToString();
            bool result = await DisplayAlert("Confirm Delete", "Are you sure you want to remove this comment?", "Yes", "No");

            if (result == true)
            {
                await deleteComment(currentId);

            }
        }
        public async Task<bool> deleteComment(string commentId)
        {
            var response = await client.DeleteAsync("http://api.krutikov.openstack.fast.sheridanc.on.ca/api/Comment/removeComment/" + commentId);
            //var response = await client.DeleteAsync("http://10.0.2.2:5156/api/Comment/removeComment/" + commentId);
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
    }
}

