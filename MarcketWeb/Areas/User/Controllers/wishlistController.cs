using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Account;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    public class wishlistController : UserBaseController
    {

        private readonly IUserServicecs _userService;
        public wishlistController(IUserServicecs userServicecs)
        {
            _userService = userServicecs;
        }
        [HttpGet("Favorite")]
        public async Task<IActionResult> Index(UserFavoritsDTO userFavorits)
        {
            userFavorits.TakeEntity = 5;

            return View(await _userService.UserFavorits(userFavorits,User.GetUserId()));
        }
        [HttpGet("DeleteFavorite{productid}")]
        public async Task<IActionResult> Delete(long productid)
        {
            var DeleteFavorite = await _userService.DeleteFavorite(productid, User.GetUserId());
            if(DeleteFavorite == true)
            {
                TempData[SuccessMessage] = "با موفقیت حذف شد";
                return RedirectToAction("Index");

            }

            TempData[WarningMessage] = "خطا";

            return RedirectToAction("Index");
        }
    }
}
