using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using IMark.Core.Helpers;
using IMark.Core.Models.Common;
using IMark.Data.Models.Response;
using IMark.Data.Models.Request;
using IMark.Service.Interfaces;

namespace IMark.Areas.ViewModels
{
    public class AccountSettinPageViewModel : BasePageViewModel
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
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(); }
        }

        public AccountSettinPageViewModel(IApiService apiService)
        {
            _apiService = apiService;
            ProfileName = "Jenny Smith";
            ProfileEmail = "jennysmith@xyz.com";
            PhoneNumber = "+00-123465789";
            Password = "*******";
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

        public ICommand ProfilePicCommand => new Command(async (obj) =>
        {
            try
            {
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file != null)
                {
                    //imagename = file.Path;
                    ProfileImageSource = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                    UserSettingData userSetting = new UserSettingData();
                    SettingExtension.UserSetting = userSetting;
                    var data = ImageHelper.ReadFully(file.GetStream());
                    SettingExtension.UserSetting.UserImage = new byte[16 * 1024];
                    App.Current.Properties["UserImage"] = data;
                    App.Locator.MainMenuMaster.UpdateImage();
                    App.Locator.ProfilePage.UpdateImage();
                }
                else
                {
                    UserDialogs.Instance.Alert("Picking a photo is not supported", "No upload", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.ToString(), "Error", "OK");
            }
        });
        public ICommand SaveCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        });

        public async void InitializeUserInfo(Customer customer)
        {
            try
            {

              
                string queryid_id = @"mutation customerUpdate($customerAccessToken: String!, $customer: CustomerUpdateInput!) {
                          customerUpdate(customerAccessToken: $customerAccessToken, customer: $customer) {
                            customer {
                              id
                            }
                            customerAccessToken {
                              accessToken
                              expiresAt
                            }
                            customerUserErrors {
                              code
                              field
                              message
                            }
                          }
                        }";
                CustomerUpdateRequest request = new CustomerUpdateRequest();
                request.customer = new UpdateCustomer();
                if(ProfileName.Contains(" "))
                {
                    request.customer.firstName = ProfileName.Split(' ')[0];
                    request.customer.lastName = ProfileName.Split(' ')[1];
                }
                else
                {
                    request.customer.firstName = ProfileName;
                    request.customer.lastName = string.Empty;
                }
                request.customer.email = ProfileEmail;
                request.customer.phone = PhoneNumber;

                var res = await _apiService.CustomerUpdate(queryid_id, request);

                if (res.data != null)
                {
                }
                else
                {
                    //UserDialogs.Instance.Alert("Server not connected", "Error", "Ok");
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
            UserDialogs.Instance.HideLoading();
        }
    }
    public static class ImageHelper
    {
        //public static MemoryStream ConvertStreamToMemoryStream(Stream stream)
        //{
        //    MemoryStream memoryStream = new MemoryStream();

        //    if (stream != null)
        //    {

        //        byte[] buffer = ReadFully(stream);

        //        if (buffer != null)
        //        {
        //            var binaryWriter = new BinaryWriter(memoryStream);
        //            binaryWriter.Write(buffer);
        //        }
        //    }
        //    return memoryStream;
        //}
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}