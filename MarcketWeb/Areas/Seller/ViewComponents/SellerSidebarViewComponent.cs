using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceWeb.Areas.Seller.ViewComponents
{
    public class SellerSidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SellerSidebar");
        }
    }
}
