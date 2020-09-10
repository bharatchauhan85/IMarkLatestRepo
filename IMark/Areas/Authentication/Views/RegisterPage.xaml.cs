using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMark.Areas.Authentication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.RegisterPage;
            App.Locator.RegisterPage.IntitializeErrorVisible();
        }

        
        private void IsSwitchIsToggled_Toggled(object sender, ToggledEventArgs e)
        {

        }
    }
}