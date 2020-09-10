using Acr.UserDialogs;
using IMark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMark.Areas.Views.MasterDetailsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuMaster :ContentPage, IMainView, IRootView
    {
        public MainMenuMaster()
        {
            InitializeComponent();
            BindingContext = App.Locator.MainMenuMaster; 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Locator.MainMenuMaster.InitializeData();
        }
    }
}