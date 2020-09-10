using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Data.Models.Response;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class SearchPageViewModel : BasePageViewModel
    {
        
            private List<ProductsEdge> _searchList;
        public List<ProductsEdge> SearchList
        {
            get { return _searchList; }
            set { _searchList = value; RaisePropertyChanged(); }
        }
        public void InitializeSortData(List<ProductsEdge> edges)
        {
           
            SearchList = new List<ProductsEdge>();
            try
            {
                SearchList = new List<ProductsEdge>(edges);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        }
        public ICommand SearchListCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as ProductsEdge;
            await App.Locator.ProductDetailsPage.InitializeData(CatagoriesByListData);
            await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
        });
    }
}
