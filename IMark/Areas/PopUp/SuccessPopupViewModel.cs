using Acr.UserDialogs;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.PopUp
{
    public class SuccessPopupViewModel : BasePageViewModel
    {
        public ICommand PaymentCommand => new Command(async (obj) =>
        { 
            UserDialogs.Instance.ShowLoading();            
            await App.Current.MainPage.Navigation.PushModalAsync(new MainMenu());
            await NavigationService.ClosePopupsAsync();
            UserDialogs.Instance.HideLoading();            
        });
    }
}