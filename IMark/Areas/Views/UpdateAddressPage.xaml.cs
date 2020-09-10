using IMark.Areas.Model;
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
    public partial class UpdateAddressPage : ContentPage
    {
        public UpdateAddressPage(EditSaveAddressModel editSave)
        {
            InitializeComponent();
            this.BindingContext = App.Locator.UpdateAddressPage;
            App.Locator.UpdateAddressPage.IntitializeErrorVisible();
            App.Locator.UpdateAddressPage.GetEditAddress(editSave);
        }
    }
}