using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Database;
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
    public class CartPageViewModel : BasePageViewModel
    {
        IApiService _apiService;
        private ObservableCollection<Node> _cartList;
        public ObservableCollection<Node> CartList
        {
            get { return _cartList; }
            set { _cartList = value; RaisePropertyChanged(); }
        }
        private string _couponCode;
        public string CouponCode
        {
            get { return _couponCode; }
            set { _couponCode = value; RaisePropertyChanged(); }
        }

        private string _pONumber;
        private double _costSummary;
        private string _totalPrice;
        private double _shippingPrice;
        private bool _isPaymentEnabled;

        public bool IsPaymentEnabled
        {
            get { return _isPaymentEnabled; }
            set { _isPaymentEnabled = value;RaisePropertyChanged(nameof(IsPaymentEnabled)); }
        }

        public double ShippingPrice
        {
            get { return _shippingPrice; }
            set { _shippingPrice = value;RaisePropertyChanged(nameof(ShippingPrice)); }
        }

        public string TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value;RaisePropertyChanged(nameof(TotalPrice)); }
        }

        public double CostSummary
        {
            get { return _costSummary; }
            set { _costSummary = value;RaisePropertyChanged(nameof(CostSummary)); }
        }
        private int _totalQuantity;

        public int TotalQuantity
        {
            get { return _totalQuantity; }
            set { _totalQuantity = value; RaisePropertyChanged(nameof(TotalQuantity)); }
        }

        public ICommand GetBackCommand => new Command(async () =>
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        });
        public string PONumber
        {
            get { return _pONumber; }
            set { _pONumber = value; RaisePropertyChanged(); }
        }
        
        public CartPageViewModel(IApiService apiService)
        {
            _apiService = apiService;
            //CartList = GetCartList();
        }

        //public ObservableCollection<CartModel> GetCartList()
        //{


        //	//return new ObservableCollection<CartModel>()
        //	//{
        //	//	new CartModel {Image="thumbnail_foam", Title="Port Authority®...", ProductColor="Color: NAVY", Qty="Size: Medium",Price="$21.58",  MinusImage ="Minus",PlusImage ="Plus",Counter=1, StackLayoutIsVisible1 =true,StackLayoutIsVisible =false, PrintName = "Custom Embroidery Or Print: John Smith" },
        //	//	new CartModel {Image="thumbnail_foam", Title="Port Authority®...", ProductColor="Color: NAVY",Qty="Size: Medium", Price="$21.58",  MinusImage ="Minus",PlusImage ="Plus",Counter=1, StackLayoutIsVisible1 =true,StackLayoutIsVisible =false,  PrintName = "Custom Embroidery Or Print: John Smith" },
        //	//	new CartModel {Image="thumbnail_foam", Title="Port Authority®...", ProductColor="Color: NAVY", Qty="Size: Medium",Price="$21.58",  MinusImage ="Minus",PlusImage ="Plus",Counter=1, StackLayoutIsVisible1 =true,StackLayoutIsVisible =false,  PrintName = "Custom Embroidery Or Print: John Smith" },
        // //     };
        //}
        public ICommand MinusCommand => new Command<Node>(async (obj) =>
        {
            if(obj.quantity > 1)
            {
                obj.quantity--;
                await UpdateCart();
            }
        });
        public ICommand PlusCommand => new Command<Node>(async (obj) =>
        {
            obj.quantity++;
            await UpdateCart();
        });
        public ICommand DeleteCommand => new Command<Node>(async (obj) =>
        {
            await DeleteCartItem(obj);
        });

        private async Task DeleteCartItem(Node obj)
        {
          await  ShowLoading();
            try
            {

                CartDeleteRequestModel request = new CartDeleteRequestModel
                {
                    checkoutId = SettingExtension.CheckoutId
                };
                request.lineItemIds = new List<string>();
                request.lineItemIds.Add(obj.id);

                string query = @"mutation checkoutLineItemsRemove($checkoutId: ID!, $lineItemIds: [ID!]!) {
                  checkoutLineItemsRemove(checkoutId: $checkoutId, lineItemIds: $lineItemIds) {
                    checkout {
                      id
                    }
                    checkoutUserErrors {
                      code
                      field
                      message
                    }
                  }
                }";
                var result = await _apiService.DeleteCart(query, request);
                if (result != null)
                {
                    await InitilizeData();
                }
                else
                {
                    await HideLoading();
                }
            }
            catch (Exception ex)
            {
                await HideLoading();
            }
            await HideLoading();
        }

        private async Task UpdateCart()
        {
            await ShowLoading();
            try
            {

                CartUpdateRequestModel request = new CartUpdateRequestModel
                {
                    checkoutId = SettingExtension.CheckoutId
                };
                request.lineItems = new List<UpdateLineItem>();
                foreach(var item in CartList)
                {
                    request.lineItems.Add(new UpdateLineItem
                    {
                        id = item.id,
                        quantity = item.quantity,
                        variantId = item.variant.id
                    });
                }

                string query = @"mutation checkoutLineItemsUpdate($checkoutId: ID!, $lineItems: [CheckoutLineItemUpdateInput!]!) {
                  checkoutLineItemsUpdate(checkoutId: $checkoutId, lineItems: $lineItems) {
                    checkout {
                      id
                    }
                    checkoutUserErrors {
                      code
                      field
                      message
                    }
                  }
                }";
                var result = await _apiService.UpdateCart(query, request);
                if (result != null)
                {
                    await InitilizeData();
                }
                else
                {
                    await HideLoading();
                }
            }
            catch (Exception ex)
            {
                await HideLoading();
            }
            await HideLoading();
        }

        
        public ICommand AddCartCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            await Task.Delay(500);
            //((MainMenu.Current.Detail as NavigationPage).CurrentPage as TabbedPage).CurrentPage =
            //((MainMenu.Current.Detail as NavigationPage).CurrentPage as TabbedPage).Children[2];
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.Alert("Item added successfully");
        });
        public ICommand EditCommand => new Command(async (obj) =>
        {
            //var MyOrderMdl = obj as CartModel;
            //App.Current.MainPage.Navigation.PushModalAsync(new EditCartPage());
        });
        //public ICommand CartListCommand => new Command(async (obj) =>
        //{
        //	var MyOrderMdl = obj as MyOrderModel;
        //	App.Current.MainPage.Navigation.PushModalAsync(new EditCartPage(MyOrderMdl));
        //});
        public ICommand PaymentCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new ShippingConfirmation());
        });

        //    public async Task InitializeData(ProductsEdge productsEdge)
        //    {

        //        try
        //        {

        //        }catch(Exception ex)
        //        {
        //ex.Message.ToString();
        //        }


        // }

        public async Task<IList<Edges>> GetAddressInfor()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                string accestoken = Convert.ToString(SettingExtension.AccessToken);
                //string addressId = SettingExtension.AddressId;
                char quote = '"';
                string modifiedString = quote + accestoken + quote;
                string query = "{ customer(customerAccessToken: " + modifiedString + "){id firstName lastName email createdAt lastIncompleteCheckout{id createdAt webUrl lineItems(first: 5){ edges{ node{quantity id title variant{ id image{ id originalSrc}}}}}}addresses(first: 3){edges{ node{ id firstName lastName address1 address2 city company country zip province}}}orders(first: 3){edges{node{id orderNumber name email lineItems(first: 3){ edges{ node{quantity variant{ id image{ originalSrc}}}}}}}}}}";
                //string qury = "{ customer(customerAccessToken:" + modifiedString + "){ id email firstName lastName createdAt  addresses(first: 9){ edges{ node{ firstName lastName address1 address2 city company country zip } } } } }";
                var res1 = await _apiService.GetAddress(query);
                if (res1.data.customer.addresses.edges.Count > 0)
                {
                    Customer_Address customer_Address = new Customer_Address();
                    SettingExtension.UserEmail = res1.data.customer.email;
                    customer_Address.firstName = res1.data.customer.addresses.edges[0].node.firstName;
                    customer_Address.lastName = res1.data.customer.addresses.edges[0].node.lastName;
                    customer_Address.address1 = res1.data.customer.addresses.edges[0].node.address1;
                    customer_Address.address2 = res1.data.customer.addresses.edges[0].node.address2;
                    customer_Address.company = res1.data.customer.addresses.edges[0].node.company;
                    customer_Address.country = res1.data.customer.addresses.edges[0].node.country;
                    customer_Address.zip = res1.data.customer.addresses.edges[0].node.zip;
                    customer_Address.city = res1.data.customer.addresses.edges[0].node.city;
                    customer_Address.province = res1.data.customer.addresses.edges[0].node.province;
                    //App.Locator.ShippingConfirmation.InitializeData(customer_Address);
                    return res1.data.customer.addresses.edges;
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.Navigation.PushModalAsync(new AddNewAddress());
                    return null;

                }
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await ShowAlert(ex.Message);
                return null;
            }
            UserDialogs.Instance.HideLoading();
        }

        public async Task InitilizeData()
        {
           
            try
            {
                string accestoken = Convert.ToString(SettingExtension.AccessToken);
                char quote = '"';
                string modifiedString = quote + accestoken + quote;
                string query = @"{customer(customerAccessToken: "+ modifiedString + "){ id firstName lastName email createdAt lastIncompleteCheckout{id createdAt totalPrice   currencyCode webUrl lineItems(first: 5){ edges{ node{quantity id title variant{ id price selectedOptions{name value}  image{ id originalSrc}}}}}}addresses(first: 3){edges{ node{ id firstName lastName address1 address2 city company country	}}}orders(first: 3){edges{node{id orderNumber name email lineItems(first: 3){ edges{ node{quantity variant{ id image{ originalSrc}}}}}}}}}}";
                var result = await _apiService.GetCustomer(query);
                if (result != null)
                {
                    CartList = new ObservableCollection<Node>();
                    foreach(var item in result.data.customer.lastIncompleteCheckout.lineItems.edges)
                    {
                        CartList.Add(item.node);
                    } 
                    
                }
                else
                {
                    CartList = new ObservableCollection<Node>();
                }
                CostSummary = CartList.Sum(s => Convert.ToDouble(s.variant.price)*s.quantity);
                ShippingPrice = 0;
                TotalQuantity = CartList.Count();
                TotalPrice = result.data.customer.lastIncompleteCheckout.totalPrice;
                if(CartList.Count > 0)
                {
                    IsPaymentEnabled = true;
                }
                else
                {
                    IsPaymentEnabled = false;
                }
            }
            catch(Exception ex)
            {
                CostSummary = 0;
                ShippingPrice = 0;
                TotalPrice = "0";
               
            }
           ;
        }
    }
}
