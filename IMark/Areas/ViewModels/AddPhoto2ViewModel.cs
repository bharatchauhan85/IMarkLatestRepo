using IMark.Areas.Views;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
     public  class AddPhoto2ViewModel : BasePageViewModel  
    {
        public ICommand GetBackCommand => new Command(async () =>
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        });

        public ICommand Done2Command => new Command(async (obj) =>
        {

            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
        });
    }
}
