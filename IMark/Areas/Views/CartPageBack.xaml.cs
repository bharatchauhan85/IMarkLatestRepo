using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace IMark.Areas.Views
{
    public partial class CartPageBack : ContentPage
    {
        public CartPageBack()
        {
            InitializeComponent();
            BindingContext = App.Locator.CartPage;
        }
    }
}
