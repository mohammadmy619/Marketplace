using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceWeb.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult mohammad()
        {
            return View();
        }
    }
}
