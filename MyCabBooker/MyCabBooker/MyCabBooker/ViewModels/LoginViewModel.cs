using MyCabBooker.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyCabBooker.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async()=>await OnLoginClicked());
        }

        private async Task OnLoginClicked()
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if(string.IsNullOrEmpty(UserPassword) && string.IsNullOrWhiteSpace(UserID))
                await Shell.Current.GoToAsync($"//{nameof(ItemDetailPage)}");
        }
    }
}
