using eShopFlix.Support.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Support.HttpClients
{
    public class ProductServiceClient
    {
        HttpClient _client;
        IHttpContextAccessor _httpContextAccessor;
        public UserModel CurrentUser
        {
            get
            {
                string strData = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData).Value;
                return JsonSerializer.Deserialize<UserModel>(strData);
            }
        }
        
        public ProductServiceClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.Token);
            HttpResponseMessage response = await _client.GetAsync("product/getall");
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent != null)
                {
                    return JsonSerializer.Deserialize<IEnumerable<ProductModel>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            return null;
        }
    }
}
