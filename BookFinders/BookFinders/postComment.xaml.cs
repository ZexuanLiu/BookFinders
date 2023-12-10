using System;
using System.Collections.Generic;
using BookFinders.Model;
using Xamarin.Forms;

namespace BookFinders
{	
	public partial class postComment : ContentPage
	{
        private Book bookObject;
        private User userObj;
        public postComment (Book bookObj, User currentUser)
		{
			InitializeComponent ();
            bookObject = bookObj;
            userObj = currentUser;
            commentEditor.Text = "";
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

        void PostButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (commentEditor.Text == "")
            {
                DisplayAlert("Post Comment Failed", "Please enter your comment.", "OK");
            }
            else
            {

            }
        }
    }
}

