using Acr.UserDialogs;
using IMark.Areas.Authentication.ViewModels;
using IMark.Areas.Views;
using IMark.Areas.Views.MasterDetailsPage;
using IMark.Behaviors;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Database;
using IMark.Helpers;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class AddNewAddressViewModel : BasePageViewModel 
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

        public bool _isFirstNameErrorVisible;
        public bool IsFirstNameErrorVisible
        {
            get { return _isFirstNameErrorVisible; }
            set { _isFirstNameErrorVisible = value; RaisePropertyChanged(nameof(IsFirstNameErrorVisible)); }
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

        public bool _isLastNameErrorVisible;
        public bool IsLastNameErrorVisible
        {
            get { return _isLastNameErrorVisible; }
            set { _isLastNameErrorVisible = value; RaisePropertyChanged(nameof(IsLastNameErrorVisible)); }
        }

        
             public string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                if (string.IsNullOrEmpty(_phone))
                    IsPhoneErrorVisible = true;
                else
                    IsPhoneErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(Phone));
            }
        }
        public bool _isPhoneErrorVisible;
        public bool IsPhoneErrorVisible
        {
            get { return _isPhoneErrorVisible; }
            set { _isPhoneErrorVisible = value; RaisePropertyChanged(nameof(IsPhoneErrorVisible)); }
        }


        public string _address1;
        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                if (string.IsNullOrEmpty(_address1))
                    IsAddressErrorVisible = true;
                else
                    IsAddressErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(Address1));
            }
        }
            public bool _isAddressErrorVisible;
        public bool IsAddressErrorVisible
        {
            get { return _isAddressErrorVisible; }
            set { _isAddressErrorVisible = value; RaisePropertyChanged(nameof(IsAddressErrorVisible)); }
        }

        public string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                if (string.IsNullOrEmpty(_country))
                    IsCountryErrorVisible = true;
                else
                    IsCountryErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(Country));
            }
        }
        public bool _isCountryErrorVisible;
        public bool IsCountryErrorVisible
        {
            get { return _isCountryErrorVisible; }
            set { _isCountryErrorVisible = value; RaisePropertyChanged(nameof(IsCountryErrorVisible)); }
        }

        public string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                if (string.IsNullOrEmpty(_postalCode))
                    IsPostalCodeErrorVisible = true;
                else
                    IsPostalCodeErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(PostalCode));
            }
        }
        public bool _isPostalCodeErrorVisible;
        public bool IsPostalCodeErrorVisible
        {
            get { return _isPostalCodeErrorVisible; }
            set { _isPostalCodeErrorVisible = value; RaisePropertyChanged(nameof(IsPostalCodeErrorVisible)); }
        }

        public string _state;
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (string.IsNullOrEmpty(_state))
                    IsStateErrorVisible = true;
                else
                    IsStateErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(State));
            }
        }
        public bool _isStateErrorVisible;
        public bool IsStateErrorVisible
        {
            get { return _isStateErrorVisible; }
            set { _isStateErrorVisible = value; RaisePropertyChanged(nameof(IsStateErrorVisible)); }
        }
        public string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                if (string.IsNullOrEmpty(_city))
                    IsCityErrorVisible = true;
                else
                    IsCityErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(City));
            }
        }
        public bool _isCityErrorVisible;
        public bool IsCityErrorVisible
        {
            get { return _isCityErrorVisible; }
            set { _isCityErrorVisible = value; RaisePropertyChanged(nameof(IsCityErrorVisible)); }
        }

        public bool _isSavedButtonEnabled;
        public bool IsSavedButtonEnabled
        {
            get { return _isSavedButtonEnabled; }
            set { _isSavedButtonEnabled = value; RaisePropertyChanged(nameof(IsSavedButtonEnabled)); }
        }

        public async Task IntitializeErrorVisible()
        {
            IsFirstNameErrorVisible = false;
            IsLastNameErrorVisible = false;
            IsAddressErrorVisible = false;
            IsCountryErrorVisible = false;
            IsCityErrorVisible = false;
            IsPostalCodeErrorVisible = false;
            IsStateErrorVisible = false;
            IsPhoneErrorVisible = false;

        }

        public bool CheckValidation()
        {
            if (IsFirstNameErrorVisible ||
                IsLastNameErrorVisible|| IsAddressErrorVisible||IsCityErrorVisible|| IsPostalCodeErrorVisible || IsCountryErrorVisible||IsStateErrorVisible||IsPhoneErrorVisible)


            {
                IsSavedButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(LastName)||string.IsNullOrEmpty(Address1)||string.IsNullOrEmpty(Country)||string.IsNullOrEmpty(City)||string.IsNullOrEmpty(PostalCode)||string.IsNullOrEmpty(State)||string.IsNullOrEmpty(Phone)
                )
            {
                IsSavedButtonEnabled = false;
                return false;
            }
            else
            {
                IsSavedButtonEnabled = true;
                return true;
            }
        }
        private string _mobileNumber;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; RaisePropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged(); }
        }
        //private string _option;
        //public string Option
        //{
        //    get { return _option; }
        //    set { _option = value; RaisePropertyChanged(); }
        //}
        
        private string _address2;
        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; RaisePropertyChanged(); }

        }
        public AddNewAddressViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }
        public ICommand SavedCommand => new Command(async (obj) =>
        {
                try
                {
                    CreateAddressReequest createAddressReequest = new CreateAddressReequest();
                    createAddressReequest.customerAccessToken = Convert.ToString(SettingExtension.AccessToken);
                    createAddressReequest.address = new Address();
                    createAddressReequest.address.firstName = FirstName;
                    createAddressReequest.address.lastName = LastName;
                    createAddressReequest.address.address1 = Address1;
                    createAddressReequest.address.address2 = Address2;
                    createAddressReequest.address.city = City;
                    createAddressReequest.address.country = Country;
                    createAddressReequest.address.zip = PostalCode;
                    createAddressReequest.address.province = State;
                    createAddressReequest.address.phone = Phone;
                    string query = @"mutation customerAddressCreate($customerAccessToken: String!, $address: MailingAddressInput!) {
                      customerAddressCreate(customerAccessToken: $customerAccessToken, address: $address) {
                                customerAddress {
                                    id
                                }
                                customerUserErrors {
                                    code
                                    field
                                  message
                                }
                            }
                        }
                        ";
                    UserDialogs.Instance.ShowLoading();
                    var res = await _apiService.CreateAddress(query, createAddressReequest);
                    if (res.data.customerAddressCreate.customerAddress != null)
                    {
                        if (res.data.customerAddressCreate.customerAddress.id != null)
                        {
                            SettingExtension.AddressId = res.data.customerAddressCreate.customerAddress.id;
                            string accestoken = Convert.ToString(SettingExtension.AccessToken);
                            string addressId = SettingExtension.AddressId;
                            char quote = '"';
                            string modifiedString = quote + accestoken + quote;
                            string qury = "{ customer(customerAccessToken:" + modifiedString + "){ addresses(first: 9){ edges{ node{ firstName lastName address1 address2 city company province country zip phone } } } } }";
                            var res1 = await _apiService.GetAddress(qury);
                            SettingExtension.UserEmail = res1.data.customer.email;
                            foreach (var item in res1.data.customer.addresses.edges)
                            {
                                Customer_Add customer_Sql = new Customer_Add();
                                customer_Sql.id = Convert.ToString(item.node.id);
                                customer_Sql.firstName = Convert.ToString(item.node.firstName);
                                customer_Sql.lastName = Convert.ToString(item.node.lastName);
                                customer_Sql.address1 = Convert.ToString(item.node.address1);
                                customer_Sql.address2 = Convert.ToString(item.node.address2);
                                customer_Sql.company = Convert.ToString(item.node.company);
                                customer_Sql.country = Convert.ToString(item.node.country);
                                customer_Sql.city = Convert.ToString(item.node.city);
                                customer_Sql.zip = Convert.ToString(item.node.zip);
                                customer_Sql.phone = Convert.ToString(item.node.phone);
                                customer_Sql.province = Convert.ToString(item.node.province);
                                customer_Sql.zip = item.node.zip;
                           
                                await App.Database.SaveNoteAsync(customer_Sql);
                            }
                        
                            App.Locator.EditSaveAddress.Init();
                        FirstName = "";
                        LastName = "";
                        Address1 = "";
                        Address2 = "";
                        City = "";
                        Country = "";
                        PostalCode = "";
                        Phone = "";
                        State = "";
                        UserDialogs.Instance.HideLoading();
                            var cfg = new ToastConfig("Address added successfully.")
                            {
                                BackgroundColor = Color.Green
                            };
                            UserDialogs.Instance.Toast(cfg);
                        }
                        else if (res.data.customerAddressCreate.customerUserErrors.Count > 0)
                        {
                            UserDialogs.Instance.HideLoading();
                            string cs = Convert.ToString(res.data.customerAddressCreate.customerUserErrors[0].message);
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
                        
                        UserDialogs.Instance.Alert(res.data.customerAddressCreate.customerUserErrors[0].message.ToString());
                    UserDialogs.Instance.HideLoading();
                }

                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
               // UserDialogs.Instance.Alert("Please select Province");
              
                    //UserDialogs.Instance.Alert(ex.Message.ToString());
                }
        });
    }
}