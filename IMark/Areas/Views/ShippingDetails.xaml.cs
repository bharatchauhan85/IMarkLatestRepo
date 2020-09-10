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
    public partial class ShippingDetails : ContentPage
    {
        public ShippingDetails()
        {
            InitializeComponent();
            BindingContext = App.Locator.ShippingDetails;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Countrypicker.Focus();
        }
                
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Statepicker.Focus();
        }
    }
}