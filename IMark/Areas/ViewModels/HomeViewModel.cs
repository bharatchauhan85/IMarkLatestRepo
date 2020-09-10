using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Helpers;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class HomeViewModel : BasePageViewModel
    {

        SortPageModelRequest _sortPageModel;
        private string _pageTitle;
        
        private string _bgImage; 
        public string BgImage
        {
            get { return _bgImage; }
            set { _bgImage = value; RaisePropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged(SearchText); }
        }

        private ObservableCollection<HomeModel> _carouselList;
        public ObservableCollection<HomeModel> CarouselList
        {
            get { return _carouselList; }
            set { _carouselList = value; RaisePropertyChanged(); }
        }
        
            private ObservableCollection<CollectionEdge> _collectionImagesList = new ObservableCollection<CollectionEdge>();
        public ObservableCollection<CollectionEdge> CollectionImagesList
        {
            get { return _collectionImagesList; }
            set { _collectionImagesList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ProductModel> _categoriesImagesList=new ObservableCollection<ProductModel>();
        public ObservableCollection<ProductModel> CategoriesImagesList
        {
            get { return _categoriesImagesList; }
            set { _categoriesImagesList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<CollectionListEdge> _featuredProductList;
        public ObservableCollection<CollectionListEdge> FeaturedProductList
        {
            get { return _featuredProductList; }
            set { _featuredProductList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<CollectionListEdge> _newArrivalList = new ObservableCollection<CollectionListEdge>();
        public ObservableCollection<CollectionListEdge> NewArrivalList
        {
            get { return _newArrivalList; }
            set { _newArrivalList = value; RaisePropertyChanged(); }
        }
        
        private ObservableCollection<CustomerModel> _customerList;
        public ObservableCollection<CustomerModel> CustomerList
        {
            get { return _customerList; }
            set { _customerList = value; RaisePropertyChanged(); }
        }
        private IApiService _apiService;
        public HomeViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _pageTitle = "CarouselViewChallege";
            BgImage = "banner";
            CarouselList = GetCarouselList();
            NewArrivalList = new ObservableCollection<CollectionListEdge>();
            FeaturedProductList = new ObservableCollection<CollectionListEdge>();
            CategoriesImagesList = new ObservableCollection<ProductModel>();
            CollectionImagesList = new ObservableCollection<CollectionEdge>();
          //  CustomerList = GetCustmerList();
        }
        public ObservableCollection<HomeModel> GetCarouselList()
        {
            return new ObservableCollection<HomeModel>()
            {
                new HomeModel{ CarouselImages="banner2",},                
                new HomeModel{ CarouselImages="banner2",},                
                new HomeModel{ CarouselImages="banner2",},                 
                new HomeModel{ CarouselImages="banner2",},                 
            };
        }
        public async Task GetCollectionList()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                CollectionImagesList.Clear();
               // string queryid_id = "{shop{products(first: 50){edges{node{id images(first: 5){ edges {node{ id src}}} title productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}    ";
                 string queryid_id = "{shop {name collections(first: 50) {edges {node { id image {id originalSrc} title products(first: 50) {edges {node {id productType variants(first: 50){edges{node{id available price title selectedOptions{name value} image{id originalSrc}}}} title}}}}}}}}";
                var res = await _apiService.GetCollectionType(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res != null)
                {
                    CollectionImagesList = new ObservableCollection<CollectionEdge>(res.data.shop.collections.edges);
                    await GetArrivalData("New");
                    await GetFeatureData("Feat");
                }
                else
                {
                    CategoriesImagesList.Clear();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
            UserDialogs.Instance.HideLoading();
        }

        private async Task GetArrivalData(string CatagoriesData)
        {
            try
            {
                char quote = '"';
                string modifiedCollectionName = quote + CatagoriesData + quote;
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:50 ) {edges {node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    NewArrivalList = new ObservableCollection<CollectionListEdge>(res.data.shop.collectionByHandle.products.edges.Take(3));
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
        }
        private async Task GetFeatureData(string CatagoriesData)
        {
            try
            {
                char quote = '"';
                string modifiedCollectionName = quote + CatagoriesData + quote;
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:50 ) {edges {node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    FeaturedProductList = new ObservableCollection<CollectionListEdge>(res.data.shop.collectionByHandle.products.edges.Take(3));
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
        }

        private List<ProductsEdge> _listOfCategoriesData;

        public async Task GetCategoriesImagesList()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                //string queryid_id = "{shop{products(first: 50){edges{node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}    ";
                string queryid_id = "{shop{products(first: 50){edges{node{id images(first: 1){ edges {node{ id src}}} tags title productType }}}}}  ";
                var res = await _apiService.GetProductType(queryid_id);
                if (res != null)
                {
                    _listOfCategoriesData = res.Data.Shop.Products.Edges;
                    CategoriesImagesList = new ObservableCollection<ProductModel>();
                    foreach (var data in res.Data.Shop.Products.Edges)
                    {
                        foreach(var item in data.Node.tags)
                        {
                           if(!CategoriesImagesList.Any(s=>s.Tag == item))
                           {
                                CategoriesImagesList.Add(new ProductModel
                                {
                                    Image = data?.Node?.Images?.Edges[0]?.Node?.Src,
                                    Tag = item
                                });
                           }
                        }
                    }
                    ProductsEdge products = new ProductsEdge();
                }
                else
                {
                   // getCategory.Clear();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
            UserDialogs.Instance.HideLoading();
        }
        private string _addProducts;
        public string AddProducts
        {
            get { return _addProducts; }
            set { _addProducts = value; RaisePropertyChanged(); }
        }
        public ICommand BellNotificationCommand => new Command(async(obj) =>
          {
              if (NavigationHelper.CheckType(typeof(NotificationPage)))
              {
                  await App.Current.MainPage.Navigation.PushModalAsync(new NotificationPage());
              }
          });
        public ICommand SearchCommand => new Command(async(obj) =>
          {
              UserDialogs.Instance.ShowLoading();
              try
              {
                  char t = '"';
                  var type = t + SearchText + t;
                  string queryid_id = "{ shop{ products(first: 50, query:" + type + "){edges{node{id images(first: 5){ edges {node{ id src}}} title productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}";
                  var res = await _apiService.SortListOfProduct(queryid_id);
                  if (res.Data.Shop.Products.Edges.Count > 0)
                  {
                      App.Locator.SearchPage.InitializeSortData(res.Data.Shop.Products.Edges);
                      await App.Current.MainPage.Navigation.PushModalAsync(new SeachPage());
                  }
                  else
                  {
                      // getCategory.Clear();
                      ShowAlert("This product is not available","Ok");
                  }
              }
              catch (Exception ex)
              {
                  UserDialogs.Instance.HideLoading();
                  UserDialogs.Instance.Alert(ex.Message.ToString());
              }
              UserDialogs.Instance.HideLoading();
              
          });
          public ICommand CartCommand => new Command(async(obj) =>
          {
              //await App.Current.MainPage.Navigation.PushModalAsync(new AddPhoto());
          });
        public ICommand FeaturedProductCommand => new Command(async (obj) =>
        {
            var NewArrivalDetailsData = obj as ProductsEdge;
            await App.Locator.ProductDetailsPage.InitializeData(NewArrivalDetailsData);
            //var NewArrivalDetailsData = obj as NewArrivalModel;
           await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage(NewArrivalDetailsData));

        });
        public Command SeeAllProduct
        {
            get
            {
                return new Command(async (obj) =>
                {
                    try
                    {
                        await App.Locator.FeaturesProducts.SeeAllProduct("Feat");
                        await App.Current.MainPage.Navigation.PushModalAsync(new FeaturesProducts());
                    }
                    catch
                    {

                    }
                    
                });
            }
        }
        public Command SeeAllNewArrival
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Locator.ProductViewPage.SeeAllProduct("New");
                    await App.Current.MainPage.Navigation.PushModalAsync(new ProductViewPage());
                });
            }
        }
        public Command ProductCommand
        {
            get
            {
                return new Command(async () =>
                { 
                    await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
                });
            }
        }

        
        public ICommand CollectionCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as CollectionEdge;
            var filtersData = CollectionImagesList.Where(s => s.node.title == CatagoriesByListData.node.title).ToList();
            await App.Locator.CollectionByList.Init(filtersData);
            await App.Current.MainPage.Navigation.PushModalAsync(new CollectionByList());
        });
        public ICommand CategoriesCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as ProductModel;
            //List<ProductsEdge> products = new List<ProductsEdge>();
            //foreach(var cat in CatagoriesByListData.Node.tags)
            //{
            //    var filterData = _listOfCategoriesData.Where(s => s.Node.tags.Any(q => q == cat)).ToList();
            //    if (filterData != null)
            //        products.AddRange(filterData);
            //}
            await App.Locator.CatagoriesByList.Init(CatagoriesByListData);
          await  App.Current.MainPage.Navigation.PushModalAsync(new CatagoriesByList());
        });
      
        public ICommand NewArrivalDetailsCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as CollectionListEdge;
            await App.Locator.ProductDetailsPage.InitializeCollection(CatagoriesByListData);
            await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());

        });
    }
    public class ProductModel
    {
        public string Tag { get; set; }
        public Uri Image { get; set; }
    }
}