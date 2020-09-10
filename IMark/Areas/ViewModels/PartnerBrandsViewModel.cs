using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IMark.Areas.ViewModels
{
   public class PartnerBrandsViewModel : BasePageViewModel
    {
        IApiService _apiService;
        private ObservableCollection<ProductsEdge> _partnerBrandsList;
        public ObservableCollection<ProductsEdge> PartnerBrandsList
        {
            get { return _partnerBrandsList; }
            set { _partnerBrandsList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ImagesEdge> _partnerBrandsList2;
        public ObservableCollection<ImagesEdge> PartnerBrandsList2
        {
            get { return _partnerBrandsList2; }
            set { _partnerBrandsList2 = value; RaisePropertyChanged(); }
        }
        public PartnerBrandsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            PartnerBrandsList2 = new ObservableCollection<ImagesEdge>();
            PartnerBrandsList = new ObservableCollection<ProductsEdge>();
        }


        //Api For Gallery from Produts
        public async void Init()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                char t = '"';
                var type = t + "x-partners-and-brands" + t;
                string queryid_id = "{ shop{ products(first: 50, query:" + type + "){edges{node{id images(first: 15){ edges {node{ id src}}} title productType }}}}}";
                var res = await _apiService.SortListOfProduct(queryid_id);
                if (res.Data.Shop.Products.Edges.Count > 0)
                    PartnerBrandsList = new ObservableCollection<ProductsEdge>();
                    PartnerBrandsList2 = new ObservableCollection<ImagesEdge>();
                foreach (var item in res.Data.Shop.Products.Edges)
                {
                    PartnerBrandsList.Add(item);
                    foreach (var images in item.Node.Images.Edges)
                        PartnerBrandsList2.Add(images);
                    // PhotoGallery = images.Node.Src.AbsoluteUri;
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
                UserDialogs.Instance.HideLoading();
            }
            UserDialogs.Instance.HideLoading();
        }
        //public ObservableCollection<ContributionModel> GetPartnerBrandsList()
        //{
        //    return new ObservableCollection<ContributionModel>
        //    {
        //      new ContributionModel{ Image="brand1"},  
        //    };
        //}
        //public ObservableCollection<ContributionModel> GetPartnerBrandsList2()
        //{
        //    return new ObservableCollection<ContributionModel>
        //    {
        //      new ContributionModel{ Image="brand1",Image1="brand1"},
        //      new ContributionModel{ Image1="brand2"},
        //      new ContributionModel{ Image1="brand3"},
        //      new ContributionModel{ Image1="brand4"},
        //      new ContributionModel{ Image1="brand5"},
        //      new ContributionModel{ Image1="brand6"},
        //      new ContributionModel{ Image1="brand7"},
        //      new ContributionModel{ Image1="brand3"},
        //      new ContributionModel{ Image1="brand4"},
        //      new ContributionModel{ Image1="brand5"},
        //      new ContributionModel{ Image1="brand6"}
        //    };
        //}
    }
}
