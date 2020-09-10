using IMark.Interfaces;
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
    public partial class LoginPage : IMainView,IRootView
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.LoginPage;
            App.Locator.LoginPage.init();
            App.Locator.LoginPage.IntitializeErrorVisible();
        }       
    }
}