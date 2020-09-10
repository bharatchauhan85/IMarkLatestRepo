using Acr.UserDialogs;
using IMark.Areas.Model;
using IMark.Areas.Views;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Database;
using IMark.Helpers;
using IMark.Service.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IMark.Areas.ViewModels
{
   public class EditSaveAddressViewModel : BasePageViewModel
    {
        IApiService _apiService;
        private ObservableCollection<EditSaveAddressModel> _editAddressList;
        public ObservableCollection<EditSaveAddressModel> EditAddressList
        {
            get { return _editAddressList; }
            set { _editAddressList = value; RaisePropertyChanged(); }
        }
        
        public EditSaveAddressViewModel(IApiService apiService)
        {
            _apiService = apiService;
            // EditAddressList = GetEditAddreesList(); 

        }
         public ICommand AddNewCommand => new Command(async (obj) =>
         {
             await App.Current.MainPage.Navigation.PushModalAsync(new AddNewAddress());
         });
         public Command<EditSaveAddressModel> EditCommand
         {
             get
             {
                 return new Command<EditSaveAddressModel>(async (obj) =>
                 {
                     await App.Current.MainPage.Navigation.PushModalAsync(new UpdateAddressPage(obj));
                 });
             }
         }
        public ICommand SelectCommand => new Command<EditSaveAddressModel>(async (obj) =>
        {

            UserDialogs.Instance.ShowLoading();
            try
            {
                string queryid_id = @"mutation checkoutShippingAddressUpdateV2($shippingAddress: MailingAddressInput!, $checkoutId: ID!) {
                                  checkoutShippingAddressUpdateV2(shippingAddress: $shippingAddress, checkoutId: $checkoutId) {
                                    userErrors {
                                      field
                                      message
                                    }
                                    checkout {
                                      id
                                      shippingAddress {
                                        firstName
                                        lastName
                                        address1
                                        province
                                        country
                                        zip
                                      }
                                    }
                                  }
                                }";
                UpdateAddressForShippingRequestModel request = new UpdateAddressForShippingRequestModel();
                request.checkoutId = SettingExtension.CheckoutId;
                UpdateShippingAddress updateShipping = new UpdateShippingAddress();
                request.shippingAddress = updateShipping;
                request.shippingAddress.address1 = obj.address1;
                request.shippingAddress.city = obj.city;
                request.shippingAddress.country = obj.country;
                request.shippingAddress.firstName = obj.firstName;
                request.shippingAddress.lastName = obj.lastName;
                request.shippingAddress.province = obj.province;
                request.shippingAddress.zip = obj.zip;

                var res = await _apiService.UpdateShippingAddress(queryid_id, request);
                if(res.data.checkoutShippingAddressUpdateV2.checkout != null)
                {
                    App.Locator.ShippingConfirmation.InitializeAddress(obj);
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    await ShowAlert("Address not updated successfully.");
                }

            }
            catch
            {
                await ShowAlert("Internal Server Error");
            }
            UserDialogs.Instance.HideLoading();
            
        });
      

        public Command RemoveCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading();
                        var EditAddressMdl = obj as EditSaveAddressModel;
                        EditAddressList.Remove(EditAddressMdl);
                        DeleteCustomerRequest deleteCusRequest = new DeleteCustomerRequest();
                        deleteCusRequest.customerAccessToken = SettingExtension.AccessToken;
                        deleteCusRequest.id = EditAddressMdl.ID;
                        string query = @"mutation customerAddressDelete($id: ID!, $customerAccessToken: String!) {
                        customerAddressDelete(id: $id, customerAccessToken: $customerAccessToken) {
                            customerUserErrors {
                                code
                                field
                              message
                            }
                            deletedCustomerAddressId
                        }
                    }";
                        var res = await _apiService.DeleteAddress(query, deleteCusRequest);
                        UserDialogs.Instance.HideLoading();
                        if (res != null)
                        {
                            if (!string.IsNullOrEmpty(res.data.customerAddressDelete.deletedCustomerAddressId))
                            {
                                var cfg = new ToastConfig("Address Deleted successfully")
                                {
                                    BackgroundColor = Color.Green
                                };
                                UserDialogs.Instance.Toast(cfg);
                                App.Locator.EditSaveAddress.Init();

                            }
                            else if (res.data.customerAddressDelete.customerUserErrors.Count > 0)
                            {
                                UserDialogs.Instance.HideLoading();
                                string cs = Convert.ToString(res.data.customerAddressDelete.customerUserErrors[0].message);
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
                            UserDialogs.Instance.Alert("deleted");
                        }
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(ex.Message.ToString());
                    }
                });
            }
        }


        //public Command RemoveCommand
        //{
        //    get
        //    {
        //        return new Command(async (obj) =>
        //        {
        //            var EditAddressMdl = obj as EditSaveAddressModel;
        //            EditAddressList.Remove(EditAddressMdl); 
        //        });
        //    }
        //}
        public ICommand DoneCommand => new Command(async (obj) =>
         {
             await App.Current.MainPage.Navigation.PopModalAsync();
         });

        public async Task<ObservableCollection<EditSaveAddressModel>> GetEditAddreesList()
        {
            ObservableCollection<EditSaveAddressModel> obj_new = new ObservableCollection<EditSaveAddressModel>();
            List<Customer_Add> obj_listAddress = new List<Customer_Add>();
            string accestoken = Convert.ToString(SettingExtension.AccessToken);
            char quote = '"';
            string modifiedString = quote + accestoken + quote;
            string query = "{ customer(customerAccessToken:" + modifiedString + "){ id addresses(first: 20){ edges{  node{id firstName lastName address1 address2 city company province phone country zip } } } } }";
            //  UserDialogs.Instance.ShowLoading();
            var res = await _apiService.GetCustomer(query);
            //  UserDialogs.Instance.HideLoading();
            if (res != null)
            {
                obj_listAddress = await App.Database.GetNoteAsync();
                foreach (var item in res.data.customer.addresses.edges)
                {
                    obj_new.Add(new EditSaveAddressModel()
                    {
                        Title = item.node.firstName + " " + item.node.lastName + ", " + item.node.address1 + ", " + item.node.address2+", "+ item.node.zip+", "+item.node.country,
                        ID = item.node.id,
                        firstName = item.node.firstName,
                        lastName = item.node.lastName,
                        address1 = item.node.address1,
                        address2 = item.node.address2,
                        city = item.node.city,
                        company = item.node.company,
                        country = item.node.country,
                        province=item.node.province,
                        phone=item.node.phone,
                        zip = item.node.zip
                    });
                }
            }
            else
            {
                obj_new.Clear();
            }

            //obj_listAddress = await App.Database.GetNoteAsync();
            //foreach (var item in obj_listAddress)
            //{
            //    obj_new.Add(new EditSaveAddressModel()
            //    {
            //        Title = item.address1 + " " + item.address2
            //    });

            //}

            return obj_new;

            //return new ObservableCollection<EditSaveAddressModel>()
            //{
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark1,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark2,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark3,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark4,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark5,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark6,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark7,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark8,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark9,NJ 07102"},
            //   new EditSaveAddressModel{ Title="2226 Deer Ridge Drive Netwark10,NJ 07102"}
            //};
        }

        internal async void Init()
        {
            UserDialogs.Instance.ShowLoading();
            EditAddressList = await GetEditAddreesList();
            UserDialogs.Instance.HideLoading();
        }
    }
}