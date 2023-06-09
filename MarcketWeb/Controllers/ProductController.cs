using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Products;
using MarketPlaceWeb.Http;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IUserServicecs _UserService;


        public ProductController(IProductService productService, IUserServicecs UserService)
        {
            _productService = productService;

            _UserService = UserService;
        }

        #endregion

        #region filter products

        [HttpGet("products")]
        [HttpGet("products/{Category}")]
        [HttpGet("products/{Category}/{Serach}")]

        public async Task<IActionResult> FilterProducts(FilterProductDTO filter)
        {
            filter.TakeEntity = 9;
            filter.FilterProductState = FilterProductState.Accepted;
            filter = await _productService.FilterProducts(filter);
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            if (filter.PageId > filter.GetLastPage() && filter.GetLastPage() != 0) return NotFound();
            return View(filter);
        }

        #endregion

        #region show product detail

        //[HttpGet("products/{productId}/{title}")]
        [Route("ProductDetail/{productId}/{title}")]

        public async Task<IActionResult> ProductDetail(long productId, string title)
        {
            var product = await _productService.GetProductDetailById(productId);
            if (product == null) return NotFound();
            ViewBag.GetAllbestSells = _productService.GetAllbestSells(12);
            return View(product);
        }

        #endregion

        #region CreateComment
        [HttpPost]
        public async Task<IActionResult> CreateComment(long ProductID, string Massege)
        {
            var comment = new CreateProductComment
            {
                Massege = Massege,
                ProductID = ProductID,
                UserID = User.GetUserId()
            };

            var result = await _productService.CraeteCommentProduct(comment);




            switch (result)
            {
                case CraeteComment.NotForUser:

                    //TempData[WarningMessage] = " فقط کاربرانی که خرید کرده اند میتوانند نظر  دهند";

                    return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Warning,
                    "فقط کاربرانی که خرید کرده اند میتوانند نظر  دهند",
                    null);


                case CraeteComment.Success:

                    return ViewComponent("ShowCommentForProducts", ProductID);

                case CraeteComment.Error:
                    return JsonResponseStatus.SendStatus(
                     JsonResponseStatusType.Danger,
                     "خطا در تبت نطر",
                     null);
            }


            return JsonResponseStatus.SendStatus(
                     JsonResponseStatusType.Danger,
                     "خطا در تبت نطر",
                     null);

        }
        #endregion


        #region ProductsFavorite

        [HttpGet("add-favorite")]

        public async Task<IActionResult> Favorite(long productid, string ReturnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData[WarningMessage] = "لطفا وارد حساب کاربری خود شوید";
                return Redirect(ReturnUrl);
            }

            var result = await _UserService.AddUserFavorite(productid, User.GetUserId());

            if (result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر با موفقیت در قسمت علاقه مندی اضافه شد";
                return Redirect(ReturnUrl);



            }

            TempData[WarningMessage] = "محصول مورد نظر قبلا در علاقه مندی اضافه شده است";
            return Redirect(ReturnUrl);
        }

        #endregion
    }
}
