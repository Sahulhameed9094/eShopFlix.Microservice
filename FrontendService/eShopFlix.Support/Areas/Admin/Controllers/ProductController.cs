using eShopFlix.Support.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace eShopFlix.Support.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        ProductServiceClient _productServiceClient;
        public ProductController(ProductServiceClient productServiceClient)
        {
            _productServiceClient = productServiceClient;
        }
        public IActionResult Index()
        {
            var products = _productServiceClient.GetProducts().Result;
            return View(products);
        }
    }
}
