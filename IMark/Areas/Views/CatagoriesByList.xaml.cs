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
    public partial class CatagoriesByList : ContentPage
    {
        private List<ProductsEdge> _catagoriesByListData;

        public CatagoriesByList()
        {
            InitializeComponent();
           BindingContext= App.Locator.CatagoriesByList;
        }

        public CatagoriesByList(List<ProductsEdge> catagoriesByListData)
        {
            InitializeComponent();
            BindingContext = App.Locator.CatagoriesByList;
            this._catagoriesByListData = catagoriesByListData;
            //App.Locator.CatagoriesByList.Init(_catagoriesByListData);
        }
    }
}