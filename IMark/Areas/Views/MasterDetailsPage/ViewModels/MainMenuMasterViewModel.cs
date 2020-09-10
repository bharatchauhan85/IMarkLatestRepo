using Acr.UserDialogs;
using IMark.Areas.Authentication.Views;
using IMark.Core.Helpers;
using IMark.Database;
using IMark.Helpers;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace IMark.Areas.Views.MasterDetailsPage.ViewModels
{
   public class MainMenuMasterViewModel : BasePageViewModel
    {
		IApiService _apiService;

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
		private string _userName;
		public string UserName

		{
			get { return _userName; }
			set { _userName = value; RaisePropertyChanged(); }
		}

		private string _userMailId;
		public string UserMailId

		{
			get { return _userMailId; }
			set { _userMailId = value; RaisePropertyChanged(); }
		}
		private string _facebookLink;
		public string FacebookLink
		{
			get { return _facebookLink; }
			set { _facebookLink = value; RaisePropertyChanged(); }
		}
		private string _instagram;
		public string Instagram
		{
			get { return _instagram; }
			set { _instagram = value; RaisePropertyChanged(); }
		}
		private string _twitter;
		public string Twitter
		{
			get { return _twitter; }
			set { _twitter = value; RaisePropertyChanged(); }
		}
		private string _pinterest;
		public string Pinterest
		{
			get { return _pinterest; }
			set { _pinterest = value; RaisePropertyChanged(); }
		}
		private string _tumblr;
		public string Tumblr
		{
			get { return _tumblr; }
			set { _tumblr = value; RaisePropertyChanged(); }
		}
		public MainMenuMasterViewModel(IApiService apiService)
        {
			_apiService = apiService;
			FacebookLink = "https://www.facebook.com/";
			Instagram = "https://www.instagram.com/accounts/login/?hl=en";
			Twitter = "https://twitter.com/login";
			Pinterest = "https://www.tumblr.com/login";
			Tumblr = "https://in.pinterest.com/login/";
			try
			{
				if (SettingExtension.UserSetting.UserImage == null)
				{
					ProfileImageSource = "Profilepic";
				}
				else
				{
					ProfileImageSource = ImageSource.FromStream(() => new MemoryStream(SettingExtension.UserSetting.UserImage));
				}
			}
			catch
			{
				ProfileImageSource = "Profilepic";
			}
		}
		 
        public ICommand ShopCommand => new Command(async (obj) => 
		{
			await App.Current.MainPage.Navigation.PushAsync(new MainMenu());
		});
        public ICommand PhotoGalleryCommand => new Command(async (obj) => 
		{
			 await App.Current.MainPage.Navigation.PushModalAsync(new PhotoGallery());
		});
		public ICommand OurContributionCommand => new Command(async (obj) =>
		{
			 await App.Current.MainPage.Navigation.PushModalAsync(new OurContribution());
		});
		public ICommand PartnerBrandsCommand => new Command(async (obj) =>
		{
			 await App.Current.MainPage.Navigation.PushModalAsync(new PartnerBrands());
		});

        public  void UpdateImage()
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

        public ICommand ViewEntireCommand => new Command(async (obj) =>
        {
			UserDialogs.Instance.Alert("Not Implemented");
			// await App.Current.MainPage.Navigation.PushModalAsync(new SettingPage());
		});
		public ICommand HelpCommand => new Command(async (obj) =>
        {
			UserDialogs.Instance.Alert("Coming Soon");
			// await App.Current.MainPage.Navigation.PushModalAsync(new SettingPage());
		});
		public ICommand LogoutCommand => new Command(async (obj) =>
		{
			try
			{
				bool? result = await ShowConfirmAlert("Are you sure you want to logout?.", "Logout!", "Yes", "No");
				if (result == null || result.Value == false)
				{
					return;
				}
				else
				{
					UserDialogs.Instance.ShowLoading();
					//  await App.Current.MainPage.Navigation.PopToRootAsync(false);
					//await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());

					Customer_Sqlite obj_ne = new Customer_Sqlite();
					Customer_Add obj_de = new Customer_Add();
					await App.Database.DeleteCustomer(obj_ne);
					await App.Database.DeleteNoteAsync(obj_de);
					//   Settings.IsWalkthroughCompleted = false;
					SettingExtension.AccessToken = "";
					Settings.Customer_FName = "";
					Settings.Customer_LName = "";
					Settings.Customer_Mobile = "";
					Settings.Customer_Email = "";
					Settings.Customer_Id = "";
					SettingExtension.CheckoutId = "";
					Application.Current.Properties.Clear();
					UserDialogs.Instance.HideLoading();
					await App.Container.GetInstance<Services.Interfaces.INavigationService>().InitializeAsync();
				}
			}
			catch (Exception ex)
			{
				UserDialogs.Instance.Toast(ex.Message);
				UserDialogs.Instance.HideLoading();
			}
		});
		public ICommand SettingCommand => new Command(async (obj) =>
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new SettingPage());
        });
		
		
		public ICommand AboutUsCommand => new Command(async (obj) =>
		{
			//await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
			UserDialogs.Instance.Alert("Coming Soon");
		});
		public ICommand TermsCommand => new Command(async (obj) =>
		{
			//await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
			UserDialogs.Instance.Alert("Coming Soon");
		});
		public ICommand PrivacyCommand => new Command(async (obj) =>
		{
			UserDialogs.Instance.Alert("Coming Soon");
			//await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
		});
		public ICommand WarningCommand => new Command(async (obj) =>
		{
			UserDialogs.Instance.Alert("Coming Soon");
			//await App.Current.MainPage.Navigation.PushModalAsync(new CartPage());
		});

		public ICommand DistributorCommand => new Command(async (obj) =>
		{
			await App.Current.MainPage.Navigation.PushModalAsync(new BecomeDistributor());
		});
		public Command FacebookCommand
		{
			get
			{
				return new Command(async () =>
				{
					await Browser.OpenAsync(FacebookLink, BrowserLaunchMode.SystemPreferred);
				});
			}
		}
		public Command InstagramCommand
		{
			get
			{
				return new Command(async () =>
				{
					await Browser.OpenAsync(Instagram, BrowserLaunchMode.SystemPreferred);
				});
			}
		}
		public Command TwitterCommand
		{
			get
			{
				return new Command(async () =>
				{
					await Browser.OpenAsync(Twitter, BrowserLaunchMode.SystemPreferred);
				});
			}
		}
		public Command PinterestCommand
		{
			get
			{
				return new Command(async () =>
				{
					await Browser.OpenAsync(Pinterest, BrowserLaunchMode.SystemPreferred);
				});
			}
		}
		public Command TumblrCommand
		{
			get
			{
				return new Command(async () =>
				{
					await Browser.OpenAsync(Tumblr , BrowserLaunchMode.SystemPreferred);
				});
			}
		}


		public async Task InitializeData()
        {
			//var id = "c0b2d7e2902283c75d30e185ae7a277a";
			
			try
			{
				string queryid_id = "{ customer(customerAccessToken:\"" + SettingExtension.AccessToken + "\"){ id firstName lastName email createdAt } }";
				var res = await _apiService.CustomerInfo(queryid_id);
				UserDialogs.Instance.HideLoading();
				if (res.data != null)
				{
					if (!string.IsNullOrEmpty(Convert.ToString(res.data.customer.id)))
					{
						UserName = res.data.customer.firstName + res.data.customer.lastName;
						UserMailId = res.data.customer.email;
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
			catch (Exception ex)
			{
				UserDialogs.Instance.HideLoading();
				UserDialogs.Instance.Alert(ex.Message.ToString());
			}
		}
        }

	}

