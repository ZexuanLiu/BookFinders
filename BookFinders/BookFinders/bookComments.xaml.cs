using System;
using System.Collections.Generic;
using BookFinders.Model;
using Xamarin.Forms;

namespace BookFinders
{	
	public partial class bookComments : ContentPage
	{
        private Book bookObject;
        private User userObj;
        public bookComments (Book bookObj, User currentUser)
		{
			InitializeComponent ();
            bookObject = bookObj;
            userObj = currentUser;
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
    }
}

