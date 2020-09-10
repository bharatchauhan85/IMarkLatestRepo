using IMark.Areas.Views;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class CustomizationViewModel : BasePageViewModel
    {
      public ICommand UploadPictureCommand => new Command(async (obj) =>
       {
          await App.Current.MainPage.Navigation.PushModalAsync(new AddPhoto());
       });
    }
}
