using BookFinders.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookFinders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class bookDetails : ContentPage
    {
        public bookDetails(book bookObj)
        {
            InitializeComponent();

            bookName.Text = "Title:"+bookObj.Name;
            bookAuthor.Text = "Author:" + bookObj.Author;
            bookDesc.Text =  bookObj.Description;
            bookImage.Source = "bookImage.jpg";
        }
        private void StartAR(object sender, EventArgs e)
        {
            DependencyService.Get<IARImplmentation>().LaunchAR();
        }
    }
}