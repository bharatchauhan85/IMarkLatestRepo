using Acr.UserDialogs;
using Plugin.Media;
using IMark.Areas.Model;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using IMark.Areas.Views;
using IMark.Data.Models.Response;
using System.Collections.ObjectModel;

namespace IMark.Areas.ViewModels
{
    public class MyOrderDetailsPageViewModel : BasePageViewModel
    {
       
        private ObservableCollection<IMark.Data.Models.Common.MyOrderModel> _myOrderDetailList;
        public ObservableCollection<IMark.Data.Models.Common.MyOrderModel> MyOrderDetailList
        {
            get { return _myOrderDetailList; }
            set { _myOrderDetailList = value; RaisePropertyChanged(nameof(MyOrderDetailList)); }
        }
        public MyOrderDetailsPageViewModel()
        {
            MyOrderDetailList = new ObservableCollection<IMark.Data.Models.Common.MyOrderModel>();
        }
        internal void Init(IMark.Data.Models.Common.MyOrderModel myOrderMdl)
        {
            MyOrderDetailList = new ObservableCollection<IMark.Data.Models.Common.MyOrderModel>();
            MyOrderDetailList.Add(myOrderMdl);
        }
        public ICommand PublishCommand => new Command(async (obj) =>
        {

            await App.Current.MainPage.Navigation.PopModalAsync();

        });
        
        public ICommand AddPicturesCommand => new Command(async (obj) =>
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    UserDialogs.Instance.Alert("no upload", "picking a photo is not supported", "ok");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)
                    return;
            }
            catch (Exception ex) 
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        });
    }
}
