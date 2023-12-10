using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
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
        public async void LoadComments(string bookId)
        {
            var response = await client.GetAsync("http://localhost:5156/api/Comment/getcomments/" + bookId);
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
    }
}

