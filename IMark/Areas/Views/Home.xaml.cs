using IMark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMark.Areas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage, IMainView, IRootView
    {
        public Home()
        {
            InitializeComponent();
            BindingContext = App.Locator.Home;
            //  App.Locator.Home.InitAsyn();
            //App.Locator.Home.InitilizeData();
            App.Locator.Home.GetCollectionList();
            App.Locator.Home.GetCategoriesImagesList();
        }

        //protected override void OnAppearing()
        //{
        //    App.Locator.Home.LoadDeshboardList();
        //    base.OnAppearing();
        //}
    }
}