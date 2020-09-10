using GalaSoft.MvvmLight.Views;
using IMark.ViewModels;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class CheckInternetPageViewModel : BasePageViewModel
    {
        private INavigationService _navigationService;
        public CheckInternetPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Command checkInternetCmd
        {
            get
            {
                return new Command(async () =>
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        App.Current.MainPage.Navigation.PopAsync();
                        return;
                    }
                    else
                    {
                        return;
                    }
                });
            }
        }
    }
}
