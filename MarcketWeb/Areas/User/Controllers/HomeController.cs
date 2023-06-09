using MarcketAppliction.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        private readonly IUserServicecs _userService;
        public HomeController(IUserServicecs userServicecs)
        {
            _userService = userServicecs;
        }
        #region user dashboard

        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = await _userService.GetUserByEmail(User.Identity.Name);
            }
            return View();
        }

        #endregion
    }
}
