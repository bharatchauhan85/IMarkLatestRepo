using Acr.UserDialogs;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class BecomeDistributor2ViewModel : BasePageViewModel
    {
        public ICommand ScbscribeCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            await App.Current.MainPage.Navigation.PushModalAsync(new MainMenu()); 
            UserDialogs.Instance.HideLoading();
        });
    }
}
