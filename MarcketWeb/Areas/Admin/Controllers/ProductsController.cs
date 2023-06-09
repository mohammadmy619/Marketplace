using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Common;
using MarcketDataLayer.DTOs.Products;
using MarketPlaceWeb.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {

        #region constructor

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        #region filter products

        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            filter.TakeEntity = 10;
            return View(await _productService.FilterProducts(filter));
        }

        #endregion
        #region accept product

        public async Task<IActionResult> AcceptSellerProduct(long id)
        {
            var result = await _productService.AcceptSellerProduct(id);
            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت تایید شد", null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "محصول مورد نظر یافت نشد", null);
        }

        #endregion

        #region reject product

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectProduct(RejectItemDTO reject)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.RejectSellerProduct(reject);

                if (result)
                {

                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success,
                        "محصول مورد نظر با موفقیت رد شد", reject);
                }

                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "اطلاعات مورد نظر جهت عدم تایید را به درستی وارد نمایید", null);
            }


            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "محصول مورد نظر یافت نشد", null);
        }

        #endregion
    }
}
