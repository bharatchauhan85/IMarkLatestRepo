using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class ProductViewPageViewModel : BasePageViewModel
    {
        IApiService _apiService;
        private bool _gridIsvisible = false;
        public bool GridIsvisible
        {
            get => _gridIsvisible;
            set { _gridIsvisible = value; RaisePropertyChanged(); }
        }

       

        private string _imageNewArrivals = "CheckGreen";
        public string ImageNewArrivals
        {
            get => _imageNewArrivals;
            set { _imageNewArrivals = value; RaisePropertyChanged(); }
        }
        private string _imageFeatured = "Uncheck";
        public string ImageFeatured
        {
            get => _imageFeatured;
            set { _imageFeatured = value; RaisePropertyChanged(); }
        }
        private string _imageBestsellers = "Uncheck";
        public string ImageBestsellers
        {
            get => _imageBestsellers;
            set { _imageBestsellers = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<CollectionListProducts> _productList;
        public ObservableCollection<CollectionListProducts> ProductList
        {
            get { return _productList; }
            set { _productList = value; RaisePropertyChanged(); }
        }
        public ICommand NewArrivalsCommand => new Command(async (obj) =>
         {
             ImageNewArrivals = "CheckGreen";
             ImageFeatured = "Uncheck";
             ImageBestsellers = "Uncheck";
         }); 
        public ICommand FeaturedCommand => new Command(async (obj) =>
         {
             ImageNewArrivals = "Uncheck";
             ImageFeatured = "CheckGreen";
             ImageBestsellers = "Uncheck";
         }); 
        public ICommand BestsellersCommand => new Command(async (obj) =>
         {
             ImageNewArrivals = "Uncheck";
             ImageFeatured = "Uncheck";
             ImageBestsellers = "CheckGreen";
         }); 
        public ICommand SelectCategoryCommand => new Command(async (obj) =>
         {
             if (GridIsvisible == false)
             {
                 GridIsvisible = true;
             }
             else
             {
                 GridIsvisible = false;
             }
         }); 
        public ICommand SortCommand => new Command(async (obj) =>
         {
             GridIsvisible = false;
         }); 
        public ICommand FilterCommand => new Command(async (obj) =>
         {
             GridIsvisible = false;
             //await App.Current.MainPage.Navigation.PushModalAsync(new FilterPage());
         });
        public Command ProductCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    var CatagoriesByListData = obj as CollectionListEdge;
                    //this._catagoriesByListData = catagoriesByListData;
                    //App.Locator.CatagoriesByList.Init(_catagoriesByListData);

                    await App.Locator.ProductDetailsPage.InitializeCollection(CatagoriesByListData);
                    await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
                });
            }
        }
        private string _favoriteImage= "wishlistgrey";
        public string FavoriteImage
        {
            get { return _favoriteImage; }
            set { _favoriteImage = value; RaisePropertyChanged(); }
        }
        private string _catagoriesData;

        public string CatagoriesData
        {
            get { return _catagoriesData; }
            set { _catagoriesData = value;RaisePropertyChanged(nameof(CatagoriesData)); }
        }

        public ProductViewPageViewModel(IApiService apiService)

        {
            _apiService = apiService;
            //ProductList = GetProductList();
            //foreach (var item in ProductList)
            //{
            //    if (int.Parse(item.Rating) == 3)
            //    {
            //        item.star1 = "star";
            //        item.star2 = "star";
            //        item.star3 = "star";
            //        item.star4 = "unfilledStar";
            //        item.star5 = "unfilledStar";
            //    }
            //    else if (int.Parse(item.Rating) == 4)
            //    {
            //        item.star1 = "star";
            //        item.star2 = "star";
            //        item.star3 = "star";
            //        item.star4 = "star";
            //        item.star5 = "unfilledStar";
            //    }
            //    else if (int.Parse(item.Rating) == 5)
            //    {
            //        item.star1 = "star";
            //        item.star2 = "star";
            //        item.star3 = "star";
            //        item.star4 = "star";
            //        item.star5 = "star";
            //    }
            //}            
        }

        public async Task SeeAllProduct(string title)
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                char quote = '"';
                string modifiedCollectionName = quote + title + quote;
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5 ) {pageInfo { hasNextPage hasPreviousPage }edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    ProductList = new ObservableCollection<CollectionListProducts>();
                    ProductList.Add(res.data.shop.collectionByHandle.products);
                    CatagoriesData = res.data.shop.collectionByHandle.title;
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    //CategoriesImagesList.Clear();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
            UserDialogs.Instance.HideLoading();
        }
        //item.Favorite = "wishlist";
        //item.Favorite = "wishlistgrey";
        public ICommand FavoriteCommand => new Command(async (obj) =>
        {            
            //ProductList = GetProductList();
            //var favoritItem = obj as ProductViewModel;  
            //if(favoritItem!=null)
            //{
            //    if (FavoriteImage == favoritItem.Favorite)
            //    {
            //        favoritItem.Favorite = "wishlist";
            //        FavoriteImage = "wishlist";
            //        //item.Favorite = false;
            //    }
            //    else
            //    {
            //        favoritItem.Favorite = "wishlistgrey";
            //        FavoriteImage = "wishlistgrey";
            //    }
            //    foreach (var item in ProductList)
            //    {
            //        item.Favorite = FavoriteImage;
            //    }
            //}         
         
        });
        public ICommand ThresoldCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            var getlastElement = ProductList.LastOrDefault();
            if (getlastElement.pageInfo.hasNextPage)
                GetCollection(getlastElement.edges.LastOrDefault().cursor);
            else
                UserDialogs.Instance.Toast("No More Data Available");
            UserDialogs.Instance.HideLoading();
        });
        private async void GetCollection(string afterData)
        {
            if (CatagoriesData == "New Arrivals")
            {
                CatagoriesData = CatagoriesData.Split(' ')[0];
            }
            char quote = '"';
            string modifiedCollectionName = quote + CatagoriesData + quote;
            string modifiedAfterCursor = quote + afterData + quote;
            try
            {
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5 after:" + modifiedAfterCursor + " ) {pageInfo { hasNextPage hasPreviousPage }edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    var result = ProductList.FirstOrDefault();
                    result.edges.AddRange(res.data.shop.collectionByHandle.products.edges);
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    //CategoriesImagesList.Clear();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        }
        public ObservableCollection<ProductViewModel> GetProductList()
        {
            return new ObservableCollection<ProductViewModel>() 
            {
                new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="21.98" },
                new ProductViewModel{Image= "img2", Favorite = "wishlist", Title ="Port Authority@ EZCotton Pique polo",Rating="5",Price="$21.98" },
                new ProductViewModel{Image= "img1",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="$21.98" },
                new ProductViewModel{Image= "thumbnail_foam",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="5",Price="$21.98" }, 
                new ProductViewModel{Image= "character", Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="$21.98" },
                new ProductViewModel{Image= "img2", Favorite = "wishlistgrey", Title ="Port Authority@ EZCotton Pique polo",Rating="5",Price="$21.98" },
                new ProductViewModel{Image= "img1",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="4",Price="$29.95" },
                new ProductViewModel{Image= "thumbnail_foam",Favorite= "wishlistgrey", Title="Port Authority@ EZCotton Pique polo",Rating="5",Price="21.98" }, 
            };
        }
        public ICommand CartCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
        });
    }
}