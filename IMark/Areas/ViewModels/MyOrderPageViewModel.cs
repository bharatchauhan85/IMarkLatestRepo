using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Core.Helpers;
using IMark.Data.Models.Common;
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
   public class MyOrderPageViewModel :BasePageViewModel
    {
		IApiService _apiService;
		private ObservableCollection<MyOrderModel> _myOrderList;
		public ObservableCollection<MyOrderModel> MyOrderList
		{
			get { return _myOrderList; }
			set { _myOrderList = value;RaisePropertyChanged(); }
		}
		private string _status;
		public string Status
		{
			get { return _status; }
			set { _status = value; RaisePropertyChanged(Status); }
		}
		public MyOrderPageViewModel(IApiService apiService)
		{
			_apiService = apiService;
		}
		public async Task InitializeData()
		{
			UserDialogs.Instance.ShowLoading();
			try
			{
				string accestoken = Convert.ToString(SettingExtension.AccessToken);
				char quote = '"';
				string modifiedString = quote + accestoken + quote;
				string query = @"{customer(customerAccessToken: " + modifiedString + "){ id firstName lastName email createdAt lastIncompleteCheckout{id createdAt webUrl lineItems(first: 5){ edges{ node{quantity id title variant{ id price selectedOptions{name value}  image{ id originalSrc}}}}}}addresses(first: 3){edges{ node{ id firstName lastName address1 address2 city company country	}}}orders(first: 20){edges{node{id orderNumber name email successfulFulfillments(first: 3){fulfillmentLineItems(first: 3){edges{node{lineItem{title variant{id}}}}}trackingCompany trackingInfo(first: 3){number url}} lineItems(first: 3){ edges{ node{quantity title variant{ id price title selectedOptions{name value} image{ originalSrc}}}}}}}}}}";
				var result = await _apiService.GetCustomer(query);
				if (result.data.customer.orders != null)
				{
					MyOrderList = new ObservableCollection<MyOrderModel>();
					foreach(var item in result.data.customer.orders.edges)
                    {
						MyOrderModel myOrder = new MyOrderModel();
						myOrder.node = item.node;
						if (item.node.successfulFulfillments.Count == 0)
							myOrder.Status = "UnFulfilled";
						else if(item.node.successfulFulfillments.Count==item.node.lineItems.edges.Count)
							myOrder.Status = "Fulfilled";
						else
							myOrder.Status = "Partialy Paid";
						MyOrderList.Add(myOrder);
					}
				}
				else
				{
					await ShowAlert("No data Found");
					UserDialogs.Instance.HideLoading();
				}
			}
			catch (Exception ex)
			{
				await ShowAlert("Internal Server Error");
				UserDialogs.Instance.HideLoading();
			}
			UserDialogs.Instance.HideLoading();
		}
		  public ICommand MyOrderCommand => new Command(async (obj) =>
		  {
			  var MyOrderMdl = obj as IMark.Data.Models.Common.MyOrderModel;
			  App.Current.MainPage.Navigation.PushModalAsync(new MyOrderDetailsPage(MyOrderMdl));
		  });
	}
}