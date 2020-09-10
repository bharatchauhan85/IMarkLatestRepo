using GraphQL;
using IMark.Data.Models;
using IMark.Service.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMark.Service.Services
{
   public class BaseApiServices
    {
        public Task<ServiceResponse<T>> GetAsync<T>()
        {
            return null;
        }
       
        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> keyValuePairs)
        {
            var newurl = url + GetQueryString(keyValuePairs);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(resStr);
                return data;
            }
        }
        public async Task<T> GetAsyncWithToken<T>(string url, Dictionary<string, string> keyValuePairs, string token)
        {
            var newurl = url + GetQueryString(keyValuePairs);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                var response = await client.GetAsync(newurl);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(resStr);
                return data;
            }
        }
        public async Task<ServiceResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest requestModel)
        {
            using (var client = new HttpClient())
            {
                var reqJson = JsonConvert.SerializeObject(requestModel);
                var response = await client.GetAsync(url);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ServiceResponse<TResponse>>(resStr);
                return data;
            }
        }

        public async Task<TResponse> PostAsyncD<TRequest, TResponse>(string url, TRequest requestModel)
        {
            using (var client = new HttpClient())
            {
                var reqJson = JsonConvert.SerializeObject(requestModel);
                var httpcontent = new StringContent(reqJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, httpcontent);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<TResponse>(resStr);
                return data;
            }
        }

        public async Task<TResponse> PostAsyncWithToken<TRequest, TResponse>(string url, TRequest requestModel, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", token);
                var reqJson = JsonConvert.SerializeObject(requestModel);
                var httpcontent = new StringContent(reqJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, httpcontent);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<TResponse>(resStr);
                return data;
            }
        }


        public async Task<TResponse> PostAsyncFormData<TResponse>(string url, Dictionary<string, string> param)
        {
            using (var client = new HttpClient())
            {

                var httpcontent = new FormUrlEncodedContent(param);
                var response = await client.PostAsync(url, httpcontent);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<TResponse>(resStr);
                return data;
            }
        }
        public async Task<TResponse> PostAsyncMultipartFormData<TResponse>(string url, byte[] fileBytes,string FileName, Dictionary<string, string> param)
        {
            using (var client = new HttpClient())
            {
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var item in param)
                    {
                        content.Add(new StringContent(item.Value), item.Key);
                    }
                    content.Add(fileContent, "resume" ,FileName);
                    var response = await client.PostAsync(url, content);
                    var resStr = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Devmessage:" + resStr);
                    var data = JsonConvert.DeserializeObject<TResponse>(resStr);
                    return data;
                }
            }
        }
        public async Task<T> PostFormData<T>(string postUrl, Dictionary<string, string> keyValuePairs)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                // form.Headers.Add("Content-Type", "multipart/form-data; boundary=--------------------------519261285821980103256135");
                foreach (var item in keyValuePairs)
                {
                    form.Add(new StringContent(item.Key), item.Value);
                }

                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(postUrl));
                request.Headers.Add("Content-Type", "multipart/form-data; boundary=--------------------------519261285821980103256135");
                request.Content = form;
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }


        public string GetQueryString(Dictionary<string, string> keyValuePairs)
        {
            return string.Join("&", keyValuePairs.Select((key, val) => $"{key}={val}"));
        }

        public async Task<TResponse> PostAsynGraphql<TRequest, TResponse>(string query, TRequest requestModel)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseCookies = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            var request = new GraphQLRequest()
            {
                Query = query,
                Variables = requestModel
            };
            httpClient.DefaultRequestHeaders.Add("X-Shopify-Storefront-Access-Token", "44cbf60d77b0f71b89f47c113b381393");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var reqJson = JsonConvert.SerializeObject(request);
            var httpcontent = new StringContent(reqJson, Encoding.UTF8, "application/json");
            var graphQLResponse = await httpClient.PostAsync("https://imark-apparel.myshopify.com/api/2019-07/graphql.json", httpcontent);
            var json = await graphQLResponse.Content.ReadAsStringAsync();
            var graphResult = JsonConvert.DeserializeObject<TResponse>(json);
            return graphResult;
        }
        public async Task<TResponse> PostAsynGraphqlWithOutRequest<TResponse>(string query)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseCookies = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            var request = new GraphQLRequest()
            {
                Query = query,
            };
            httpClient.DefaultRequestHeaders.Add("X-Shopify-Storefront-Access-Token", "44cbf60d77b0f71b89f47c113b381393");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var reqJson = JsonConvert.SerializeObject(request);
            var httpcontent = new StringContent(reqJson, Encoding.UTF8, "application/json");
            var graphQLResponse = await httpClient.PostAsync(Urls.IMARK_Product, httpcontent);
            var json = await graphQLResponse.Content.ReadAsStringAsync();
            var graphResult = JsonConvert.DeserializeObject<TResponse>(json);
            return graphResult;
        }


        public async Task<T> GetAsyncWithUserPassword<T>(string url, string userName, string passwd)
        {
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            using (var client = new HttpClient())
            {
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(userName + ":" + passwd));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + svcCredentials);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
                var response = await client.GetAsync(url);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(resStr);
                return data;
            }
        }
        public async Task<T> GetAsyncWithUser<T>(string url, string userName, string passwd)
        {
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            using (var client = new HttpClient())
            {
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(userName + ":" + passwd));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + svcCredentials);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
                var response = await client.GetAsync(url);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(resStr);
                return data;
            }
        }

        public async Task<T> GetAsyncWithUserData<T>(string url)
        {
            
            using (var client = new HttpClient())
            {
                
                var response = await client.GetAsync(url);
                var resStr = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(resStr);
                return data;
            }
        }
        //public virtual async Task<Result<T>> GetJsonAsync<T>(string url)
        //{
        //    //await SetTokenAsync();
        //    await SetHeaderAsync();
        //    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var result = await _client.GetAsync(url);
        //    return await HandleResponseAsync<T>(result);
        //}


        public async Task<TResponse> PostAsynWithOutvariable<TResponse>(string query)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.UseCookies = false;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            var request = new GraphQLRequest()
            {
                Query = query
            };
            httpClient.DefaultRequestHeaders.Add("X-Shopify-Storefront-Access-Token", "fec912ae8f1051ce097be6130450d901");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var reqJson = JsonConvert.SerializeObject(request);
            var httpcontent = new StringContent(reqJson, Encoding.UTF8, "application/json");
            var graphQLResponse = await httpClient.PostAsync("https://sportdirect-ca-dev.myshopify.com/api/2019-07/graphql.json", httpcontent);
            var json = await graphQLResponse.Content.ReadAsStringAsync();
            var graphResult = JsonConvert.DeserializeObject<TResponse>(json);
            return graphResult;
        }


    }
}
