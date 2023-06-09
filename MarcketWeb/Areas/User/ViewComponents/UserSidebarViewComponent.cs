using MarcketAppliction.Services.Interface;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.ViewComponents
{
    public class UserSidebarViewComponent : ViewComponent
    {
        private ISellerService _sellerService;

        public UserSidebarViewComponent(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.hasUserAnyActiveSellerPanel = await _sellerService.HasUserAnyActiveSellerPanel(User.GetUserId());

            return View("UserSidebar");
        }

    }
}
