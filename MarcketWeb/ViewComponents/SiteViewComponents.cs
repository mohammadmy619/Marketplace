using System.Threading.Tasks;
using MarcketAppliction.Services.Interface;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceWeb.ViewComponents
{
    #region site header

    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IUserServicecs _userService;
        private readonly IProductService _productService;
        public SiteHeaderViewComponent(ISiteService siteService, IUserServicecs userServicecs,IProductService productService)
        {
            _siteService = siteService;
            _userService = userServicecs;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();
            ViewBag.user = await _userService.GetUserByEmail(User.Identity.Name);
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = await _userService.GetUserByEmail(User.Identity.Name);

             
            }

            return View("SiteHeader");
        }

    }

    #endregion

    #region site footer

    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();

            return View("SiteFooter");
        }
    }

    #endregion

    #region home sliders

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteService.GetAllActiveSliders();
            return View("HomeSlider", sliders);
        }
    }

    #endregion


    #region user order

    public class UserOrderViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;

        private readonly IUserServicecs _userService;

        public UserOrderViewComponent(IOrderService orderService, IUserServicecs userServicecs)
        {
            _orderService = orderService;
            _userService = userServicecs;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
         
            // var openOrder = await _orderService.GetUserLatestOpenOrder(User.GetUserId());
            var openOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            if (User.Identity.IsAuthenticated)
            {


                ViewBag.FavoritCount = await _userService.UserFavoritCount(User.GetUserId());

            }

            return View("UserOrder", openOrder);
        }
    }

    #endregion
}
