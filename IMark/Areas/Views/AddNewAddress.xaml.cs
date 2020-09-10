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
    public partial class AddNewAddress : ContentPage
    {
        public AddNewAddress()
        {
            InitializeComponent();
            BindingContext = App.Locator.AddNewAddress;
            App.Locator.AddNewAddress.IntitializeErrorVisible();
        }

        //private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    Countrypicker.Focus();
        //}

        //private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        //{
        //    Statepicker.Focus();
        //}
    }
}