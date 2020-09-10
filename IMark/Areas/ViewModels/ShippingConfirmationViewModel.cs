using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Core.Helpers;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
	public class ShippingConfirmationViewModel : BasePageViewModel
	{
		EditSaveAddressModel _editAddress;
		IApiService _apiService;
		LastIncompleteCheckout _lastIncomplete;
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
		public string PONumber
		{
			get { return _pONumber; }
			set { _pONumber = value; RaisePropertyChanged(); }
		}

		private double _shippingPrice;
		public double ShippingPrice
		{
			get { return _shippingPrice; }
			set { _shippingPrice = value; RaisePropertyChanged(nameof(ShippingPrice)); }
		}

		private double _totalPrice;
		public double TotalPrice
		{
			get { return _totalPrice; }
			set { _totalPrice = value; RaisePropertyChanged(nameof(TotalPrice)); }
		}

		private double _costSummary;
		public double CostSummary
		{
			get { return _costSummary; }
			set { _costSummary = value; RaisePropertyChanged(nameof(CostSummary)); }
		}


		private int _totalQuantity;

		public int TotalQuantity
		{
			get { return _totalQuantity; }
			set { _totalQuantity = value; RaisePropertyChanged(nameof(TotalQuantity)); }
		}

      

        private bool _isPaymentEnabled;

        public bool IsPaymentEnabled
        {
            get { return _isPaymentEnabled; }
            set { _isPaymentEnabled = value;RaisePropertyChanged(nameof(IsPaymentEnabled)); }
        }
		
			private string _emailId;
		public string EmailId
		{
			get { return _emailId; }
			set { _emailId = value; RaisePropertyChanged(EmailId); }
		}
		private string _phoneNumber;
		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { _phoneNumber = value; RaisePropertyChanged(PhoneNumber); }
		}
		private string _completeAddress;
		public string CompleteAddress
		{
			get { return _completeAddress; }
			set { _completeAddress = value; RaisePropertyChanged(nameof(CompleteAddress)); }
		}
		

		public ShippingConfirmationViewModel(IApiService apiService)
		{
			_apiService = apiService;
			 InitilizeData();
			//CartList = GetCartList();
		}


		public void InitializeAddress(EditSaveAddressModel obj)
		{
			_editAddress = obj;
			CompleteAddress = _editAddress.address1 + ", " + _editAddress.address2 + ", " + _editAddress.city + ", " + _editAddress.company + ", " + _editAddress.country;
		}
		public async Task InitilizeData()
		{
			try
			{
				string accestoken = Convert.ToString(SettingExtension.AccessToken);
				char quote = '"';
				string modifiedString = quote + accestoken + quote;
				string query = @"{customer(customerAccessToken: " + modifiedString + "){ id firstName lastName email createdAt lastIncompleteCheckout{id createdAt webUrl lineItems(first: 5){ edges{ node{quantity id title variant{ id price selectedOptions{name value}  image{ id originalSrc}}}}}}addresses(first: 3){edges{ node{ id firstName lastName address1 address2 city company country	}}}orders(first: 3){edges{node{id orderNumber name email lineItems(first: 3){ edges{ node{quantity variant{ id image{ originalSrc}}}}}}}}}}";
				var result = await _apiService.GetCustomer(query);
				if (result != null)
				{ 
					_lastIncomplete = result.data.customer.lastIncompleteCheckout;
					EmailId = result.data.customer.email;
					if(result.data.customer.addresses.edges.Count > 0)
                    {
						CompleteAddress = result.data.customer.addresses.edges[0].node.address1 + ", " + result.data.customer.addresses.edges[0].node.address2 + ", " + result.data.customer.addresses.edges[0].node.city + ", " + result.data.customer.addresses.edges[0].node.company + ", " + result.data.customer.addresses.edges[0].node.country;
					}
					//foreach(var item in result.data.customer.addresses.edges)
     //               {
					//	CompleteAddress = item.node.address1 + ", " + item.node.address2 + ", " + item.node.city + ", " + item.node.company + ", " + item.node.country;
     //               }
					
					CartList = new ObservableCollection<Node>();
					foreach (var item in result.data.customer.lastIncompleteCheckout.lineItems.edges)
					{
						CartList.Add(item.node);
					}
				}
				else
				{
					CartList = new ObservableCollection<Node>();
				}
                CostSummary = CartList.Sum(s => Convert.ToDouble(s.variant.price) * s.quantity);
                ShippingPrice = 0;
                TotalQuantity = CartList.Count();
                TotalPrice = ShippingPrice + CostSummary;
                if (CartList.Count > 0)
				{
					IsPaymentEnabled = true;
				}
				else
				{
					IsPaymentEnabled = false;
				}
			}
			catch (Exception ex)
			{
                CostSummary = 0;
                ShippingPrice = 0;
                TotalPrice = 0;
            }
        }
		public ICommand ChangeCommand => new Command(async (obj) =>
		{
			//var MyOrderMdl = obj as CartModel;
			App.Locator.EditSaveAddress.Init();
			await	App.Current.MainPage.Navigation.PushModalAsync(new EditSaveAddress());
		});
		public ICommand PaymentCommand => new Command(async (obj) =>
		{
			var item = _lastIncomplete.webUrl;
			await Browser.OpenAsync(item, BrowserLaunchMode.SystemPreferred);
			SettingExtension.CheckoutId = "";
			SettingExtension.CartList = null;
			SettingExtension.CustomizeURLS = null;
			 await App.Locator.CartPage.InitilizeData();
			App.Current.MainPage = new NavigationPage(new MainMenu());
			
		});

        
    }
}