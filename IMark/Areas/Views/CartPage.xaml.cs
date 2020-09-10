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
    public partial class CartPage : ContentPage
    {
        public CartPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.CartPage;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           // App.Locator.CartPage.InitAsyn();
        }
    }
}