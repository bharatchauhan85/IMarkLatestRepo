using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class SortPageViewModel:BasePageViewModel
    {
        List<ProductsEdge> _productsEdge;
        List<CollectionEdge> _collectionEdges;
        SortPageModelRequest _sortPageModel;
        IApiService _apiService;
        private bool IsCollectionWay;
        private ObservableCollection<SortPageModelRequest> _sortList;

        public ObservableCollection<SortPageModelRequest> SortList
        {
            get { return _sortList; }
            set { _sortList = value; RaisePropertyChanged(nameof(SortList)); }
        }
        public SortPageViewModel(IApiService apiService)
        {
            _apiService = apiService;
            SortList = GetRelatedItemsList();
        }


        public ObservableCollection<SortPageModelRequest> GetRelatedItemsList()
        {
            return new ObservableCollection<SortPageModelRequest>()
            {
                new SortPageModelRequest{   title="A-Z" , sortKey="TITLE" , Condition=false},
                new SortPageModelRequest{  title="Z-A" , sortKey="TITLE" , Condition=true},
                new SortPageModelRequest{  title="Low to High" , sortKey="PRICE" , Condition=false },
                new SortPageModelRequest{   title="High to Low" , sortKey="PRICE" , Condition=true},
           };
            
        }
        public ICommand SortListCommand => new Command<SortPageModelRequest>(async (obj) =>
        {
            _sortPageModel = obj;
            if(IsCollectionWay)
            {
                await SortForCollection();
            }
            else
            {
                await SortForProduct();
            }
        });


        public async Task SortForProduct()
        {
            
          
            UserDialogs.Instance.ShowLoading();
            try
            {

                var name = _sortPageModel.sortKey;
                var condition = _sortPageModel.Condition.ToString().ToLower();
                char t = '"';
                var type = t + _productsEdge[0].Node.ProductType + t;
                string queryid_id = "{ shop{ products(first: 50, query:" + type + " sortKey:" + name + ",reverse:" + condition + "){edges{node{id images(first: 5){ edges {node{ id src}}} title productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}";
                var res = await _apiService.SortListOfProduct(queryid_id);
                if (res != null)
                {
                    App.Locator.CatagoriesByList.InitializeSortData(res.Data.Shop.Products);
                    await App.Current.MainPage.Navigation.PopModalAsync();
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

        public async Task InitializeCollection(List<CollectionEdge> collectionEdge)
        {
            IsCollectionWay = true;
            _collectionEdges = collectionEdge;
        }

        public async Task SortForCollection()
        {
            SortPageModelRequest sortPageModel = new SortPageModelRequest();
            sortPageModel = _sortPageModel;
            UserDialogs.Instance.ShowLoading();
            try
            {

                var name = _sortPageModel.sortKey;
                var condition = _sortPageModel.Condition.ToString().ToLower();
                char t = '"';
                var type = t + _collectionEdges[0].node.title + t;
                if (_collectionEdges[0].node.title == "New Arrivals")
                {
                    type = t + _collectionEdges[0].node.title.Split(' ')[0] + t;
                }
                if (_collectionEdges[0].node.title == "Featured Product")
                {
                    type = t + "Feat" + t;
                }

                string queryid_id = "{shop {name collectionByHandle(handle:"+type+") {title products(first:5,"+ "sortKey:"+ name + ","+"reverse: "+ condition + " ) {pageInfo { hasNextPage hasPreviousPage }edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
               // string queryid_id = "{ shop{ products(first: 50, query:" + type + ",sortKey:" + name + ",reverse:" + condition + "){edges{node{id images(first: 5){ edges {node{ id src}}} title productType description variants(first: 50){ edges{ node{ id  available price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                if (res != null)
                {
                    App.Locator.CollectionByList.InitializeSortData(res.data.shop.collectionByHandle.products, type, name, condition);
                    // App.Locator.CollectionByList.InitializeSortData(res.data.shop.collectionByHandle.products.edges);
                    await App.Current.MainPage.Navigation.PopModalAsync();
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
        public async Task<CollectionListProducts> ConvertToCollectionListEdge(Products edges)
        {
            CollectionListProducts collectionListProducts = new CollectionListProducts();
            List<CollectionListEdge> collectionLists = new List<CollectionListEdge>();
            foreach(var item in edges.Edges)
            {
                CollectionListEdge collection = new CollectionListEdge();
                CollectionListVariants listVariants = new CollectionListVariants();
                List<CollectionListEdge2> listEdge2s = new List<CollectionListEdge2>();
                foreach(var items in item.Node.Variants.Edges)
                {
                    CollectionListImage image = new CollectionListImage
                    {
                        id = items.Node.image.id,
                        originalSrc = items.Node.image.originalSrc
                    };
                    List<CollectionSelectedOption> selectedOptions = new List<CollectionSelectedOption>();
                    foreach(var option in items.Node.SelectedOptions)
                    {
                        CollectionSelectedOption collectionSelected = new CollectionSelectedOption
                        {
                            name = option.Name,
                            value = option.Value
                        };
                        selectedOptions.Add(collectionSelected);
                    }
                    CollectionListEdge2 collectionList = new CollectionListEdge2();
                    CollectionListNode2 collectionListNode = new CollectionListNode2();
                    collectionList.node = collectionListNode;
                    collectionList.node.available = items.Node.available;
                    collectionList.node.id = items.Node.Id;
                    collectionList.node.title = items.Node.Title;
                    collectionList.node.image = image;
                    collectionList.node.price = items.Node.price;
                    collectionList.node.selectedOptions = selectedOptions;
                    listEdge2s.Add(collectionList);
                }
                collection.node = new CollectionListNode
                {
                    description = item.Node.Description,
                    id = item.Node.Id,
                    title = item.Node.Title,
                };
                CollectionListVariants variants = new CollectionListVariants();
                collection.node.variants = variants;
                collection.node.variants.edges = listEdge2s;
                collectionLists.Add(collection);
            }
            collectionListProducts.edges = collectionLists;
            collectionListProducts.pageInfo = edges.pageInfo;
            return collectionListProducts;
        }

        internal async Task InitializeData(List<ProductsEdge> productsEdge)
        {
            IsCollectionWay = false;
            _productsEdge = productsEdge;
        }
    }
}
