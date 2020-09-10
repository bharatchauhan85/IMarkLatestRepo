using Acr.UserDialogs;
using IMark.Areas.Authentication.Views;
using IMark.Areas.ViewModels;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Behaviors;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.Authentication.ViewModels
{
    public class RegisterPageViewModel : BasePageViewModel
    {
        IApiService _apiService;

        public string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                if (string.IsNullOrEmpty(_firstName))
                    IsFirstNameErrorVisible = true;
                else
                    IsFirstNameErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                if (string.IsNullOrEmpty(_lastName))
                    IsLastNameErrorVisible = true;
                else
                    IsLastNameErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(LastName));
            }
        }
        //private string _firstName;
        //public string FirstName
        //{
        //    get { return _firstName; }
        //    set { _firstName = value; RaisePropertyChanged(); }
        //}

        //private string _lastName;
        //public string LastName
        //{
        //    get { return _lastName; }
        //    set { _lastName = value; RaisePropertyChanged(); }
        //}
        private string _organizationName;
        public string OrganizationName
        {
            get { return _organizationName; }
            set { _organizationName = value; RaisePropertyChanged(); }
        }
       
       
        private bool _isPassword;
        public bool IsPassword
        {
            get { return _isPassword; }
            set { _isPassword = value; RaisePropertyChanged(); }
        }
        private string _imageVisible;
        public string ImageVisible
        {
            get { return _imageVisible; }
            set { _imageVisible = value; RaisePropertyChanged(); }
        }
        private bool _isPasswordErrorVisible;
        public bool IsPasswordErrorVisible
        {
            get { return _isPasswordErrorVisible; }
            set { _isPasswordErrorVisible = value; RaisePropertyChanged(); }
        }

        private bool _isEmailErrorVisible;
        public bool IsEmailErrorVisible
        {
            get { return _isEmailErrorVisible; }
            set { _isEmailErrorVisible = value; RaisePropertyChanged(); }
        }

        private bool _isFirstNameErrorVisible;
        public bool IsFirstNameErrorVisible
        {
            get { return _isFirstNameErrorVisible; }
            set { _isFirstNameErrorVisible = value; RaisePropertyChanged(); }
        }

        private bool _isLastNameErrorVisible;
        public bool IsLastNameErrorVisible
        {
            get { return _isLastNameErrorVisible; }
            set { _isLastNameErrorVisible = value; RaisePropertyChanged(); }
        }

        private bool _isRegisterButtonEnabled;

        public bool IsRegisterButtonEnabled
        {
            get { return _isRegisterButtonEnabled; }
            set { _isRegisterButtonEnabled = value; RaisePropertyChanged(); }
        }



        public ICommand password_showCommand => new Command(password_Show);
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

        public bool CheckValidation()
        {
            if (IsEmailErrorVisible ||
                IsPasswordErrorVisible||IsFirstNameErrorVisible||IsLastNameErrorVisible
               )
            {
                IsRegisterButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Email)||string.IsNullOrEmpty(FirstName)||string.IsNullOrEmpty(LastName)
                )
            {
                IsRegisterButtonEnabled = false;
                return false;
            }
            else
            {
                IsRegisterButtonEnabled = true;
                return true;
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
        private void password_Show(object obj)
        {
          if(IsPassword)
            {
                ImageVisible = "iconfinder_eye_226579";
                IsPassword = false;
            }
          else
            {
                IsPassword = true;
                ImageVisible = "hide.png";
            }
        }

        public async Task IntitializeErrorVisible()
        {
            IsEmailErrorVisible = false;
            IsPasswordErrorVisible = false;
            IsFirstNameErrorVisible = false;
            IsLastNameErrorVisible = false;
        }

        public RegisterPageViewModel(IApiService apiService)
        {
            _apiService = apiService;
            FirstName = string.Empty;
            LastName = string.Empty;
            IsPassword = true;
            ImageVisible = "hide.png";
            OrganizationName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
        public ICommand RegisterCommand => new Command(async (obj) =>
         {
             try
             {
             
                 if (string.IsNullOrEmpty(FirstName))
                 {
                     UserDialogs.Instance.Alert("Please enter the first name", "Error", "Ok");
                 }
                 else if (string.IsNullOrEmpty(LastName))
                 {
                     UserDialogs.Instance.Alert("Please enter the last name", "Error", "Ok");
                 }
                 else if (string.IsNullOrEmpty(Email))
                 {
                     UserDialogs.Instance.Alert("Please enter the emailid.", "Error", "Ok");
                     return;
                 }
                 else if (!Regex.IsMatch(Email, RegexBehavior.emailRegex().ToString()))
                 {
                     UserDialogs.Instance.Alert("Please enter the valid emailid.", "Error", "Ok");
                     return;
                 }
                 else if (string.IsNullOrEmpty(Password))
                 {
                     UserDialogs.Instance.Alert("Please enter the Password.", "Error", "Ok");
                     return;
                 }
                 else if (Password.Length <8)
                 {
                     UserDialogs.Instance.Alert("Password length must have atleast 8 characters !!", "Error", "Ok");
                     return;
                 }
                 else if (!Regex.IsMatch(Password, RegexBehavior.passwordRegex().ToString()))
                 {
                     UserDialogs.Instance.Alert("Password should contain atleast one upper case, one lower case, one numeric value and one special character.");
                     return;
                 }

                 else
                 {
                 UserDialogs.Instance.ShowLoading();
                 CreateCusomerRequest obj_cusrequst = new CreateCusomerRequest();
                 obj_cusrequst.input = new Input();
                 obj_cusrequst.input.firstName = FirstName;
                 obj_cusrequst.input.lastName = LastName;
                 //obj_cusrequst.input.phone = "+87698792397";
                 obj_cusrequst.input.password = Password;
                 obj_cusrequst.input.email = Email;
                 string query = @"mutation customerCreate($input: CustomerCreateInput!) {customerCreate(input: $input) { customer { id } customerUserErrors { code field message } } }";
                 var res = await _apiService.CreateCusmerbyGraphql(query, obj_cusrequst);
                 if (res != null)
                 {
                     if (res.data.customerCreate.customer != null)
                     {
                         UserDialogs.Instance.HideLoading();
                         //  Settings.Customer_Id = Convert.ToString(res.customerCreate.customer.id);
                       
                             FirstName = string.Empty;
                             LastName = string.Empty;
                             OrganizationName = string.Empty;
                             Email = string.Empty;
                             Password = string.Empty;
                             SettingExtension.CheckoutId = string.Empty;
                             await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                     }
                     else if (res.data.customerCreate.customerUserErrors.Count > 0)
                     {
                         UserDialogs.Instance.HideLoading();
                         string cs = Convert.ToString(res.data.customerCreate.customerUserErrors[0].message);
                         UserDialogs.Instance.Alert(cs);
                     }
                     else
                     {
                         UserDialogs.Instance.HideLoading();
                         UserDialogs.Instance.Alert("There are some problem");
                     }
                 }
                 else
                 {
                     UserDialogs.Instance.HideLoading();
                     UserDialogs.Instance.Alert("Server not connected");
                 }
             }
         }

            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message.ToString());
            }
        });


        public ICommand SigninCommand => new Command(async (obj) =>
         {
             await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
         });
    }
}