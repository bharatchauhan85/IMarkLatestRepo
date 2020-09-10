using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Behaviors;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class BecomeDistributorViewModel : BasePageViewModel
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged(); }
        }
        public ICommand DonedisCommand => new Command(async (obj) =>
        {
             if (string.IsNullOrEmpty(Email))
                UserDialogs.Instance.Alert("Please enter the email id.", "Error", "Ok");
            else if (!Regex.IsMatch(Email, RegexBehavior.emailRegex().ToString()))
                UserDialogs.Instance.Alert("Please enter the valid email id.", "Error", "Ok");
             else
            await App.Current.MainPage.Navigation.PushModalAsync(new BecomeDistributor2());
        });
    }
}
