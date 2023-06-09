using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Discount;
using MarcketDataLayer.DTOs.ProductDiscount;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Seller.Controllers
{
  
        public class ProductDiscountController : SellerBaseController
        {
            #region constructor

            private readonly IProductDiscountService _productDiscountService;
            private readonly ISellerService _sellerService;

            public ProductDiscountController(IProductDiscountService productDiscountService, ISellerService sellerService)
            {
                _productDiscountService = productDiscountService;
                _sellerService = sellerService;
            }

            #endregion

            #region filter discount

            [HttpGet("discounts")]
            [HttpGet("discounts/{ProductId}")]
            public async Task<IActionResult> FilterDiscounts(FilterProductDiscountDTO filter)
            {
                //if (filter.ProductId == null || filter.ProductId == 0) return NotFound();
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                filter.SellerId = seller.Id;
                return View(await _productDiscountService.FilterProductDiscount(filter));
            }

            #endregion

            #region create discount

            [HttpGet("create-discount")]
            public IActionResult CreateDiscount()
            {
                return View();
            }

            [HttpPost("create-discount"), ValidateAntiForgeryToken]
            public async Task<IActionResult> CreateDiscount(CreateProductDiscountDto discount)
            {
                if (ModelState.IsValid)
                {
                    var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                    var res = await _productDiscountService.CreateProductDiscount(discount, seller.Id);
                    switch (res)
                    {
                        case CreateDiscountResult.Error:
                            TempData[ErrorMessage] = "عملیات ثبت تخفیف مورد نظر با شکست مواجه شد";
                            break;
                        case CreateDiscountResult.ProductNotFound:
                            TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                            break;
                        case CreateDiscountResult.ProductIsNotForSeller:
                            TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                            break;
                        case CreateDiscountResult.Success:
                            TempData[SuccessMessage] = "عملیات ثبت تخفیف برای محصول مورد نظر با موفقیت انجام شد";
                            return RedirectToAction("FilterDiscounts");
                    }
                }

                return View(discount);
            }

            #endregion
        }
    }
