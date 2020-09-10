using Acr.UserDialogs;
using GalaSoft.MvvmLight.Views;
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
	public class CategoryPageViewModel : BasePageViewModel
	{
		IApiService _apiService;
		private ObservableCollection<ProductsEdge> _categoryList;
		public ObservableCollection<ProductsEdge> CategoryList
		{
			get { return _categoryList; }
			set { _categoryList = value; RaisePropertyChanged(); }
		}
		public CategoryPageViewModel(IApiService apiService)
		{
			_apiService = apiService;
			CategoryList = new ObservableCollection<ProductsEdge>();
		}
		public async Task  GetCategoryList()
		{

			UserDialogs.Instance.ShowLoading();
			try
			{
				string queryid_id = "{shop{products(first: 250){edges{node{id images(first: 5){ edges {node{ id src}}} title productType description variants(first: 3){ edges{ node{ id price title selectedOptions{name value} image{ id originalSrc} } } }}}}}}    ";
				var res = await _apiService.GetProductType(queryid_id);
				if (res != null)
				{
					//var list=res.Data.Shop.Products.Edges.FirstOrDefault(s=>s.Node.ProductType).Distin
					CategoryList = new ObservableCollection<ProductsEdge>(res.Data.Shop.Products.Edges);
					ProductsEdge products = new ProductsEdge();
					var filterData = CategoryList.Where(s => s.Node.ProductType == "T-Shirts").ToList();
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
	

		public ICommand CategoryCommand => new Command(async (obj) =>
		{
			var CatagoriesByListData = obj as ProductsEdge;
			var filterData = CategoryList.Where(s => s.Node.ProductType == CatagoriesByListData.Node.ProductType).ToList();
			App.Locator.CatagoriesTapList.Init(filterData);
			
			await App.Current.MainPage.Navigation.PushModalAsync(new CatagoriesTapList());
		});
	}
}