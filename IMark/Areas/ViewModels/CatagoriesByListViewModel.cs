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
        private ProductModel _catagoriesByListData;
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

       

        private ObservableCollection<Products> _catagoriesList;
        public ObservableCollection<Products> CatagoriesList
        {
            get { return _catagoriesList; }
            set { _catagoriesList = value; RaisePropertyChanged(nameof(CatagoriesList)); }
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
            _condition = string.Empty;
            _name = string.Empty;
            UserDialogs.Instance.ShowLoading();
            _catagoriesByListData = catagoriesByListData;
            try
            {
                char quote = '"';
                string modifiedCollectionName = quote +"tag:"+ catagoriesByListData.Tag + quote;
                //string queryid_id = "{shop{products(first: 50){edges{node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}    ";
                string queryid_id = "{shop{products(first: 20, query:"+ modifiedCollectionName + "){pageInfo { hasNextPage hasPreviousPage }edges{cursor node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}} ";
                var res = await _apiService.GetProductType(queryid_id);
                if (res != null)
                {
                    CatagoriesList = new ObservableCollection<Products>();
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
            string type = CatagoriesData;
            char quote = '"';
            string modifiedCollectionName = quote + type + quote;
            string modifiedAfterCursor = quote + afterData + quote;
            try
            {
                string queryid_id;
                if (string.IsNullOrEmpty(_condition) && string.IsNullOrEmpty(_name))
                    queryid_id = "{shop{products(first: 5 after:" + modifiedAfterCursor + "," + "query:" + modifiedCollectionName + "){pageInfo { hasNextPage hasPreviousPage }edges{cursor node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}";
                else
                    queryid_id = "{shop{products(first: 5 after:" + modifiedAfterCursor + "," + "query:" + modifiedCollectionName + "," + "sortKey:" + _name + "," + "reverse: " + _condition + "){pageInfo { hasNextPage hasPreviousPage }edges{cursor node{id images(first: 5){ edges {node{ id src}}} title tags productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}";


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
            
            await App.Locator.SortPage.InitializeData(_catagoriesByListData);
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
        private string _condition = string.Empty;
        private string _name = string.Empty;
        public void InitializeSortData(Products edges, string name, string condition)
        {
            try
            {
                _condition = condition;
                _name = name;
                CatagoriesList = new ObservableCollection<Products>();
                CatagoriesList.Add(edges);
                RaisePropertyChanged(nameof(CatagoriesList));
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