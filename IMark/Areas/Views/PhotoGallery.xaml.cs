using IMark.Data.Models.Response;
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
    public partial class PhotoGallery : ContentPage
    {
    
        public PhotoGallery()
        {
            InitializeComponent();
            BindingContext = App.Locator.PhotoGallery;
            App.Locator.PhotoGallery.Init();
        }
        
    }
}