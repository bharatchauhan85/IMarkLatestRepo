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
    public partial class CollectionByList : ContentPage
    {
        private List<CollectionEdge> _catagoriesByListData;
        public CollectionByList()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.CollectionByList;
        }

        public CollectionByList(List<CollectionEdge> catagoriesByListData)
        {
            InitializeComponent();
            BindingContext = App.Locator.CatagoriesByList;
            this._catagoriesByListData = catagoriesByListData;
            App.Locator.CollectionByList.Init(_catagoriesByListData);
        }
    }
}