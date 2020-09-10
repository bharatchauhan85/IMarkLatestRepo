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
   
    public class OurContributionViewModel:BasePageViewModel
    {
        IApiService _apiService;
        private ObservableCollection<ImagesEdge> _contributionList;

        public ObservableCollection<ImagesEdge> ContributionList
        {
            get { return _contributionList; }
            set { _contributionList = value; RaisePropertyChanged(); }
        }
        public OurContributionViewModel(IApiService apiService)
        {
            _apiService = apiService;
            ContributionList = new ObservableCollection<ImagesEdge>();
        }

        //Api For Gallery from Produts
        public async void Init()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                char t = '"';
                var type = t + "x-our-contribution" + t;
                string queryid_id = "{ shop{ products(first: 50, query:" + type + "){edges{node{id images(first: 15){ edges {node{ id src}}} title productType }}}}}";
                var res = await _apiService.SortListOfProduct(queryid_id);
                if (res.Data.Shop.Products.Edges.Count > 0)

                    ContributionList = new ObservableCollection<ImagesEdge>();
                foreach (var item in res.Data.Shop.Products.Edges)
                {
                    foreach (var images in item.Node.Images.Edges)
                        ContributionList.Add(images);
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
        //public ObservableCollection<ImagesEdge> GetContributionList()
        //{
        //    return new ObservableCollection<ContributionModel>
        //    {
        //      new ContributionModel{  Image="Rectangle13",Image1="Rectangle14"},
        //      new ContributionModel{  Image="Rectangle15",Image1="Rectangle16"},
        //      new ContributionModel{  Image="Rectangle119",Image1="Rectangle117"},
        //      new ContributionModel{  Image="Rectangle113",Image1="Rectangle115"},
        //      new ContributionModel{ Image="Rectangle113",Image1="Rectangle114"},

        //    };
        //}
    }
}
