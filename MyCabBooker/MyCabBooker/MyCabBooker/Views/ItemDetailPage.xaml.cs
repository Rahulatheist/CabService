using MyCabBooker.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyCabBooker.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}