using eShopFlix.Web.Models;
using System.Text;
using System.Text.Json;

namespace eShopFlix.Web.HttpClients
{
    public class AuthServiceClient
    {
        HttpClient _client;
        public AuthServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserModel> LoginAsync(LoginModel model)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent != null)
                {
                    return JsonSerializer.Deserialize<UserModel>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            return null;
        }

        public async Task<bool> SignUpAsync(SignUpModel model)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("auth/SignUp", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
