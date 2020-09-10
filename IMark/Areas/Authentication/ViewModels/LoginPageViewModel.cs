using Acr.UserDialogs;
using IMark.Areas.Authentication.Views;
using IMark.Areas.ViewModels;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Areas.Views.MasterDetailsPage.ViewModels;
using IMark.Behaviors;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Helpers;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IMark.Areas.Authentication.ViewModels
{
    public class LoginPageViewModel :BasePageViewModel
    {

        public ICommand password_showCommand { get; set; }
        IApiService _apiservice;
       
        

        private string _facebookLink;
        public string FacebookLink
        {
            get { return _facebookLink; }
            set { _facebookLink = value; RaisePropertyChanged(); }
        }
        private string _googleLink;
        public string GoogleLink
        {
            get { return _googleLink; }
            set { _googleLink = value; RaisePropertyChanged(); }
        }
        private string _imageVisible;

        public string ImageVisible
        {
            get { return _imageVisible; }
            set { _imageVisible = value; RaisePropertyChanged(); }
        }
        private bool _isPassword;

        public bool IsPassword
        {
            get { return _isPassword; }
            set { _isPassword = value; RaisePropertyChanged(); }
        }
        private bool _isEmailErrorVisible;

        public bool IsEmailErrorVisible
        {
            get { return _isEmailErrorVisible; }
            set { _isEmailErrorVisible = value; RaisePropertyChanged(); }
        }
        private bool _isPasswordErrorVisible;

        public bool IsPasswordErrorVisible
        {
            get { return _isPasswordErrorVisible; }
            set { _isPasswordErrorVisible = value; RaisePropertyChanged(); }
        }

        private bool _isSignInButtonEnabled;

        public bool IsSignInButtonEnabled
        {
            get { return _isSignInButtonEnabled; }
            set { _isSignInButtonEnabled = value; RaisePropertyChanged(); }
        }

        public string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                if (RegexUtilities.EmailValidation(_email))
                    IsEmailErrorVisible = false;
                else
                    IsEmailErrorVisible = true;
                CheckValidation();
                RaisePropertyChanged(nameof(Email));
            }
        }


        public string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                if (RegexUtilities.PasswordValidation(_password))
                    IsPasswordErrorVisible = false;
                else
                    IsPasswordErrorVisible = true;
                CheckValidation();
                RaisePropertyChanged(nameof(Password));
            }
        }
        public bool CheckValidation()
        {
            if (IsEmailErrorVisible ||
                IsPasswordErrorVisible
               )
            {
                IsSignInButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Email)
                )
            {
                IsSignInButtonEnabled = false;
                return false;
            }
            else
            {
                IsSignInButtonEnabled = true;
                return true;
            }
        }

        public async Task IntitializeErrorVisible()
        {
            IsEmailErrorVisible = false;
            IsPasswordErrorVisible = false;
        }

        public LoginPageViewModel(IApiService apiService)
        {
            ImageVisible = "hide.png";
            IsPassword = true;
            _apiservice = apiService;
            FacebookLink = "https://www.facebook.com";
            GoogleLink = "https://www.google.com";
            password_showCommand = new Command(PasswordShow);
            Email = "";
            Password = "";
        }



        private void PasswordShow(object obj)
        {
            if (IsPassword)
            {
                IsPassword = false;
                ImageVisible = "iconfinder_eye_226579";
            }
            else
            {
                IsPassword = true;
                ImageVisible = "hide.png";
            }
        }

        internal void init()
        {
            Email = "";
            Password = "";
        }

        public ICommand ForgotPassword => new Command(async(obj) =>
        {            
            await App.Current.MainPage.Navigation.PushModalAsync(new ForgetPasswordPage());
           // UserDialogs.Instance.Alert("In progress");
        });
        public ICommand SigninCommand => new Command(async (obj) =>
        {
            UserDialogs.Instance.ShowLoading();
            try
            {

                if (string.IsNullOrEmpty(Email))
                {
                    UserDialogs.Instance.Alert("Email can not be blank.");
                    return;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    UserDialogs.Instance.Alert("Password can not be blank.");
                    return;
                }
                string queryid_id = @"mutation customerAccessTokenCreate($input: CustomerAccessTokenCreateInput!) {
  customerAccessTokenCreate(input: $input) {
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
                LoginRequest loginRequest = new LoginRequest();
                loginRequest.input = new Input_Log();
                loginRequest.input.email = Email;
                loginRequest.input.password = Password;
               
                var res = await _apiservice.CustomerLogin(queryid_id, loginRequest);
              
                if (res.data != null)
                {

                    if (!string.IsNullOrEmpty(Convert.ToString(res.data.customerAccessTokenCreate.customerAccessToken)))
                    {

                       // SettingExtension.AccessToken = Convert.ToString(res.data.customerAccessTokenCreate.customerAccessToken.accessToken);
                        SettingExtension.AccessToken = res.data.customerAccessTokenCreate.customerAccessToken.accessToken;
                        
                            //  UserDialogs.Instance.ShowLoading();
                            //await NavigationService.NavigateToAsync(new MainMenuViewModel());
                          await  App.Current.MainPage.Navigation.PushAsync(new MainMenu());
                            //App.Locator.Home.InitilizeData();
                            Settings.IsWalkthroughCompleted = true;
                            Email = string.Empty;
                            Password = string.Empty;
                            IsEmailErrorVisible = false;
                            IsPasswordErrorVisible = false;
                      
                        //  UserDialogs.Instance.HideLoading();
                    }
                    else if (res.data.customerAccessTokenCreate.customerUserErrors.Count > 0)
                    {
                        string msg = Convert.ToString(res.data.customerAccessTokenCreate.customerUserErrors[0].message);
                        UserDialogs.Instance.Alert("Please enter the valid emailid and password.");
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Please enter the valid emailid and password.");
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Server not connected", "Error", "Ok");
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
            UserDialogs.Instance.HideLoading();
        });

        public ICommand NewaccountCommand => new Command(async (obj) =>
          {
                await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
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
        public Command GoogleCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Browser.OpenAsync(GoogleLink, BrowserLaunchMode.SystemPreferred);
                });
            }
        }
    }
}