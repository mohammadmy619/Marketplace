using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MarketPlaceWeb.Http;

namespace MarketPlaceWeb.Areas.Seller.Controllers
{
    [Authorize]

    [Area("Seller")]
    [Route("seller")]
    [CheckSellerState]
    public class SellerBaseController : Controller {


        protected string ErrorMessage = "ErrorMessage";
        protected string WarningMessage = "WarningMessage";
        protected string SuccessMessage = "SuccessMessage";

        protected string InfoMessage = "InfoMessage";
    }
}
