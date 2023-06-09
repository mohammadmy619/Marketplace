using GoogleReCaptcha.V3.Interface;
using MarcketAppliction.Services.Interface;
using MarcketAppliction.Utils;
using MarcketDataLayer.Entities.Contacts;
using MarcketDataLayer.Entities.Site;
using MarketPlaceWeb.Models;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IProductService _productService;
        private readonly ISiteService _siteService;
        private  IArticlesService _ArticlesService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IContactService contactService, ICaptchaValidator captchaValidator, ISiteService siteService,IProductService productService, IArticlesService ArticlesService)
        {
            _logger = logger;
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _siteService = siteService;
            _productService = productService;
            _ArticlesService = ArticlesService;
        }
        #endregion


      
       
        public async Task<IActionResult> Index()
        {
            //await Task.Delay(3000);

            var Product = _productService.GetallproductsforIndex();

            ViewBag.banners = await _siteService
                .GetSiteBannersByPlacement(new List<BannerPlacement>
                {
                    BannerPlacement.Home_1,
                    BannerPlacement.Home_2,
                    BannerPlacement.Home_3
                });
            ViewData["OffProducts"] = await _productService.GetAllOffProducts(12);
            ViewBag.GetAllbestSells = Product;

            //SendEmail.Send("mohammadmy619@gmail.com", "فعالسازی", "<h1>mohammad</h1>");

            ViewBag.GetallPopularproducts = Product;

            ViewBag.GetArticlesGroups =  _ArticlesService.GetArticlesGroups();

          
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region contact us

        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            ViewBag.GetSiteSeting =await _siteService.GetDefaultSiteSetting();
            return  View();
        }

        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO contact)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contact.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();
                await _contactService.CreateContactUs(contact, HttpContext.GetUserIp(), User.GetUserId());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("ContactUs");
            }

            return View(contact);
        }

        #endregion

        #region about us

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var siteSetting = await _siteService.GetDefaultSiteSetting();
            return View(siteSetting);
        }

        #endregion
        #region
        public IActionResult ErrorPage()
        {
            return View();
        }
        #endregion
    }
}
