using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Core.Helpers;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class ProfilePageViewModel : BasePageViewModel
    {
        private IApiService _apiService;
        private ImageSource _profileImageSource;
        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                RaisePropertyChanged();
            }
        }
        private string _profilePic;
        public string ProfilePic
        {
            get { return _profilePic; }
            set { _profilePic = value; RaisePropertyChanged(); }
        }
        private string _profileName;
        public string ProfileName
        {
            get { return _profileName; }
            set { _profileName = value; RaisePropertyChanged(); }
        }
        private string _profileEmail;
        public string ProfileEmail
        {
            get { return _profileEmail; }
            set { _profileEmail = value; RaisePropertyChanged(); }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; RaisePropertyChanged(); }
        }

        public ProfilePageViewModel(IApiService apiService)
        {
            //ProfilePic = "Profilepic";
            //ProfileName = "Jenny Smith";
            //ProfileEmail = "jennysmith@xyz.com";
            //PhoneNumber = "+00-123465789";
            _apiService = apiService;
            GetCustomerInformation();
            try
            {
                if (App.Current.Properties["UserImage"] as byte[] == null)
                {
                    ProfileImageSource = "Profilepic";
                }
                else
                {
                    ProfileImageSource = ImageSource.FromStream(() => new MemoryStream(App.Current.Properties["UserImage"] as byte[]));
                }
            }
            catch
            {
                ProfileImageSource = "Profilepic";
            }
        }

        private async Task GetCustomerInformation()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                string queryid_id = "{ customer(customerAccessToken:\"" + SettingExtension.AccessToken + "\"){ id firstName lastName email createdAt } }";
                var res = await _apiService.CustomerInfo(queryid_id);
                UserDialogs.Instance.HideLoading();
                if (res.data != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(res.data.customer.id)))
                    {
                        ProfileName = res.data.customer.firstName + " " + res.data.customer.lastName;
                        ProfileEmail = res.data.customer.email;
                        PhoneNumber = res.data.customer.phone;
                        SettingExtension.UserEmail = res.data.customer.email;
                        App.Locator.ProfilePage.InitializeUserInfo(res.data.customer);
                        App.Locator.AccountSettinPage.InitializeUserInfo(res.data.customer);
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Please enter the valid emailid and password.", "Error", "Ok");
                    }
                }
                else
                {
                    //UserDialogs.Instance.Alert("Server not connected", "Error", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
             
            }
            catch
            {
                await ShowAlert("Internal server error");
            }
            UserDialogs.Instance.HideLoading();
        }

        public Command AccountSettingCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushModalAsync(new AccountSettinPage());
                });
            }
        }
        public Command SaveAddressCommand
        {
            get
            {
                return new Command(async () =>
                {
                    App.Locator.EditSaveAddress.Init();
                    await App.Current.MainPage.Navigation.PushModalAsync(new EditSaveAddress());
                });
            }
        }
        public Command MyOrderCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Locator.MyOrderPage.InitializeData();
                    await App.Current.MainPage.Navigation.PushModalAsync(new MyOrderPage());
                });
            }
        }

        public void UpdateImage()
        {
            try
            {
                if (App.Current.Properties["UserImage"] as byte[] == null)
                {
                    ProfileImageSource = "Profilepic";
                }
                else
                {
                    ProfileImageSource = ImageSource.FromStream(() => new MemoryStream(App.Current.Properties["UserImage"] as byte[]));
                }
            }
            catch
            {
                ProfileImageSource = "Profilepic";
            }
        }

        public Command MyFavoritsCommand
        {
            get
            {
                return new Command(async () =>
                {

                    await App.Current.MainPage.Navigation.PushModalAsync(new FavoritesPage());
                });
            }
        }

        internal void InitializeUserInfo(Customer customer)
        {
            ProfilePic = "Profilepic";
            ProfileName = customer.firstName + " " + customer.lastName;
            ProfileEmail = customer.email;

        }
    }
}
