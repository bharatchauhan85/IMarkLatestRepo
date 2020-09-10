using IMark.Data.Models.Request;
using IMark.Data.Models.Response;
using IMark.Service.Helpers;
using IMark.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
namespace IMark.Service.Services
{
   public class ApiService : BaseApiServices, IApiService
    {
        public async Task<GetProductResponse> GetProduct(string username, string UserPassword, string productType)
        {
            var res = await GetAsyncWithUserPassword<GetProductResponse>(string.Format(Urls.GetProduct, productType), username, UserPassword);
            return res;
        }
        public async Task<GraphqlResponse> CreateCusmerbyGraphql(string query, CreateCusomerRequest obj_create)
        {
            var res = await PostAsynGraphql<CreateCusomerRequest, GraphqlResponse>(query, obj_create);
            return res;
        }

        public async Task<CustomerInfoResponse> CustomerInfo(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<CustomerInfoResponse>(query);
            return res;
        }

        public async Task<CustLoginResponse> CustomerLogin(string query, LoginRequest obj_create)
        {
            var res = await PostAsynGraphql<LoginRequest, CustLoginResponse>(query, obj_create);
            return res;
        }
        public async Task<DashboardResponseModel> GetDashboardAync()
        {
             var dict = new Dictionary<string, string>();
             var res = await GetAsync<DashboardResponseModel>(Urls.DashboardUrl,dict);
             return res;
         }
        public async Task<SaveResumeResponseModel> SaveResumeAync(SaveResumeRequestModel saveResumeRequestModel)        {            
            var dict = new Dictionary<string, string>();
            dict.Add("jobId",saveResumeRequestModel.jobId);
            dict.Add("name", saveResumeRequestModel.name);
            dict.Add("email", saveResumeRequestModel.email);
            dict.Add("phone", saveResumeRequestModel.phone);
            dict.Add("description", saveResumeRequestModel.description);
            var res = await PostAsyncMultipartFormData<SaveResumeResponseModel>(Urls.SaveResumeUrl,saveResumeRequestModel.resume,saveResumeRequestModel.FileName ,dict);
            return res;
        }

        public async Task<GetProductResponse> GetProductType(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<GetProductResponse>(query);
            return res;
        }

        public async Task<OrderResponse> GetOrders(string username, string UserPassword)
        {
            var res = await GetAsyncWithUser<OrderResponse>(Urls.GetOrders, username,UserPassword);
            return res;
        }

        public async Task<CreateCustomerResponse> CreateAddress(string query, CreateAddressReequest createAddressReequest)
        {
            var res = await PostAsynGraphql<CreateAddressReequest, CreateCustomerResponse>(query, createAddressReequest);
            return res;
        }

        public async Task<UpdateAddressResponse> UpdateAddress(string query, UpdateAddressRequest updateAddressRequest)
        {
            var res = await PostAsynGraphql<UpdateAddressRequest, UpdateAddressResponse>(query, updateAddressRequest);
            return res;
        }


        public async Task<CustomerDetailAddressResponse> GetAddress(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<CustomerDetailAddressResponse>(query);
            return res;
        }

        public async Task<CustomerDetailAddressResponse> GetCustomer(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<CustomerDetailAddressResponse>(query);
            return res;
        }

        public async Task<CustomerDeleteAddressResponse> DeleteAddress(string query, DeleteCustomerRequest deleteCusRequest)
        {
            var res = await PostAsynGraphql<DeleteCustomerRequest, CustomerDeleteAddressResponse>(query, deleteCusRequest);
            return res;
        }

        public async Task<ForgetModelResponse> ForgetPassword(string query, ForgetModelRequest forgetModelRequest)
        {
            var res = await PostAsynGraphql<ForgetModelRequest, ForgetModelResponse>(query, forgetModelRequest);
            return res;
        }

        public async Task<CheckoutResponseModel> CheckOutData(string query, CheckoutRequestModel checkoutRequest)
        {
            var res = await PostAsynGraphql<CheckoutRequestModel, CheckoutResponseModel>(query, checkoutRequest);
            return res;
        }

        public async Task<LineItemResponse> LineItemData(string query, LineItemRequest lineItemRequest)
        {
            var res = await PostAsynGraphql<LineItemRequest, LineItemResponse>(query, lineItemRequest);
            return res;
        }

        public async Task<CheckOutCustomerResponseModel> CheckOutAssociate(string query, CheckOutCustomerRequestModel checkoutRequest)
        {
            var res = await PostAsynGraphql<CheckOutCustomerRequestModel, CheckOutCustomerResponseModel>(query, checkoutRequest);
            return res;
        }

        public async Task<CartUpdateResponseModel> UpdateCart(string query, CartUpdateRequestModel request)
        {
            var res = await PostAsynGraphql<CartUpdateRequestModel, CartUpdateResponseModel>(query, request);
            return res;
        }

        public async Task<CartUpdateResponseModel> DeleteCart(string query, CartDeleteRequestModel request)
        {
            var res = await PostAsynGraphql<CartDeleteRequestModel, CartUpdateResponseModel>(query, request);
            return res;
        }

        public async Task<CollectionResponseModel> GetCollectionType(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<CollectionResponseModel>(query);
            return res;
        }

        public async Task<CollectionListResponseModel> GetCollectionList(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<CollectionListResponseModel>(query);
            return res;
        }

        public async Task<GetProductResponse> SortListOfProduct(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<GetProductResponse>(query);
            return res;
        }

        public async Task<GetProductResponse> SortListOfCollection(string query)
        {
            var res = await PostAsynGraphqlWithOutRequest<GetProductResponse>(query);
            return res;
        }

        public async Task<UpdateAddressForShippingResponseModel> UpdateShippingAddress(string query,UpdateAddressForShippingRequestModel request)
        {
            var res = await PostAsynGraphql<UpdateAddressForShippingRequestModel, UpdateAddressForShippingResponseModel>(query, request);
            return res;
        }
        public async Task<UpdateAddressForShippingResponseModel> CustomerUpdate(string query, CustomerUpdateRequest request)
        {
            var res = await PostAsynGraphql<CustomerUpdateRequest, UpdateAddressForShippingResponseModel>(query, request);
            return res;
        }
    }
}