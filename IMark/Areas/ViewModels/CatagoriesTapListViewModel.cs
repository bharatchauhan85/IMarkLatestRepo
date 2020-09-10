using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Data.Models.Response;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class CatagoriesTapListViewModel : BasePageViewModel
    {
        private string _catagoriesData;
        public string CatagoriesData
        {
            get { return _catagoriesData; }
            set { _catagoriesData = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ProductsEdge> _catagoriesList;
        public ObservableCollection<ProductsEdge> CatagoriesList
        {
            get { return _catagoriesList; }
            set { _catagoriesList = value; RaisePropertyChanged(); }
        }

        internal void Init(List<ProductsEdge> categorymdl)
        {
            CatagoriesData = categorymdl[0].Node.ProductType;
            CatagoriesList = new ObservableCollection<ProductsEdge>();
            try
            {
                CatagoriesList = new ObservableCollection<ProductsEdge>(categorymdl);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        }
        public CatagoriesTapListViewModel()
        {
            CatagoriesList = new ObservableCollection<ProductsEdge>();
        }
        //public ObservableCollection<ProductViewModel> GetCatagoriesList()
        //{
        //    return new ObservableCollection<ProductViewModel>()
        //    {
        //        new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="21.98" },
        //        new ProductViewModel{Image= "img1", Favorite = "wishlist", Title ="Automotive Parts & Accessories",Rating="5",Price="$21.98" },
        //        new ProductViewModel{Image= "img2",Favorite= "wishlistgrey", Title="Clothing, Shoes & Jewelry",Rating="4",Price="$21.98" },
        //        new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Automotive Parts & Accessories",Rating="4",Price="$21.98" },
        //        new ProductViewModel{Image= "img2", Favorite = "wishlistgrey", Title ="Electronics parts & Accessories",Rating="5",Price="$21.98" },
        //        new ProductViewModel{Image= "img1",Favorite= "wishlistgrey", Title="Clothing, Shoes & Jewelry",Rating="4",Price="$29.95" },
        //        new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Clothing, Shoes & Jewelry",Rating="4",Price="$21.98" },
        //        new ProductViewModel{Image= "img2", Favorite = "wishlistgrey", Title ="Automotive Parts & Accessories",Rating="5",Price="$21.98" },
        //};
        //}
        public ICommand CartCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
        });
      public ICommand CatagoriesTapListCommand => new Command(async (obj) =>
      {
          var CatagoriesByListData = obj as ProductsEdge;
          await App.Locator.ProductDetailsPage.InitializeData(CatagoriesByListData);
          await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
      });
    }
}