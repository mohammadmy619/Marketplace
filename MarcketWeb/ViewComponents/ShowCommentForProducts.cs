using MarcketAppliction.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.ViewComponents
{
    public class ShowCommentForProducts: ViewComponent
    {

        private readonly IProductService _productService;

        public ShowCommentForProducts(IProductService productService)
        {
            _productService = productService;
        }

        #region body

        public async Task<IViewComponentResult> InvokeAsync(long ProductID)
        {

            var comment = await _productService.ShowComment(ProductID);
            ViewBag.ProductID = ProductID;
            ViewData["comment"] = comment.Count().ToString();
            ViewBag.comment= comment.Count().ToString();
         
            return View("ShowComments", comment);
        }

        #endregion
    }
}
