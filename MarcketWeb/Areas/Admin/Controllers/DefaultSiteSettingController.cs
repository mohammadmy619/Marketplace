using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Slider;
using MarcketDataLayer.Entities.Site;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class DefaultSiteSettingController : AdminBaseController
    {
        private ISiteService _SiteService;
        public DefaultSiteSettingController(ISiteService SiteService)
        {
            _SiteService = SiteService;
        }
        public async Task<IActionResult> SiteSetting()
        {
            return View(await _SiteService.GetDefaultSiteSetting());
        }
        [HttpPost]
        public async Task<IActionResult> SiteSetting(SiteSetting site,IFormFile Logo)
        {
            var SiteSetting = await _SiteService.EditDefaultSiteSetting(site, Logo);


            if (SiteSetting == false) return View();

            TempData[SuccessMessage] = "باموفقیت ویرایش  شد";

            return View(await _SiteService.GetDefaultSiteSetting());
        }
        public async Task<IActionResult> GetAllSliders()
        {
            return View(await _SiteService.GetAllActiveSliders());
        }
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSlider(CraeteSliderDto dto, IFormFile Img)
        {
            var res = await _SiteService.CreateSlider(dto, Img);
            switch (res)
            {
                case CreateSliderResult.error:
                    TempData[ErrorMessage] = "خطا";
                    return View();


                case CreateSliderResult.HasNoImage:
                    TempData[WarningMessage] = "اطلاعات خواسته شده را وارد کنید";
                    return View();

                case CreateSliderResult.Sucsses:
                    TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                    break;
            }
            return RedirectToAction("GetAllSliders");
        }
        [HttpGet("GetSliderforEdit")]
        public async Task<IActionResult> GetSliderforEdit(long id)
        {
            var getSlider =await _SiteService.GetSliderforEdit(id);

            return View(getSlider);
        }
        [HttpPost("GetSliderforEdit")]
        public async Task<IActionResult> GetSliderforEdit(EditSliderDto edit, IFormFile img)
        {
            var res = await _SiteService.EditSliderResults(edit, img);
            switch (res)
            {
                case EditSliderResult.error:
                    TempData[ErrorMessage] = "خطا";
                    return View();


                case EditSliderResult.HasNoImage:
                    TempData[WarningMessage] = "اطلاعات خواسته شده را وارد کنید";
                    return View();

                case EditSliderResult.Sucsses:
                    TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت  ویرایش شد";
                    break;
            }
            return RedirectToAction("GetAllSliders");
        }
        public async Task<IActionResult> Delete(long id)
        {
            var res = await _SiteService.DeleteSlider(id);
            if (res == false)
            {
                TempData[ErrorMessage] = "خطا";

            }
            TempData[SuccessMessage] = "اسلایدر مورد نظر با موفقیت حذف شد";

            return RedirectToAction("GetAllSliders");
        }
        public async Task<IActionResult> Banner()
        {
            var banner = await _SiteService.GetSiteBannersByPlacement(new List<BannerPlacement> {

                    BannerPlacement.Home_1,
                    BannerPlacement.Home_2,
                    BannerPlacement.Home_3
            });
               
            return View(banner);
        }

        [HttpGet]

        public IActionResult CreateBanner(long BPID)
        {
            ViewBag.BPID = BPID;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBanner(long Bpid, string Url, IFormFile img)
        {
            ViewBag.BPID = Bpid;
            var res = await _SiteService.CreateBanner(Bpid, Url, img);

            if(res==true)
            {
                TempData[SuccessMessage] = "بنر مورد نظر با موفقیت ایجاد شد";

                return RedirectToAction("Banner");
            }
            TempData[ErrorMessage] = "خطا";
            return View();
        }

        public async Task<IActionResult> DeleteBanner(long id)
        {
            var res = await _SiteService.DeleteBanner(id);

            if (res == true)
            {
                TempData[SuccessMessage] = "بنر مورد نظر با موفقیت حذف شد";

                return RedirectToAction("Banner");
            }
            TempData[ErrorMessage] = "خطا";
            return View();
        }
    }


    
}
