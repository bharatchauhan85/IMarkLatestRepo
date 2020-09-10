using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace IMark.Areas.ViewModels
{
   public class PhotoGalleryViewModel : BasePageViewModel
    {
        IApiService _apiService;
        private ObservableCollection<ImagesEdge> _photoList;

        public ObservableCollection<ImagesEdge> PhotoList
        {
            get { return _photoList; }
            set { _photoList = value; RaisePropertyChanged(); }
        }
        private string _photoGallery;

        public string PhotoGallery
        {
            get { return _photoGallery; }
            set { _photoGallery = value; RaisePropertyChanged(PhotoGallery); }
        }
      
        public PhotoGalleryViewModel(IApiService apiService)
        {
            _apiService = apiService;
            PhotoList = new ObservableCollection<ImagesEdge>();
        }
     
        //Api For Gallery from Produts
        public async void Init()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                char t = '"';
                var type = t + "shopgallery" + t;
                string queryid_id = "{ shop{ products(first: 50, query:" + type + "){edges{node{id images(first: 15){ edges {node{ id src}}} title productType }}}}}";
                var res = await _apiService.SortListOfProduct(queryid_id);
                if (res.Data.Shop.Products.Edges.Count > 0)

                       PhotoList = new ObservableCollection<ImagesEdge>();
                        foreach (var item in res.Data.Shop.Products.Edges)
                        {
                            foreach (var images in item.Node.Images.Edges)
                                PhotoList.Add(images);
                            // PhotoGallery = images.Node.Src.AbsoluteUri;
                        }
            }
            catch(Exception ex)
            {
               await ShowAlert(ex.Message);
              UserDialogs.Instance.HideLoading();
            }
            UserDialogs.Instance.HideLoading();
        }
    }
}
