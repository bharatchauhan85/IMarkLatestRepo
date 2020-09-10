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
   public class FeaturesProductsViewModel :  BasePageViewModel
    {
        IApiService _apiService;
        private ObservableCollection<CollectionListProducts> _featuresProducts;
        public ObservableCollection<CollectionListProducts> FeaturesProducts
        {
            get { return _featuresProducts; }
            set { _featuresProducts = value; RaisePropertyChanged(nameof(FeaturesProducts)); }
        }
        //public ICommand NewArrivalsCommand => new Command(async (obj) =>
        //{
        //    ImageNewArrivals = "CheckGreen";
        //    ImageFeatured = "Uncheck";
        //    ImageBestsellers = "Uncheck";
        //});
        //public ICommand FeaturedCommand => new Command(async (obj) =>
        //{
        //    ImageNewArrivals = "Uncheck";
        //    ImageFeatured = "CheckGreen";
        //    ImageBestsellers = "Uncheck";
        //});
        //public ICommand BestsellersCommand => new Command(async (obj) =>
        //{
        //    ImageNewArrivals = "Uncheck";
        //    ImageFeatured = "Uncheck";
        //    ImageBestsellers = "CheckGreen";
        //});

        public ICommand SortCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new SortPage());
        });

        private string _catagoriesData;

        public string CatagoriesDataTitle
        {
            get { return _catagoriesData; }
            set { _catagoriesData = value;RaisePropertyChanged(nameof(CatagoriesDataTitle)); }
        }


        public FeaturesProductsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            //foreach (var item in FeaturesProducts)
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

        public ICommand ThresoldCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            var getlastElement = FeaturesProducts.LastOrDefault();
            if (getlastElement.pageInfo.hasNextPage)
                GetCollection(getlastElement.edges.LastOrDefault().cursor);
            else
                UserDialogs.Instance.Toast("No More Data Available");
            UserDialogs.Instance.HideLoading();
        });

        private async void GetCollection(string afterData)
        {
            string type = CatagoriesDataTitle;
            if (CatagoriesDataTitle == "New Arrivals")
            {
                type = CatagoriesDataTitle.Split(' ')[0];
            }
            if(CatagoriesDataTitle == "Featured Product")
            {
                type = "Feat";
            }
            char quote = '"';
            string modifiedCollectionName = quote + type + quote;
            string modifiedAfterCursor = quote + afterData + quote;
            try
            {
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5 after:" + modifiedAfterCursor + " )  { pageInfo { hasNextPage hasPreviousPage } edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    var result = FeaturesProducts.FirstOrDefault();
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

        public void InitializeData(ShopProducts products)
        {
            FeaturesProducts.Clear();
           //FeaturesProducts = ConvertShopProductToProductEdge(products);
            RaisePropertyChanged(nameof(FeaturesProducts));
        }

        private List<ProductsEdge> ConvertShopProductToProductEdge(ShopProducts products)
        {
            List<ProductsEdge> response = new List<ProductsEdge>();
            foreach(var item in products.edges)
            {
                Variants variants = new Variants();
                variants.Edges = new List<VariantsEdge>();
                foreach(var variant in item.node.variants.edges)
                {
                    TentacledNode tentacled1 = new TentacledNode();
                    tentacled1.SelectedOptions = new List<SelectedOption>();
                    foreach(var tentacleData in item.node.variants.edges[0].node.SelectedOptions)
                    {
                        SelectedOption option = new SelectedOption
                        {
                             Name=tentacleData.Name,
                             Value=tentacleData.Value
                        };
                        tentacled1.SelectedOptions.Add(option);
                    }
                    TentacledNode tentacled = new TentacledNode
                    {
                        Title = variant.node.Title,
                        price = variant.node.price,
                       
                    };
                    variants.Edges.Add(new VariantsEdge
                    {
                        Node=tentacled
                    });
                }
                Images images = new Images();
                images.Edges = new List<ImagesEdge>();
                foreach (var value in item.node.images.edges)
                {
                    FluffyNode fluffy = new FluffyNode
                    {
                        Id = value.node.id,
                        Src = new Uri(value.node.src)
                    };
                    images.Edges.Add(new ImagesEdge
                    {
                        Node = fluffy
                    });
                }
                ProductsEdge edge = new ProductsEdge
                {
                    Node = new PurpleNode
                    {
                        Id = item.node.id,
                        Title = item.node.title,
                        Images = images,
                        Variants=variants,
                        Description=item.node.description
                    }
                };
                response.Add(edge);
            }
            return response;
        }

        public async Task SeeAllProduct(string CatagoriesData)
        {

            UserDialogs.Instance.ShowLoading();
            try
            {
                char quote = '"';
                string modifiedCollectionName = quote + CatagoriesData + quote;
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5 )  { pageInfo { hasNextPage hasPreviousPage } edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    FeaturesProducts = new ObservableCollection<CollectionListProducts>();
                    FeaturesProducts.Add(res.data.shop.collectionByHandle.products);
                    CatagoriesData = res.data.shop.collectionByHandle.title;
                    CatagoriesDataTitle = res.data.shop.collectionByHandle.title;
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

        public async void Init(List<ProductsEdge> catagoriesByListData)
        {
            FeaturesProducts = new ObservableCollection<CollectionListProducts>();
            ProductsEdge products = new ProductsEdge();
            //var filterData = FeaturesProducts.Where(s => s.node.ProductType == "T-Shirts").ToList();

            //FeaturesProducts = new List<ProductsEdge>(filterData);
            
        }
        public ICommand ProductCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as CollectionListEdge;
            //this._catagoriesByListData = catagoriesByListData;
            //App.Locator.CatagoriesByList.Init(_catagoriesByListData);

            await App.Locator.ProductDetailsPage.InitializeCollection(CatagoriesByListData);
            await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
        });

        
        //public ObservableCollection<ProductViewModel> GetFeaturesProducts()
        //{

        //}
        public ICommand CartCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
        });
    }
}
