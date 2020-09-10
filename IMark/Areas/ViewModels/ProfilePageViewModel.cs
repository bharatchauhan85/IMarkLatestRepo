using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Core.Helpers;
using IMark.Data.Models.Response;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class ProfilePageViewModel : BasePageViewModel
    {
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
            set { _profilePic = value;RaisePropertyChanged(); }
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
            set { _phoneNumber = value;RaisePropertyChanged(); }
        }

        public ProfilePageViewModel()
        {
                ProfilePic = "Profilepic";
                ProfileName = "Jenny Smith";
                ProfileEmail = "jennysmith@xyz.com";
                PhoneNumber = "+00-123465789";
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
                  await  App.Current.MainPage.Navigation.PushModalAsync(new MyOrderPage());
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
