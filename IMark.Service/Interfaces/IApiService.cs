using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMark.Service.Interfaces
{
   public interface IApiService
    {
        Task<DashboardResponseModel> GetDashboardAync();
        Task<SaveResumeResponseModel> SaveResumeAync(SaveResumeRequestModel saveResumeRequestModel);
        Task<GraphqlResponse> CreateCusmerbyGraphql(string query, CreateCusomerRequest obj_create);
        Task<CustLoginResponse> CustomerLogin(string query, LoginRequest obj_create);
        Task<CustomerInfoResponse> CustomerInfo(string query);
        Task<GetProductResponse> GetProduct(string username, string UserPassword, string product_type);
        Task<GetProductResponse> GetProductType(string query);
        Task<CollectionResponseModel> GetCollectionType(string query);
        Task<CollectionListResponseModel> GetCollectionList(string query);
        Task<OrderResponse> GetOrders(string username, string UserPassword);
        Task<CreateCustomerResponse> CreateAddress(string query, CreateAddressReequest createAddressReequest);
        Task<UpdateAddressResponse> UpdateAddress(string query, UpdateAddressRequest updateAddressRequest);
        Task<CustomerDetailAddressResponse> GetAddress(string query);
        Task<CheckoutResponseModel> CheckOutData(string query, CheckoutRequestModel checkoutRequest);
        Task<CheckOutCustomerResponseModel> CheckOutAssociate(string query, CheckOutCustomerRequestModel checkoutRequest);

        Task<LineItemResponse> LineItemData(string query, LineItemRequest lineItemRequest);
        Task<CustomerDetailAddressResponse> GetCustomer(string query);
        Task<CustomerDeleteAddressResponse> DeleteAddress(string query, DeleteCustomerRequest deleteCusRequest);
        Task<ForgetModelResponse> ForgetPassword(string query, ForgetModelRequest obj_create);
        Task<CartUpdateResponseModel> UpdateCart(string query, CartUpdateRequestModel request);
        Task<CartUpdateResponseModel> DeleteCart(string query, CartDeleteRequestModel request);

        Task<GetProductResponse> SortListOfProduct(string query);
        Task<GetProductResponse> SortListOfCollection(string query);
        Task<UpdateAddressForShippingResponseModel> UpdateShippingAddress(string query, UpdateAddressForShippingRequestModel request);
        Task<CustomerUpdateResponse> CustomerUpdate(string query, CustomerUpdateRequest request);
        //   Task<CollectionResponse> GetCollection(string query);
        //Task<OrderResponse> GetOrders(, string UserPassword);
    }
}
