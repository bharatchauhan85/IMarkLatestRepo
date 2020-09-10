using IMark.Areas.Model;
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
    public partial class MyOrderDetailsPage : ContentPage
    {
        private IMark.Data.Models.Common.MyOrderModel _myOrderMdl;

        public MyOrderDetailsPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.MyOrderDetailsPage;
        }

        public MyOrderDetailsPage(IMark.Data.Models.Common.MyOrderModel myOrderMdl)
        {
            InitializeComponent();
            BindingContext = App.Locator.MyOrderDetailsPage;
            this._myOrderMdl = myOrderMdl;
            App.Locator.MyOrderDetailsPage.Init(_myOrderMdl);
        }
    }
}