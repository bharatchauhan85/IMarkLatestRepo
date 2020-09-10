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
    public partial class AddPhoto2 : ContentPage
    {
        public AddPhoto2()
        {
            InitializeComponent();
            BindingContext = App.Locator.AddPhoto2;
        }
    }
}