using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Common;
using MarcketDataLayer.DTOs.Seller;
using MarketPlaceWeb.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    
    public class SellerController : AdminBaseController
    {

        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region seller requests

        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            return View(await _sellerService.FilterSellers(filter));
        }

        #endregion


        #region accept seller request

        public async Task<IActionResult> AcceptSellerRequest(long requestId)
        {
            var result = await _sellerService.AcceptSellerRequest(requestId);

            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نظر با موفقیت تایید شد",
                    null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion

        #region reject seller request

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerRequest(RejectItemDTO reject)
        {
            var result = await _sellerService.RejectSellerRequest(reject);

            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نظر با موفقیت رد شد شد",
                    reject);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion
    }
}
