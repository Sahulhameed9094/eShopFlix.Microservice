using eShopFlix.Support.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Support.Helpers
{
    public abstract class BaseViewPage<TModel> : RazorPage<TModel>
    {
        public UserModel CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    string strData = User.FindFirst(ClaimTypes.UserData).Value;
                    if (strData != null)
                    {
                        return JsonSerializer.Deserialize<UserModel>(strData);
                    }
                }
                return null;
            }
        }
    }
}
