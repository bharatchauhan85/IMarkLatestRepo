using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Core.Helpers;
using IMark.Data.Models.Common;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.Services.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class ProductDetailsPageViewModel : BasePageViewModel
    {
        IApiService _apiService;
        INavigationService _navigationService;
        private string _pageTitle;
        private bool IsCollectionWay;
        ProductsEdge _productsEdge;
        CollectionListEdge _collectionListEdge;
        public ObservableCollection<String> _carouselList = new ObservableCollection<String>();
        public ObservableCollection<String> CarouselList
        {
            get
            {
                return _carouselList;
            }
            set
            {
                if (_carouselList != value)
                {
                    _carouselList = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<CollectionFilterModel> _listOfAttributes;

        public ObservableCollection<CollectionFilterModel> ListOfAttributes
        {
            get { return _listOfAttributes; }
            set { _listOfAttributes = value;RaisePropertyChanged(nameof(ListOfAttributes)); }
        }



        private ObservableCollection<ProductDetailModel> _customerReviewsList;
        public ObservableCollection<ProductDetailModel> CustomerReviewsList
        {
            get { return _customerReviewsList; }
            set { _customerReviewsList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ProductDetailModel> _relatedItemsList;
        public ObservableCollection<ProductDetailModel> RelatedItemsList
        {
            get { return _relatedItemsList; }
            set { _relatedItemsList = value; RaisePropertyChanged(); }
        }
       
        private int _counter = 1;
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; RaisePropertyChanged(); }
        }
        private bool _stackLayoutIsVisible;
        public bool StackLayoutIsVisible
        {
            get { return _stackLayoutIsVisible; }
            set { _stackLayoutIsVisible = value; RaisePropertyChanged(); }
        }


//Code for Collection Details information
        public async Task InitializeCollection(CollectionListEdge catagoriesByListData)
        {
            IsCollectionWay = true;
            try
            {
                CarouselList.Clear();
                if (catagoriesByListData.node.variants.edges[0].node.available)
                {
                    IsBuyAvailable = true;
                    BuyNowText = "Buy Now";
                }
                else
                {
                    IsBuyAvailable = false;
                    BuyNowText = "Sold Out";
                }
                _collectionListEdge = catagoriesByListData;
                CarouselList = new ObservableCollection<String>();
                CarouselList.Add(catagoriesByListData.node.variants.edges[0].node.image.originalSrc.ToString());
                CarouselList.Add(catagoriesByListData.node.variants.edges[0].node.image.originalSrc.ToString());
                CarouselList.Add(catagoriesByListData.node.variants.edges[0].node.image.originalSrc.ToString());
                RaisePropertyChanged(nameof(CarouselList));
                Title = catagoriesByListData.node.title;
                Price = catagoriesByListData.node.variants.edges[0].node.price;
                Description = catagoriesByListData.node.description;
                Counter = 1;
                var ColorData = catagoriesByListData.node.variants.edges.Select(s => s.node.selectedOptions.Where(t => t.name == "Color").ToList()).ToList();
                var SizeData = catagoriesByListData.node.variants.edges.Select(s => s.node.selectedOptions.Where(t => t.name == "Size").ToList()).ToList();
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
              
                ListOfAttributes = new ObservableCollection<CollectionFilterModel>();
                try
                {
                    foreach(var varient in _collectionListEdge.node.variants.edges)
                    {
                        bool i = true;
                        foreach (var option in varient.node.selectedOptions)
                        {
                            var item = new Data.Models.Common.Data();
                            if (!i)
                            {
                                item.IsSelected = true;
                                item.BorderCol = "Black";
                            }
                            else
                            {
                                item.IsSelected = false;
                                item.BorderCol = "White";
                            }
                            if (ListOfAttributes.Any(s => s.AttributeName == option.name))
                            {
                                //Available
                                var findattribute = ListOfAttributes.FirstOrDefault(s => s.AttributeName == option.name);
                                if (!findattribute.datas.Any(s => s.VarientValue == option.value))
                                {
                                    item.VarientId = varient.node.id; item.VarientName = option.name; item.VarientValue = option.value;
                                    findattribute.datas.Add(item);
                                }
                            }
                            else
                            {
                                //not available
                                CollectionFilterModel datas = new CollectionFilterModel();
                                datas.AttributeName = option.name;
                                datas.datas = new List<Data.Models.Common.Data>();
                                item.VarientId = varient.node.id; item.VarientName = option.name; item.VarientValue = option.value;
                                datas.datas.Add(item);
                                ListOfAttributes.Add(datas);
                            }
                        }                        
                    }
                    if(ListOfAttributes != null)
                    {
                        for (int i = 0; i < ListOfAttributes.Count; i++)
                        {
                            if (ListOfAttributes[i].datas != null)
                            {
                                ListOfAttributes[i].datas[0].IsSelected = true;
                                ListOfAttributes[i].datas[0].BorderCol = "Black";
                            }
                        }
                        
                    }
                }
                catch(Exception ex)
                {
                    ShowAlert(ex.Message);
                }

            }
            catch(Exception ex)
            {
                ShowAlert(ex.Message);
            }
        }
        

        //To check availabity of Products
        public ICommand CheckAvailibilityCmd => new Command(async(obj)=>
        {

            if(IsCollectionWay)
            {
                List<Data.Models.Common.Data> datas = new List<Data.Models.Common.Data>();
                foreach (var item in ListOfAttributes)
                {
                    var selectedItem = item.datas.Where(x => x.IsSelected).FirstOrDefault();
                    if (selectedItem != null)
                        datas.Add(selectedItem);
                }
                bool isFound = false;
                foreach (var edges in _collectionListEdge.node.variants.edges)
                {
                    foreach (var attNode in datas)
                    {
                        isFound = edges.node.selectedOptions.Any(s => s.name == attNode.VarientName && s.value == attNode.VarientValue);
                        if (!isFound)
                        {
                            break;
                        }
                    }
                    if (isFound)
                    {
                        if (edges.node.available)
                        {
                            IsBuyAvailable = true;
                            BuyNowText = "Buy Now";
                        }
                        else
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                        }
                        CarouselList.Clear();
                        CarouselList.Add(edges.node.image.originalSrc);
                        SelectedNode = await ConvertCollectionListEdge2ToTentacledNode(edges);
                        break;
                    }

                }
                if (!isFound)
                {
                    IsBuyAvailable = false;
                    BuyNowText = "Sold Out";
                }
            }
            else
            {
                List<Data.Models.Common.Data> datas = new List<Data.Models.Common.Data>();
                foreach (var item in ListOfAttributes)
                {
                    var selectedItem = item.datas.Where(x => x.IsSelected).FirstOrDefault();
                    if (selectedItem != null)
                        datas.Add(selectedItem);
                }
                bool isFound = false;
                foreach (var edges in _productsEdge.Node.Variants.Edges)
                {
                    foreach (var attNode in datas)
                    {
                        isFound = edges.Node.SelectedOptions.Any(s => s.Name == attNode.VarientName && s.Value == attNode.VarientValue);
                        if (!isFound)
                        {
                            break;
                        }
                    }
                    if (isFound)
                    {
                        if (edges.Node.available)
                        {
                            IsBuyAvailable = true;
                            BuyNowText = "Buy Now";
                        }
                        else
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                        }
                        CarouselList.Clear();
                        CarouselList.Add(edges.Node.image.originalSrc);
                        SelectedNode = edges.Node;
                        break;
                    }

                }
                if (!isFound)
                {
                    await ShowAlert("Combination not found");
                }
            }
        });


        private async Task<TentacledNode> ConvertCollectionListEdge2ToTentacledNode(CollectionListEdge2 edges)
        {
            List<SelectedOption> selectedOptions = new List<SelectedOption>();
            foreach (var itemss in edges?.node?.selectedOptions)
            {
                SelectedOption selected = new SelectedOption
                {
                    Name = itemss.name,
                    Value = itemss.value
                };
                selectedOptions.Add(selected);
            }

            TentacledNode node = new TentacledNode
            {
                available = edges.node.available,
                Id = edges.node.id,
                price = edges.node.price,
                SelectedOptions = selectedOptions,
                image = new Image1
                {
                    id = edges.node.image.id,
                    originalSrc = edges.node.image.originalSrc
                }
            };
            return node;
        }

       

        

        private bool _isBuyAvailable;
        public bool IsBuyAvailable
        {
            get { return _isBuyAvailable; }
            set { _isBuyAvailable = value; RaisePropertyChanged(); }
        }

        private bool _stackLayoutIsVisible1;
        public bool StackLayoutIsVisible1
        {
            get { return _stackLayoutIsVisible1; }
            set { _stackLayoutIsVisible1 = value; RaisePropertyChanged(); }
        }

        private string _buyNowText;
        public string BuyNowText
        {
            get { return _buyNowText; }
            set { _buyNowText = value; RaisePropertyChanged(nameof(BuyNowText)); }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        private string _price;
        public string Price
        {
            get { return _price; }
            set { _price = value; RaisePropertyChanged(); }
        }

        public ICommand MinusCommand => new Command(async (obj) =>
        {
            if (Counter > 1)
            {
                Counter--;
            }
        });
        public ICommand PlusCommand => new Command(async (obj) =>
        {
            Counter++;
        });
        public ProductDetailsPageViewModel(IApiService apiService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            StackLayoutIsVisible = true;
            _pageTitle = "CarouselViewChallege";
            CustomerReviewsList = GetCustomerReviewsList();
        }
        public async Task InitializeData(ProductsEdge catagoriesByListData)
        {
            IsCollectionWay = false;
            try
            {
                
                _productsEdge = catagoriesByListData;
                if (catagoriesByListData.Node.Variants.Edges[0].Node.available)
                {
                    IsBuyAvailable = true;
                    BuyNowText = "Buy Now";
                }
                else
                {
                    IsBuyAvailable = false;
                    BuyNowText = "Sold Out";
                }
                SelectedNode = null;
                CarouselList = new ObservableCollection<String>();
                CarouselList.Add(catagoriesByListData.Node.Images.Edges[0].Node.Src.ToString());
                CarouselList.Add(catagoriesByListData.Node.Images.Edges[0].Node.Src.ToString());
                CarouselList.Add(catagoriesByListData.Node.Images.Edges[0].Node.Src.ToString());
                RaisePropertyChanged(nameof(CarouselList));
                Title = catagoriesByListData.Node.Title;
                Price = catagoriesByListData.Node.Variants.Edges[0].Node.price;
                Description = catagoriesByListData.Node?.Description;
                Counter = 1;
                var ColorData = catagoriesByListData.Node.Variants.Edges.Select(s => s.Node.SelectedOptions.Where(t => t.Name == "Color").ToList()).ToList();
                var SizeData = catagoriesByListData.Node.Variants.Edges.Select(s => s.Node.SelectedOptions.Where(t => t.Name == "Size").ToList()).ToList();
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                ListOfAttributes = new ObservableCollection<CollectionFilterModel>();
                try
                {
                    foreach (var varient in _productsEdge.Node.Variants.Edges)
                    {
                        bool i = true;
                        foreach (var option in varient.Node.SelectedOptions)
                        {
                            var item = new Data.Models.Common.Data();
                            if (!i)
                            {
                                item.IsSelected = true;
                                item.BorderCol = "Black";
                            }
                            else
                            {
                                item.IsSelected = false;
                                item.BorderCol = "White";
                            }
                            if (ListOfAttributes.Any(s => s.AttributeName == option.Name))
                            {
                                //Available
                                var findattribute = ListOfAttributes.FirstOrDefault(s => s.AttributeName == option.Name);
                                if (!findattribute.datas.Any(s => s.VarientValue == option.Value))
                                {
                                    item.VarientId = varient.Node.Id; item.VarientName = option.Name; item.VarientValue = option.Value;
                                    findattribute.datas.Add(item);
                                }
                            }
                            else
                            {
                                //not available
                                CollectionFilterModel datas = new CollectionFilterModel();
                                datas.AttributeName = option.Name;
                                datas.datas = new List<Data.Models.Common.Data>();
                                item.VarientId = varient.Node.Id; item.VarientName = option.Name; item.VarientValue = option.Value;
                                datas.datas.Add(item);
                                ListOfAttributes.Add(datas);
                            }
                        }
                    }
                    if (ListOfAttributes != null)
                    {
                        for (int i = 0; i < ListOfAttributes.Count; i++)
                        {
                            if (ListOfAttributes[i].datas != null)
                            {
                                ListOfAttributes[i].datas[0].IsSelected = true;
                                ListOfAttributes[i].datas[0].BorderCol = "Black";
                            }
                        }

                    }
                }
                catch(Exception ex)
                {
                    await ShowAlert(ex.Message);
                }
            }
            catch (Exception ex)
            {
              await  ShowAlert(ex.Message);
            }


        }
        public ObservableCollection<ProductDetailModel> GetCustomerReviewsList()
        {
            return new ObservableCollection<ProductDetailModel>()
            {
                new ProductDetailModel{ ProductImages="character"},
                new ProductDetailModel{ ProductImages="character" },
                new ProductDetailModel{ ProductImages="character"},
                new ProductDetailModel{ ProductImages="character"},
                new ProductDetailModel{ ProductImages="character"},
                new ProductDetailModel{ ProductImages="character"}
           };
        }
        public ObservableCollection<ProductDetailModel> GetRelatedItemsList()
        {
            return new ObservableCollection<ProductDetailModel>()
            {
                new ProductDetailModel{ ProductImages="character", productName="Gymnastic mats",ProductPrice="$139.95", Rating="3",ProductForwardImages="BlackForward" },
                new ProductDetailModel{ ProductImages="character", productName="Gymnastic mats",ProductPrice="$495.00",Rating="4",ProductForwardImages="BlackForward" },
                new ProductDetailModel{ ProductImages="character", productName="Gymnastic mats",Rating="5",ProductPrice="$ 139.95",ProductForwardImages="BlackForward" },
                new ProductDetailModel{ ProductImages="character", productName="Gymnastic mats",ProductPrice="$139.95",Rating="3",ProductForwardImages="BlackForward" },
                new ProductDetailModel{ ProductImages="character", productName="Fall mattress",ProductPrice="$495.00",Rating="4",ProductForwardImages="BlackForward" },
                new ProductDetailModel{ ProductImages="character", productName="Gymnastic mats",ProductPrice="$ 495.00",Rating="5",ProductForwardImages="BlackForward" }
           };
        }
       

        private CollectionSelectedOption _collectionSelectedOption;
        private SelectedOption _selectedColor;
        private SizeModel _selectedSize;
        private SizeModel _selectedmaterial;
        private ObservableCollection<SizeModel> _materialList;

        public ObservableCollection<SizeModel> MaterialList
        {
            get { return _materialList; }
            set { _materialList = value; RaisePropertyChanged(nameof(MaterialList)); }
        }
        
        public ProductDetailModel PrevColor { get; set; }
        public ICommand SizeSelectCommand => new Command<SizeModel>(async (obj) =>
        {
            if(IsCollectionWay)
            {
                //For collection
                try
                {
                    try
                    {
                        _selectedSize.BorderCol = "White";
                    }
                    catch
                    {

                    }
                    _selectedSize = obj;
                    obj.BorderCol = "Black";
                    _selectedmaterial = null;

                    var edges = _collectionListEdge?.node?.variants?.edges?.Where(s => s.node.selectedOptions.Any(x => x.name == "Size" && x.value == obj.Value))?.Where(x => x.node.selectedOptions.Any(t => t.name == "Color" && t.value == _selectedColor.Value));///s.node?.selectedOptions.Where(t => t.value == obj.Value).ToList()).ToList();
                    MaterialList = new ObservableCollection<SizeModel>();
                    foreach (var edg in edges)
                    {
                        var materialKeyValue = edg.node.selectedOptions.FirstOrDefault(s => s.name == "Material");
                        if (!MaterialList.Any(s => s.Value == materialKeyValue.value))
                            MaterialList.Add(new SizeModel { BorderCol = "White", Name = materialKeyValue.name, Value = materialKeyValue.value });
                    }


                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.Alert(ex.Message);
                }
            }
            else
            {
                //For product
                try
                {
                    try
                    {
                        _selectedSize.BorderCol = "White";
                    }
                    catch
                    {

                    }
                    _selectedSize = obj;
                    obj.BorderCol = "Black";
                    _selectedmaterial = null;

                    var edges = _productsEdge?.Node?.Variants?.Edges?.Where(s => s.Node.SelectedOptions.Any(x => x.Name == "Size" && x.Value == obj.Value))?.Where(x => x.Node.SelectedOptions.Any(t => t.Name == "Color" && t.Value == _selectedColor.Value));///s.node?.selectedOptions.Where(t => t.value == obj.Value).ToList()).ToList();
                    MaterialList = new ObservableCollection<SizeModel>();
                    foreach (var edg in edges)
                    {
                        var materialKeyValue = edg.Node.SelectedOptions.FirstOrDefault(s => s.Name == "Material");
                        if (!MaterialList.Any(s => s.Value == materialKeyValue.Value))
                            MaterialList.Add(new SizeModel { BorderCol = "White", Name = materialKeyValue.Name, Value = materialKeyValue.Value });
                    }


                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.Alert(ex.Message);
                }
                
            }

            
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        });

        CollectionListNode2 selectedvarient;
        TentacledNode SelectedNode;
        public ICommand MaterialSelectCommand => new Command<SizeModel>(async (obj) =>
        {

            if(IsCollectionWay)
            {
                //For Collection
                try
                {
                    try
                    {
                        _selectedmaterial.BorderCol = "White";
                    }
                    catch
                    {

                    }
                    _selectedmaterial = obj;
                    obj.BorderCol = "Black";
                    foreach (var item in _collectionListEdge.node.variants.edges)
                    {
                        var IsAvailable = true;
                        var result = item?.node?.selectedOptions?.Where(p => p.name == "Size" && p.value == _selectedSize.Value);
                        if (result.Count() == 0)
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                            IsAvailable = false;
                        }
                        var colorresult = item?.node?.selectedOptions?.Where(s => s.name == "Color" && s.value == _selectedColor.Value);
                        if (colorresult.Count() == 0)
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                            IsAvailable = false;
                        }
                        var materialesult = item?.node?.selectedOptions?.Where(s => s.name == "Material" && s.value == _selectedmaterial.Value);
                        if (materialesult.Count() == 0)
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                            IsAvailable = false;
                        }
                        if (IsAvailable)
                        {
                            //selectedvarient = item.Node;
                            var nodes = _collectionListEdge.node.variants.edges.FirstOrDefault(s => s.node.id == item.node.id);
                            CarouselList.Clear();
                            List<SelectedOption> selectedOptions = new List<SelectedOption>();
                            foreach(var itemss in nodes?.node?.selectedOptions)
                            {
                                SelectedOption selected = new SelectedOption
                                {
                                    Name = itemss.name,
                                    Value = itemss.value
                                };
                                selectedOptions.Add(selected);
                            }

                            TentacledNode node = new TentacledNode
                            {
                                available = nodes.node.available,
                                Id = nodes.node.id,
                                price = nodes.node.price,
                                SelectedOptions = selectedOptions,
                                image = new Image1
                                {
                                    id = nodes.node.image.id,
                                    originalSrc = nodes.node.image.originalSrc
                                }
                            };
                            SelectedNode = node;
                            CarouselList.Add(nodes.node.image.originalSrc);
                            if (item.node.available)
                            {
                                IsBuyAvailable = true;
                                BuyNowText = "Buy Now";
                            }
                            else
                            {
                                IsBuyAvailable = false;
                                BuyNowText = "Sold Out";
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.Alert(ex.Message);
                }
            }
            else
            {
                //For Product
                try
                {
                    try
                    {
                        _selectedmaterial.BorderCol = "White";
                    }
                    catch
                    {

                    }
                    _selectedmaterial = obj;
                    obj.BorderCol = "Black";
                    foreach (var item in _productsEdge.Node.Variants.Edges)
                    {
                        var IsAvailable = true;
                        var result = item?.Node?.SelectedOptions?.Where(p => p.Name == "Size" && p.Value == _selectedSize.Value);
                        if (result.Count() == 0)
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                            IsAvailable = false;
                        }
                        var colorresult = item?.Node?.SelectedOptions?.Where(s => s.Name == "Color" && s.Value == _selectedColor.Value);
                        if (colorresult.Count() == 0)
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                            IsAvailable = false;
                        }
                        var materialesult = item?.Node?.SelectedOptions?.Where(s => s.Name == "Material" && s.Value == _selectedmaterial.Value);
                        if (materialesult.Count() == 0)
                        {
                            IsBuyAvailable = false;
                            BuyNowText = "Sold Out";
                            IsAvailable = false;
                        }
                        if (IsAvailable)
                        {
                            //selectedvarient = item.Node;
                            var nodes = _productsEdge.Node.Variants.Edges.FirstOrDefault(s => s.Node.Id == item.Node.Id);
                            CarouselList.Clear();
                            SelectedNode = nodes.Node;
                            CarouselList.Add(nodes.Node.image.originalSrc);
                            if (item.Node.available)
                            {
                                IsBuyAvailable = true;
                                BuyNowText = "Buy Now";
                            }
                            else
                            {
                                IsBuyAvailable = false;
                                BuyNowText = "Sold Out";
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.Alert(ex.Message);
                }
            }

            
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        });


        public ICommand CustomizeCommand => new Command(async (obj) =>
        {
            //if (_selectedColor == null || _selectedSize == null || _selectedmaterial == null)
            //{
            //    await ShowAlert("Please select color,size and material", string.Empty, "Ok");
            //    return;
            //}
            try
            {
                if (SelectedNode.available)
                    await App.Current.MainPage.Navigation.PushModalAsync(new DesignLab(SelectedNode));
                else
                    await ShowAlert("This product is not available for customize", string.Empty, "Ok");
            }
            catch
            {
                ShowAlert("Inprocess");
            }
        });
        public ICommand SatisfactionCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.Alert("Coming Soon");
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        });
        public ICommand SeeAllCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.Alert("Coming Soon");
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        });
        public ICommand CustomerReviewsCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.Alert("Coming Soon");
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        });
        private bool Check = false;
        private string _wishImage = "wishlistgrey";
        public string WishImage
        {
            get => _wishImage;
            set { _wishImage = value; RaisePropertyChanged(); }
        }
        public ICommand WishCommand => new Command(async (obj) =>
        {
            if (Check == false)
            {
                WishImage = "wishlistgrey";
                Check = true;
            }
            else
            {
                WishImage = "wishlist";
                Check = false;
            }
        });
        public ICommand RelatedItemsCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.Alert("Coming Soon");
            //await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        });
        public ICommand BuyNowCommand => new Command(async (obj) =>
        {
            if (IsCollectionWay)
            {
                await BuyNowMethod();
            }
            else
            {
                await BuyNowProductMethod();
            }
        });


        public async Task BuyNowMethod()
        {
            UserDialogs.Instance.ShowLoading();
            var address = await App.Locator.CartPage.GetAddressInfor();
            if (address != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(SettingExtension.CheckoutId))
                    {
                        //Check out Id is not found
                        CheckoutRequestModel request = new CheckoutRequestModel();
                        Inputs inputs = new Inputs();
                        request.input = inputs;
                        request.input.email = SettingExtension.UserEmail;
                        request.input.note = "hello";
                        request.input.lineItems = new List<Data.Models.Request.LineItem>();
                        request.input.lineItems.Add(new Data.Models.Request.LineItem
                        {
                            quantity = Counter,
                            variantId = SelectedNode.Id
                        });

                        Data.Models.Request.ShippingAddress shipping = new Data.Models.Request.ShippingAddress();

                        request.input.shippingAddress = shipping;
                        request.input.shippingAddress.firstName = address[0].node.firstName;
                        request.input.shippingAddress.lastName = address[0].node.lastName;
                        request.input.shippingAddress.address1 = address[0].node.address1;
                        request.input.shippingAddress.address2 = address[0].node.address2;
                        request.input.shippingAddress.city = address[0].node.city;
                        request.input.shippingAddress.country = address[0].node.country;
                        request.input.shippingAddress.zip = address[0].node.zip;
                        request.input.shippingAddress.phone = address[0].node.phone;
                        request.input.shippingAddress.province = address[0].node.province;
                        request.input.shippingAddress.company = "Arknewtech";
                        var queryId = @"mutation checkoutCreate($input: CheckoutCreateInput!) {
                          checkoutCreate(input: $input) {
                            checkout {
                              id
                            }
                            checkoutUserErrors {
                              code
                              field
                              message
                            }
                          }
                        }
                        ";
                        var res1 = await _apiService.CheckOutData(queryId, request);
                        if (res1.data.checkoutCreate.checkoutUserErrors.Count>0)
                        {

                            await ShowAlert(res1.data.checkoutCreate.checkoutUserErrors[0].message);
                        }
                        else
                        {
                            SettingExtension.CheckoutId = res1.data.checkoutCreate.checkout.id;
                            var queryCustomerAssociate = @"mutation checkoutCustomerAssociateV2($checkoutId: ID!, $customerAccessToken: String!) {
                              checkoutCustomerAssociateV2(checkoutId: $checkoutId, customerAccessToken: $customerAccessToken) {
                                checkout {
                                  id
                                }
                                checkoutUserErrors {
                                  code
                                  field
                                  message
                                }
                                customer {
                                  id
                                }
                              }
                            }";
                            CheckOutCustomerRequestModel checkOut = new CheckOutCustomerRequestModel();
                            checkOut.checkoutId = SettingExtension.CheckoutId;
                            checkOut.customerAccessToken = SettingExtension.AccessToken;
                            var checkOutAssociateResponse = await _apiService.CheckOutAssociate(queryCustomerAssociate, checkOut);
                            if (checkOutAssociateResponse.data.checkoutCustomerAssociateV2.checkout.id != null)
                            {
                                await App.Locator.CartPage.InitilizeData();
                                await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
                            }
                        }
                    }
                    else
                    {
                        //Check out Id is found
                        var queryLineId = @"mutation checkoutLineItemsAdd($lineItems: [CheckoutLineItemInput!]!, $checkoutId: ID!) {
                                      checkoutLineItemsAdd(lineItems: $lineItems, checkoutId: $checkoutId) {
                                        checkout {
                                          id
                                        }
                                        checkoutUserErrors {
                                          code
                                          field
                                          message
                                        }
                                      }
                                    }
                                    ";
                        LineItemRequest lineItem = new LineItemRequest();
                        lineItem.checkoutId = SettingExtension.CheckoutId;
                        lineItem.lineItems = new List<Data.Models.Request.LineItems>();
                        lineItem.lineItems.Add(new Data.Models.Request.LineItems
                        {
                            quantity = Counter,
                            variantId = SelectedNode.Id
                        });
                        var res = await _apiService.LineItemData(queryLineId, lineItem);
                        if (string.IsNullOrEmpty(res.data.checkoutLineItemsAdd.checkout.id))
                        {
                            //Unable to add cart
                            await ShowAlert("Product doesn't added into the cart successfully");
                        }
                        else
                        {
                            //cart added
                            await App.Locator.CartPage.InitilizeData();
                            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());

                            // await _navigationService.ShellNavigationPushAsync(new CartPage());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please provide address");
            }
            UserDialogs.Instance.HideLoading();



        }

        public async Task BuyNowProductMethod()
        {

            UserDialogs.Instance.ShowLoading();
            var address = await App.Locator.CartPage.GetAddressInfor();
            if (address != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(SettingExtension.CheckoutId))
                    {
                        //Check out Id is not found
                        CheckoutRequestModel request = new CheckoutRequestModel();
                        Inputs inputs = new Inputs();
                        request.input = inputs;
                        request.input.email = SettingExtension.UserEmail;
                        request.input.note = "hello";
                        request.input.lineItems = new List<Data.Models.Request.LineItem>();
                        request.input.lineItems.Add(new Data.Models.Request.LineItem
                        {
                            quantity = Counter,
                            variantId = SelectedNode.Id
                        });

                        Data.Models.Request.ShippingAddress shipping = new Data.Models.Request.ShippingAddress();

                        request.input.shippingAddress = shipping;
                        request.input.shippingAddress.firstName = address[0].node.firstName;
                        request.input.shippingAddress.lastName = address[0].node.lastName;
                        request.input.shippingAddress.address1 = address[0].node.address1;
                        request.input.shippingAddress.address2 = address[0].node.address2;
                        request.input.shippingAddress.city = address[0].node.city;
                        request.input.shippingAddress.country = address[0].node.country;
                        request.input.shippingAddress.zip = address[0].node.zip;
                        request.input.shippingAddress.phone = address[0].node.phone;
                        request.input.shippingAddress.province = address[0].node.province;
                        request.input.shippingAddress.company = address[0].node.company;
                        var queryId = @"mutation checkoutCreate($input: CheckoutCreateInput!) {
                          checkoutCreate(input: $input) {
                            checkout {
                              id
                            }
                            checkoutUserErrors {
                              code
                              field
                              message
                            }
                          }
                        }
                        ";
                        var res1 = await _apiService.CheckOutData(queryId, request);
                        if (res1.data.checkoutCreate.checkoutUserErrors.Count > 0)
                        {

                            await ShowAlert(res1.data.checkoutCreate.checkoutUserErrors[0].message);
                        }
                        else
                        {
                            SettingExtension.CheckoutId = res1.data.checkoutCreate.checkout.id;
                            var queryCustomerAssociate = @"mutation checkoutCustomerAssociateV2($checkoutId: ID!, $customerAccessToken: String!) {
                              checkoutCustomerAssociateV2(checkoutId: $checkoutId, customerAccessToken: $customerAccessToken) {
                                checkout {
                                  id
                                }
                                checkoutUserErrors {
                                  code
                                  field
                                  message
                                }
                                customer {
                                  id
                                }
                              }
                            }";
                            CheckOutCustomerRequestModel checkOut = new CheckOutCustomerRequestModel();
                            checkOut.checkoutId = SettingExtension.CheckoutId;
                            checkOut.customerAccessToken = SettingExtension.AccessToken;
                            var checkOutAssociateResponse = await _apiService.CheckOutAssociate(queryCustomerAssociate, checkOut);
                            if (checkOutAssociateResponse.data.checkoutCustomerAssociateV2.checkout.id != null)
                            {
                                SettingExtension.CartList = request.input.lineItems;
                                await App.Locator.CartPage.InitilizeData();
                                await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
                            }
                        }
                    }
                    else
                    {
                        //Check out Id is found
                        var queryLineId = @"mutation checkoutLineItemsAdd($lineItems: [CheckoutLineItemInput!]!, $checkoutId: ID!) {
                                      checkoutLineItemsAdd(lineItems: $lineItems, checkoutId: $checkoutId) {
                                        checkout {
                                          id
                                        }
                                        checkoutUserErrors {
                                          code
                                          field
                                          message
                                        }
                                      }
                                    }
                                    ";
                        LineItemRequest lineItem = new LineItemRequest();
                        lineItem.checkoutId = SettingExtension.CheckoutId;
                        lineItem.lineItems = new List<Data.Models.Request.LineItems>();
                        lineItem.lineItems.Add(new Data.Models.Request.LineItems
                        {
                            quantity = Counter,
                            variantId = SelectedNode.Id
                        });
                        var res = await _apiService.LineItemData(queryLineId, lineItem);
                        if (string.IsNullOrEmpty(res.data.checkoutLineItemsAdd.checkout.id))
                        {
                            //Unable to add cart
                            await ShowAlert("Product doesn't added into the cart successfully");
                        }
                        else
                        {
                            //cart added
                            //foreach(var item in lineItem.lineItems)
                            //{
                            //    SettingExtension.CartList.Add(new Data.Models.Request.LineItem
                            //    {
                            //        quantity = item.quantity,
                            //        variantId = item.variantId
                            //    });
                            //}
                            await App.Locator.CartPage.InitilizeData();
                            await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());

                            // await _navigationService.ShellNavigationPushAsync(new CartPage());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    UserDialogs.Instance.HideLoading();
                }
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Please provide address");
            }
            UserDialogs.Instance.HideLoading();
        }
        public ICommand ShareCommand => new Command(async (obj) =>
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = "I am suggesting to buy " + Title + "From IMark",
                Uri = CarouselList[0]
            });
        });
    }
}
