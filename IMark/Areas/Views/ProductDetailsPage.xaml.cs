using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IMark.Areas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailsPage : ContentPage
    {
        private HomeModel _featuredProductData;
        private ProductsEdge _newArrivalDetailsData;
        private ProductViewModel _productData;

        public ProductDetailsPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.ProductDetailsPage;
        }

        public ProductDetailsPage(HomeModel featuredProductData)
        {
            InitializeComponent();
            BindingContext = App.Locator.ProductDetailsPage;
            _featuredProductData = featuredProductData;
            //App.Locator.ProductDetailsPage.Init(_featuredProductData);
        }

        public ProductDetailsPage(ProductsEdge newArrivalDetailsData)
        {
            InitializeComponent();
            BindingContext = App.Locator.ProductDetailsPage;
            _newArrivalDetailsData = newArrivalDetailsData; 
           // App.Locator.ProductDetailsPage.Init(_newArrivalDetailsData);
        }

        //public ProductDetailsPage(ProductViewModel productData)
        //{
        //    InitializeComponent();
        //    BindingContext = App.Locator.ProductDetailsPage;
        //    this._productData = productData;
        //  //  App.Locator.ProductDetailsPage.Init(_productData);
        //}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var cl=((sender as Frame).Parent as CollectionView).ItemsSource as IList<Data.Models.Common.Data>;
                foreach (var it in cl)
                {
                    it.BorderCol = "White";
                    it.IsSelected = false;
                }
                var item = (sender as Frame).BindingContext as Data.Models.Common.Data;
                item.BorderCol = "Black";
                item.IsSelected = true;
                App.Locator.ProductDetailsPage.CheckAvailibilityCmd.Execute(item);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}