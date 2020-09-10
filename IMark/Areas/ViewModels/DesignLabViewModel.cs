using Acr.UserDialogs;
using IMark.Areas.Views;
using IMark.Core.Helpers;
using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Service.Interfaces;
using IMark.Services.Interfaces;
using IMark.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMark.Areas.ViewModels
{
    public class DesignLabViewModel : BasePageViewModel
    {
        IApiService _apiService;
        INavigationService _navigationService;
        public DesignLabViewModel(IApiService apiService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public async Task SendImageToServer(string url, TentacledNode node)
        {
            await ShowLoading();
            var address = await App.Locator.CartPage.GetAddressInfor();
            if (address.Count == 0)
            {
                UserDialogs.Instance.Alert("No address found.please add first");
                return;
            }
            if (SettingExtension.CustomizeURLS == null)
                SettingExtension.CustomizeURLS = new List<string>();
            List<string> uri1 = new List<string>();
            uri1.Add(url);
            if(SettingExtension.CustomizeURLS != null)
            {
                foreach(var item in SettingExtension.CustomizeURLS)
                {
                    uri1.Add(item);
                }
            }
            SettingExtension.CustomizeURLS = uri1;
            try
            {
                StringBuilder builder = new StringBuilder();
                CheckoutRequestModel request = new CheckoutRequestModel();
                Inputs inputs = new Inputs();
                request.input = inputs;
                request.input.email = SettingExtension.UserEmail;
                if (SettingExtension.CustomizeURLS.Count == 0)
                {
                    request.input.note = "nothing for customize";
                }
                else
                {
                    foreach (var uri in SettingExtension.CustomizeURLS)
                    {
                        if (string.IsNullOrEmpty(builder.ToString()))
                        {
                            builder.Append(uri);
                        }
                        else
                        {
                            builder.Append(",");
                            builder.Append(uri);
                        }
                    }
                    request.input.note = builder.ToString();
                }
                request.input.lineItems = new List<Data.Models.Request.LineItem>();
                request.input.lineItems.Add(new Data.Models.Request.LineItem
                {
                    quantity = 1,
                    variantId = node.Id
                });
                
                List<Data.Models.Request.LineItem> data = new List<Data.Models.Request.LineItem>();
                data.Add(new Data.Models.Request.LineItem
                {
                    quantity = 1,
                    variantId = node.Id
                });
                
                if (SettingExtension.CartList != null)
                {
                    foreach (var item in SettingExtension.CartList)
                    {
                        request.input.lineItems.Add(new Data.Models.Request.LineItem
                        {
                            quantity = item.quantity,
                            variantId = item.variantId
                        });
                        data.Add(new Data.Models.Request.LineItem
                        {
                            quantity = item.quantity,
                            variantId = item.variantId
                        });
                    }
                }
                SettingExtension.CartList = data;
                Data.Models.Request.ShippingAddress shipping = new Data.Models.Request.ShippingAddress();
                request.input.shippingAddress = shipping;
                request.input.shippingAddress.firstName = address[0].node.firstName;
                request.input.shippingAddress.lastName = address[0].node.lastName;
                request.input.shippingAddress.address1 = address[0].node.address1;
                request.input.shippingAddress.address2 = address[0].node.address2;
                request.input.shippingAddress.city = address[0].node.city;
                request.input.shippingAddress.country = address[0].node.country;
                request.input.shippingAddress.zip = address[0].node.zip;
                request.input.shippingAddress.phone = address[0].node.phone;
                request.input.shippingAddress.province = address[0].node.province;
                request.input.shippingAddress.company = "Arknewtech";
                var queryId = @"mutation checkoutCreate($input: CheckoutCreateInput!) {
                          checkoutCreate(input: $input) {
                            checkout {
                              id
                            }
                            checkoutUserErrors {
                              code
                              field
                              message
                            }
                          }
                        }
                        ";
                var res1 = await _apiService.CheckOutData(queryId, request);
                if (string.IsNullOrEmpty(res1.data.checkoutCreate.checkout.id))
                {

                    await ShowAlert(res1.data.checkoutCreate.checkoutUserErrors[0].message);
                }
                else
                {
                    SettingExtension.CheckoutId = res1.data.checkoutCreate.checkout.id;
                    var queryCustomerAssociate = @"mutation checkoutCustomerAssociateV2($checkoutId: ID!, $customerAccessToken: String!) {
                              checkoutCustomerAssociateV2(checkoutId: $checkoutId, customerAccessToken: $customerAccessToken) {
                                checkout {
                                  id
                                }
                                checkoutUserErrors {
                                  code
                                  field
                                  message
                                }
                                customer {
                                  id
                                }
                              }
                            }";
                    CheckOutCustomerRequestModel checkOut = new CheckOutCustomerRequestModel();
                    checkOut.checkoutId = SettingExtension.CheckoutId;
                    checkOut.customerAccessToken = SettingExtension.AccessToken;
                    var checkOutAssociateResponse = await _apiService.CheckOutAssociate(queryCustomerAssociate, checkOut);
                    if (checkOutAssociateResponse.data.checkoutCustomerAssociateV2.checkout.id != null)
                    {
                        await App.Locator.CartPage.InitilizeData();
                        await App.Current.MainPage.Navigation.PushModalAsync(new CartPageBack());
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Internal Server Error");
            }
            await HideLoading();
        }
    }
}
