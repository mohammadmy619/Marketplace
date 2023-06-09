using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Articles;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Controllers
{
    public class ArticlesController : SiteBaseController
    {
        private IArticlesService _Articlesservese;
        private IUserServicecs _UserServicecs;
        public ArticlesController(IArticlesService Articlesservese, IUserServicecs UserServicecs)
        {
            _Articlesservese = Articlesservese;
            _UserServicecs = UserServicecs;

        }
        [HttpGet("ArticlesPage")]
        //[HttpGet("ArticlesPage/{search}")]
        [HttpGet("ArticlesPage/{GroupName}")]
        public async Task<IActionResult> ArticlesPage(FilterArticlesDTO filter)
         {
            filter.SkipEntity = 6;

            var getarticles =await _Articlesservese.FilterArticles(filter);
            ViewBag.ArticlesGroups =  _Articlesservese.GetArticlesGroups();
            return View(getarticles);
        }

        [HttpGet("DetailArticles/{DetailArticles}")]
        public async Task<IActionResult> DetailArticles(long DetailArticles)
        {

            var Detail = await _Articlesservese.GetArticlesDetail(DetailArticles);
            if (Detail == null) return BadRequest();


            ViewBag.RelatedArticles= await _Articlesservese.RelatedArticles(DetailArticles);
            return View(Detail);
        }

        public async Task<IActionResult> ADDComment(long Articleid, long UserId, string text, string name, string Email,string url)
        {

            if (User.Identity.IsAuthenticated)
            {
               

                var user =await _UserServicecs.GetUserByEmail(User.Identity.Name);

                var wres = await _Articlesservese.AddComment(Articleid, user.Id, text, user.FirstName, user.Email);

                switch (wres)
                {
                    case true:
                        TempData[SuccessMessage] = "کامنت شما با موافثیت ثبت شد";
                        break;

                    case false:
                        TempData[WarningMessage] = "خطا";

                        break;
                }

            }
            if(!User.Identity.IsAuthenticated)
            {
                var res = await _Articlesservese.AddComment(Articleid, UserId, text, name, Email);

                switch (res)
                {
                    case true:
                        TempData[SuccessMessage] = "کامنت شما با موافثیت ثبت شد";
                        break;

                    case false:
                        TempData[WarningMessage] = "خطا";

                        break;
                }
            }
      


            return Redirect(url);

        }
    }
}
