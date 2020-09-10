using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Behaviors;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Database;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
    public class UpdateAddressPageViewModel : BasePageViewModel
    {
        IApiService _apiService;

        public string _addressId;
        public string AddressId
        {
            get { return _addressId; }
            set
            {
                _addressId = value;
               
                RaisePropertyChanged(nameof(AddressId));
            }
        }

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
        public string _province;
        public string Province
        {
            get { return _province; }
            set
            {
                _province = value;
                if (string.IsNullOrEmpty(_province))
                    IsProvinceErrorVisible = true;
                else
                    IsProvinceErrorVisible = false;
                CheckValidation();
                RaisePropertyChanged(nameof(Province));
            }
        }
        public bool _isProvinceErrorVisible;
        public bool IsProvinceErrorVisible
        {
            get { return _isProvinceErrorVisible; }
            set { _isProvinceErrorVisible = value; RaisePropertyChanged(nameof(IsProvinceErrorVisible)); }
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


        public bool _isSavedButtonEnabled;
        public bool IsSavedButtonEnabled
        {
            get { return _isSavedButtonEnabled; }
            set { _isSavedButtonEnabled = value; RaisePropertyChanged(nameof(IsSavedButtonEnabled)); }
        }
        public string _address2;
        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; RaisePropertyChanged(nameof(Address2)); }
        }
        public async Task IntitializeErrorVisible()
        {
            IsFirstNameErrorVisible = false;
            IsLastNameErrorVisible = false;
            IsAddressErrorVisible = false;
            IsCountryErrorVisible = false;
            IsCityErrorVisible = false;
            IsPostalCodeErrorVisible = false;
            IsProvinceErrorVisible = false;
            IsPhoneErrorVisible = false;

        }
        public bool CheckValidation()
        {
            if (IsFirstNameErrorVisible ||
                IsLastNameErrorVisible || IsAddressErrorVisible || IsCityErrorVisible || IsPostalCodeErrorVisible || IsCountryErrorVisible||IsProvinceErrorVisible||IsPhoneErrorVisible)


            {
                IsSavedButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Address1) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(PostalCode)||string.IsNullOrEmpty(Province)||string.IsNullOrEmpty(Phone)
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
        private EditSaveAddressModel _editSave;
        public UpdateAddressPageViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ObservableCollection<EditSaveAddressModel>> GetEditAddress(EditSaveAddressModel editSave)
        {
            _editSave = editSave;
            ObservableCollection<EditSaveAddressModel> obj_new = new ObservableCollection<EditSaveAddressModel>();
            Customer_Add obj_listAddress = new Customer_Add();
            string accestoken = Convert.ToString(SettingExtension.AccessToken);
            char quote = '"';
            string modifiedString = quote + accestoken + quote;
            string query = "{ customer(customerAccessToken:" + modifiedString + "){ id addresses(first: 20){ edges{  node{id firstName lastName address1 address2 city company province phone country zip } } } } }";
            //  UserDialogs.Instance.ShowLoading();
            var res = await _apiService.GetCustomer(query);
            //  UserDialogs.Instance.HideLoading();
            if (res != null)
            {
                AddressId = _editSave.ID;
                FirstName = _editSave.firstName;
                LastName = _editSave.lastName;
                Address1 = _editSave.address1;
                Address2 = _editSave.address2;
                City = _editSave.city;
                Country = _editSave.country;
                PostalCode = _editSave.zip;
                Province = _editSave.province;
                Phone = _editSave.phone;
                //var list = res.data?.customer?.addresses?.edges?.FirstOrDefault(s => s.node.id == editSave.ID);
            }
           
             
            
            else
            {
                obj_new.Clear();
            }
            return obj_new;
        }
        public ICommand SavedCommand => new Command(async (obj) =>
        {
                try
                {
                    UpdateAddressRequest createAddressReequest = new UpdateAddressRequest();
                    
                    createAddressReequest.customerAccessToken = Convert.ToString(SettingExtension.AccessToken);
                    createAddressReequest.address = new Address();
                createAddressReequest.id = _editSave.ID;
                createAddressReequest.address.firstName = FirstName;
                    createAddressReequest.address.lastName = LastName;
                    createAddressReequest.address.address1 = Address1;
                    createAddressReequest.address.address2 = Address2;
                    createAddressReequest.address.city = City;
                    createAddressReequest.address.country = Country;
                createAddressReequest.address.province = Province;
                createAddressReequest.address.phone = Phone;
                    createAddressReequest.address.zip = PostalCode;
                    string query = @"mutation customerAddressUpdate($customerAccessToken: String!, $id: ID!, $address: MailingAddressInput!) {
                                  customerAddressUpdate(customerAccessToken: $customerAccessToken, id: $id, address: $address) {
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
                    var res = await _apiService.UpdateAddress(query, createAddressReequest);
                    if (res != null)
                    {
                        if (res.data.customerAddressUpdate.customerAddress.id != null)
                        {
                            string accestoken = Convert.ToString(SettingExtension.AccessToken);
                            char quote = '"';
                            string modifiedString = quote + accestoken + quote;
                            string qury = "{ customer(customerAccessToken:" + modifiedString + "){ addresses(first: 9){ edges{ node{ firstName lastName address1 address2 city company country zip } } } } }";
                            var res1 = await _apiService.GetAddress(qury);
                            foreach (var item in res1.data.customer.addresses.edges)
                            {
                                Customer_Add customer_Sql = new Customer_Add();
                                customer_Sql.firstName = Convert.ToString(item.node.firstName);
                                customer_Sql.lastName = Convert.ToString(item.node.lastName);
                                customer_Sql.address1 = Convert.ToString(item.node.address1);
                                customer_Sql.address2 = Convert.ToString(item.node.address2);
                                customer_Sql.company = Convert.ToString(item.node.company);
                                customer_Sql.country = Convert.ToString(item.node.country);
                            customer_Sql.province = Convert.ToString(item.node.province);
                            customer_Sql.phone = Convert.ToString(item.node.phone);
                                customer_Sql.zip = item.node.zip;
                                await App.Database.SaveNoteAsync(customer_Sql);
                            }
                            App.Locator.EditSaveAddress.Init();
                            UserDialogs.Instance.HideLoading();
                            var cfg = new ToastConfig("Address Updated successfully.")
                            {
                                BackgroundColor = Color.Green
                            };
                            UserDialogs.Instance.Toast(cfg);
                        }
                        else if (res.data.customerAddressUpdate.customerUserErrors.Count > 0)
                        {
                            UserDialogs.Instance.HideLoading();
                            string cs = Convert.ToString(res.data.customerAddressUpdate.customerUserErrors[0]);
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
                        UserDialogs.Instance.Alert("Server not connected.");
                    }

                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert(ex.Message.ToString());
                }
        });
    }
}
    

