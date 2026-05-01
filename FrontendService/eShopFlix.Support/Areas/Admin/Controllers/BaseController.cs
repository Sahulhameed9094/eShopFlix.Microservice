using eShopFlix.Support.Helpers;
using eShopFlix.Support.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Support.Areas.Admin.Controllers
{

    [CustomAuthorize(Roles = "Admin")]
    [Area("Admin")]
    public class BaseController : Controller
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
