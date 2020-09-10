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
   public class CatagoriesByListViewModel : BasePageViewModel
    {
        IApiService _apiService;
        List<ProductsEdge> _productsEdge;
        
        private string _catagoriesData;
        public string CatagoriesData
        {
            get { return _catagoriesData; }
            set { _catagoriesData = value; RaisePropertyChanged(); }
        }
        private string _images;
        public string Images
        {
            get { return _images; }
            set { _images = value; RaisePropertyChanged(); }
        }

       

        private List<Products> _catagoriesList;
        public List<Products> CatagoriesList
        {
            get { return _catagoriesList; }
            set { _catagoriesList = value; RaisePropertyChanged(); }
        }

        public async Task Init(ProductModel catagoriesByListData)
        {
            //_productsEdge = catagoriesByListData;
            //CatagoriesData = catagoriesByListData[0].Node.ProductType;
            //CatagoriesList = new List<ProductsEdge>();
            //try
            //{
            //    CatagoriesList = new List<ProductsEdge>(catagoriesByListData);
            //}
            //catch (Exception ex)
            //{
            //    UserDialogs.Instance.HideLoading();
            //    UserDialogs.Instance.Alert(ex.Message.ToString());
            //}

            UserDialogs.Instance.ShowLoading();
            try
            {
                char quote = '"';
                string modifiedCollectionName = quote +"tag:"+ catagoriesByListData.Tag + quote;
                //string queryid_id = "{shop{products(first: 50){edges{node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}    ";
                string queryid_id = "{shop{products(first: 20, query:"+ modifiedCollectionName + "){pageInfo { hasNextPage hasPreviousPage }edges{cursor node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}} ";
                var res = await _apiService.GetProductType(queryid_id);
                if (res != null)
                {
                    CatagoriesList = new List<Products>();
                    CatagoriesList.Add(res.Data.Shop.Products);
                    CatagoriesData = catagoriesByListData.Tag;
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
        public CatagoriesByListViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }
        public ICommand ThresoldCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            var getlastElement = CatagoriesList.LastOrDefault();
            if (getlastElement.pageInfo.hasNextPage)
                GetCollection(getlastElement.Edges.LastOrDefault().cursor);
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
                var res = await _apiService.GetProductType(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.Data.Shop.Products != null)
                {
                    var result = CatagoriesList.FirstOrDefault();
                    result.Edges.AddRange(res.Data.Shop.Products.Edges);
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
        public ICommand SortCommand => new Command(async (obj) =>
        {
            
            await App.Locator.SortPage.InitializeData(_productsEdge);
            await App.Current.MainPage.Navigation.PushModalAsync(new SortPage());
        });

        public ICommand CatagoriesByListCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as ProductsEdge;
            await App.Locator.ProductDetailsPage.InitializeData(CatagoriesByListData);
            await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
        });
        public ICommand CartCommand => new Command(async (obj) =>
        {
           await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
        });

        public void InitializeData(ShopProducts products)
        {
            CatagoriesList.Clear();
            CatagoriesList.Add(ConvertShopProductToProductEdge(products));
            RaisePropertyChanged(nameof(CatagoriesList));
        }

        public void InitializeSortData(Products edges)
        {
            CatagoriesData = edges.Edges[0].Node.ProductType;
            CatagoriesList = new List<Products>();
            try
            {
                CatagoriesList = new List<Products>();
                CatagoriesList.Add(edges);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        }

        private Products ConvertShopProductToProductEdge(ShopProducts products)
        {
            List<ProductsEdge> response = new List<ProductsEdge>();
            Products pro = new Products();
            foreach (var item in products.edges)
            {
                Variants variants = new Variants();
                variants.Edges = new List<VariantsEdge>();
                foreach (var variant in item.node.variants.edges)
                {
                    TentacledNode tentacled1 = new TentacledNode();
                    tentacled1.SelectedOptions = new List<SelectedOption>();
                    foreach (var tentacleData in variant.node.SelectedOptions)
                    {
                        SelectedOption option = new SelectedOption
                        {
                            Name = tentacleData.Name,
                            Value = tentacleData.Value
                        };
                        tentacled1.SelectedOptions.Add(option);
                    }
                    
                    TentacledNode tentacled = new TentacledNode
                    {
                         Id=variant.node.id,
                        Title = variant.node.Title,
                        price = variant.node.price,
                        SelectedOptions = tentacled1.SelectedOptions,
                       
                    };
                    variants.Edges.Add(new VariantsEdge
                    {
                        Node = tentacled
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
                        Variants = variants,
                        Description = item.node.description
                    }
                };
                response.Add(edge);
            }
            pro.Edges = response;
            return pro;
        }

    }
} 