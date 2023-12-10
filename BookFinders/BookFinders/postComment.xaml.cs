using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookFinders.Model;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BookFinders
{	
	public partial class postComment : ContentPage
	{
        private Book bookObject;
        private User userObj;
        private HttpClient client;

        public postComment (Book bookObj, User currentUser)
		{
			InitializeComponent ();
            bookObject = bookObj;
            userObj = currentUser;
            commentEditor.Text = "";
            var handler = new HttpClientHandler();

            // Set the ServerCertificateCustomValidationCallback to a delegate that accepts any certificate
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
        }

        void OnCloseButtonTapped(System.Object sender, System.EventArgs e)
        {
        }

        void searchIcon_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        void CancelButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void PostButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (commentEditor.Text == "")
            {
                await DisplayAlert("Post Comment Failed", "Please enter your comment.", "OK");
            }
            else
            {
                var commentObj = new Comment()
                {
                    UserId = userObj.Id,
                    BookId = bookObject.Id,
                    UserName = userObj.Name,
                    Description = commentEditor.Text

                };
                var result = await PostComment("http://localhost:5156/api/Comment/postcomment", commentObj);
                if (result != null)
                {
                    await DisplayAlert("Post Comment Success", "You comment has been posted.", "OK");
                    var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as bookComments;
                    if (previousPage != null)
                    {
                       previousPage.LoadComments(bookObject.Id);
                    }
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Post Comment Failed", "Please check connection.", "OK");
                }

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


    }
}

