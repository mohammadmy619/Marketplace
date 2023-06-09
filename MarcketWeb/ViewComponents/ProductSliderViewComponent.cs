using MarcketAppliction.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.ViewComponents
{
    public class ProductSliderViewComponent : ViewComponent
    {

        #region constructor

        private readonly IProductService _productService;

        public ProductSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region body

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var products = await _productService.GetCategoryProductsByCategoryName();

            ViewBag.title = products.Select(s => s.ProductSelectedCategories.Select(x=>x.ProductCategory).Select(s=>s.UrlName).SingleOrDefault()).FirstOrDefault();

            return View("ProductSlider", products);
        }

        #endregion
    }
}
