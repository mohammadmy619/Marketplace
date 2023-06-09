using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Seller;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region request seller

        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }

        [HttpPost("request-seller-panel"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO seller)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.AddNewSellerRequest(seller, User.GetUserId());
                switch (res)
                {
                    case RequestSellerResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما دسترسی لازم جهت انجام فرایند مورد نظر را ندارید";
                        break;
                    case RequestSellerResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "درخواست های قبلی شما در حال پیگیری می باشند";
                        TempData[InfoMessage] = "در حال حاضر نمیتوانید درخواست جدیدی ثبت کنید";
                        break;
                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات شما در حال پیگیری می باشد";
                        return RedirectToAction("SellerRequests");
                }
            }

            return View(seller);
        }

        #endregion

        #region seller requests

        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.TakeEntity = 1;
            filter.UserId = User.GetUserId();
            filter.State = FilterSellerState.All;

            return View(await _sellerService.FilterSellers(filter));
        }

        #endregion
        #region edit request

        [HttpGet("edit-request-seller/{id}")]
        public async Task<IActionResult> EditRequestSeller(long id)
        {
            var requestSeller = await _sellerService.GetRequestSellerForEdit(id, User.GetUserId());
            if (requestSeller == null) return NotFound();
            return View(requestSeller);
        }

        [HttpPost("edit-request-seller/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequestSeller(EditRequestSellerDTO request)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.EditRequestSeller(request, User.GetUserId());
                switch (res)
                {
                    case EditRequestSellerResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditRequestSellerResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ویرایش شد";
                        TempData[InfoMessage] = "فرآیند تایید اطلاعات از سر گرفته شد";
                        return RedirectToAction("SellerRequests");
                }
            }

            return View(request);
        }

        #endregion

    }
}
