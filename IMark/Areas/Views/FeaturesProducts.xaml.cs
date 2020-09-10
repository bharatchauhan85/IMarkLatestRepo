﻿using IMark.Data.Models.Response;
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
    public partial class FeaturesProducts : ContentPage
    {
        public FeaturesProducts()
        {
            InitializeComponent();
            BindingContext = App.Locator.FeaturesProducts;
        }
    }
}