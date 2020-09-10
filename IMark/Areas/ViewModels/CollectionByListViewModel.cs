using Acr.UserDialogs;
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
    public class CollectionByListViewModel : BasePageViewModel
    {
        List<CollectionEdge> _collectionEdge;
        IApiService _apiService;
        public string _catagoriesData;
        public string CatagoriesData
        {
            get { return _catagoriesData; }
            set { _catagoriesData = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<CollectionListProducts> _collectionList;
        public ObservableCollection<CollectionListProducts> CollectionList
        {
            get { return _collectionList; }
            set { _collectionList = value; RaisePropertyChanged(nameof(CollectionList)); }
        }
        public CollectionByListViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        internal async Task Init(List<CollectionEdge> filterData)
        {
            _Condition = string.Empty;
            _Name = string.Empty;
            _collectionEdge = filterData;
            UserDialogs.Instance.ShowLoading();
            CatagoriesData = _collectionEdge[0].node.title;
            if (CatagoriesData == "New Arrivals")
            {
                CatagoriesData = CatagoriesData.Split(' ')[0];
            }
            CollectionList = new ObservableCollection<CollectionListProducts>();
            char quote = '"';
            string modifiedCollectionName = quote + CatagoriesData + quote;
            try
            {
                CollectionList.Clear();
                string queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5 ) {pageInfo { hasNextPage hasPreviousPage }edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    CollectionList = new ObservableCollection<CollectionListProducts>();
                    CollectionList.Add(res.data.shop.collectionByHandle.products);
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

        public ICommand SortCommand => new Command(async (obj) =>
        {
            await App.Locator.SortPage.InitializeCollection(_collectionEdge);
            await App.Current.MainPage.Navigation.PushModalAsync(new SortPage());
        });
        public ICommand ThresoldCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            var getlastElement = CollectionList.LastOrDefault();
            if (getlastElement.pageInfo.hasNextPage)
                GetCollection(getlastElement.edges.LastOrDefault().cursor);
            else
                UserDialogs.Instance.Toast("No More Data Available");
            UserDialogs.Instance.HideLoading();
        });
        private async void GetCollection(string afterData)
        {
            
            char quote = '"';
            string type = CatagoriesData;
            if (CatagoriesData == "New Arrivals")
            {
                type = quote + CatagoriesData.Split(' ')[0] + quote;
            }
            if (CatagoriesData == "Featured Product")
            {
                type = quote + "Feat" + quote;
            }
            string modifiedCollectionName = quote + type + quote;
            string modifiedAfterCursor = quote + afterData + quote;
            try
            {
                string queryid_id;
                if (string.IsNullOrEmpty(_Condition) && string.IsNullOrEmpty(_Name))
                    queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5 after:" + modifiedAfterCursor + " ) {pageInfo { hasNextPage hasPreviousPage }edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                else
                    queryid_id = "{shop {name collectionByHandle(handle:" + modifiedCollectionName + ") {title products(first:5," + "sortKey:" + _Name + "," + "reverse: " + _Condition + " ) {pageInfo { hasNextPage hasPreviousPage }edges { cursor node {id productType description variants(first: 50){edges{node{id available title selectedOptions{name value} price image{id originalSrc}}}} title}}}}}}";
                var res = await _apiService.GetCollectionList(queryid_id);
                //  UserDialogs.Instance.HideLoading();
                if (res.data.shop.collectionByHandle != null)
                {
                    var result = CollectionList.FirstOrDefault();
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
        public ICommand CatagoriesByListCommand => new Command(async (obj) =>
        {
            var CatagoriesByListData = obj as CollectionListEdge;
            await App.Locator.ProductDetailsPage.InitializeCollection(CatagoriesByListData);
            await App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailsPage());
        });
        public ICommand CartCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
        });
        private string _Condition = string.Empty;
        private string _Name = string.Empty;
        public void InitializeSortData(CollectionListProducts listEdges, string type, string name, string condition)
        {
            _Condition = condition;
            _Name = name;
            CollectionList = new ObservableCollection<CollectionListProducts>();
            CollectionList.Add(listEdges);
            RaisePropertyChanged(nameof(CollectionList));
            //_collectionEdge = edges;
            //CollectionList = new ObservableCollection<CollectionListProducts>(listEdges);
        }
    }
}
