using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Checkout.Api.Model;
using Newtonsoft.Json;

namespace Checkout.Client
{
    public class ApiClient
    {
        private HttpClient _httpClient;
        private const string BasketUrl = "/api/baskets";
        private const string ItemUrl = "/api/items";
        public ApiClient(Uri baseAddress){
            _httpClient = new HttpClient{
                BaseAddress = baseAddress
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Web API Client");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        #region Items

        public async Task<IEnumerable<ItemResponse>> GetItems()
        {
            var response = await _httpClient.GetStringAsync(ItemUrl);
            return JsonConvert.DeserializeObject<IEnumerable<ItemResponse>>(response);
        }

        public async Task<ItemResponse> GetItem(int id)
        {
            var response = await _httpClient.GetStringAsync($"{ItemUrl}/{id}");
            return JsonConvert.DeserializeObject<ItemResponse>(response);
        }

        public async Task<ItemResponse> AddItem(ItemResponse item)
        {
            try
            {
                var response = await _httpClient.PostAsync(ItemUrl, new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<ItemResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ItemUrl}/{id}");
                return  response.IsSuccessStatusCode ? true : false;

            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
        #endregion
        
        #region Basket

        public async Task<IEnumerable<BasketResponse>> GetBaskets()
        {
            var response = await _httpClient.GetStringAsync(BasketUrl);
            return JsonConvert.DeserializeObject<IEnumerable<BasketResponse>>(response);
        }

        public async Task<BasketResponse> GetBasket(int id)
        {
            var response = await _httpClient.GetStringAsync($"{BasketUrl}/{id}");
            return JsonConvert.DeserializeObject<BasketResponse>(response);
        }

        public async Task<BasketItemResponse> AddBasket(BasketResponse basket)
        {
            try
            {
                var response = await _httpClient.PostAsync(BasketUrl, new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<BasketItemResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> DeleteBasket(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BasketUrl}/{id}");
                return  response.IsSuccessStatusCode ? true : false;

            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<int> CreateEmptyBasket(){
            try
            {
                var response = await _httpClient.PostAsync($"{BasketUrl}/create", null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return -1;
            }
        }

        public async Task<decimal> CheckoutBasket(int id){
            try
            {
                var response = await _httpClient.PostAsync($"{BasketUrl}/{id}/checkout", null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return -1;
            }
        }

        public async Task<decimal> ClearBasket(int id){
            try
            {
                var response = await _httpClient.PostAsync($"{BasketUrl}/{id}/clear", null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return 0;
            }
        }

        public async Task<bool> AddOrUpdateItemToBasket(int id, int itemId, int count){
            try
            {
                var response = await _httpClient.PostAsync($"{BasketUrl}/{id}/add/{itemId}/{count}", null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveItemFromBasket(int id, int itemId){
            try
            {
                var response = await _httpClient.PostAsync($"{BasketUrl}/{id}/remove/{itemId}", null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidOperationException(response.ReasonPhrase);

                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<decimal> BasketPrice(int id){
            try
            {
                var response = await _httpClient.GetStringAsync($"{BasketUrl}/{id}/price");

                return decimal.Parse(response);
            }
            catch (HttpRequestException)
            {
                return 0m;
            }
        }
        #endregion
    }
}
