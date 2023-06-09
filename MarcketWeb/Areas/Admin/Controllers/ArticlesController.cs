using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Articles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class ArticlesController : AdminBaseController
    {
        private IArticlesService _articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            _articlesService = articlesService;

        }
        [HttpGet("FilterGroups")]
        public async Task<IActionResult> FilterGroups(FilterArticlesGroupsDto filter)
        {
            filter.TakeEntity = 6;


           var FilterGroups= await _articlesService.FilterArticlesGroups(filter); 


            return View(FilterGroups);

        }

        [HttpGet]
        public IActionResult CreateGroup()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateGroup(AddorEditArticleGroupDTO group,IFormFile img)
        {

            if (ModelState.IsValid)
            {
                var res = await _articlesService.AddorEditArticleGroup(group, img);
                switch (res)
                {
                    case AddorEditArticleGroupResul.succses:
                        TempData[SuccessMessage] = "گروه فوق با موفقیت ایجاد شد";

                        return RedirectToAction("FilterGroups");

                        break;

                    case AddorEditArticleGroupResul.Error:
                        TempData[ErrorMessage] = "خطا";

                        break;
                   
                }
            }

            return View(group);

        }

        [HttpGet("getGroup")]
        public async Task<IActionResult> getGroup(long groupid)
        {
            var getgroups =await _articlesService.GetGroupsByID(groupid);


            return View(getgroups);
        }
        [HttpPost("getGroup")]

        public async Task<IActionResult> getGroup(AddorEditArticleGroupDTO group, IFormFile img)
        {
            var getgroups = await _articlesService.GetGroupsByID(group.ArticleGroupID);

            if (ModelState.IsValid)
            {
                var res = await _articlesService.AddorEditArticleGroup(group, img);
                switch (res)
                {
                    case AddorEditArticleGroupResul.succses:
                        TempData[SuccessMessage] = "گروه فوق با موفقیت ویراش شد";

                        return RedirectToAction("FilterGroups");

                        break;

                    case AddorEditArticleGroupResul.Error:
                        TempData[ErrorMessage] = "خطا";

                        break;

                }
            }
            return View(getgroups);
        }


        public async Task<IActionResult> DeleteGroup(long groupid)
        {
            var res =await _articlesService.DeleteArticleGroupByid(groupid);

            if(res == true)
            {
                TempData[SuccessMessage] = "گروه فوق با موفقیت حذف  شد";

                return RedirectToAction("FilterGroups");

            }
            TempData[ErrorMessage] = "خطا";

            return RedirectToAction("FilterGroups");

        }

        [HttpGet("FilterArticles")]
        public async Task<IActionResult> FilterArticles(FilterArticlesDTO filter)
        {
            filter.TakeEntity = 6;

            var FilterGroups = await _articlesService.FilterArticles(filter);

            return View(FilterGroups);

        }

        [HttpGet("CreateArticles")]

        public IActionResult CreateArticles()
        {
            ViewBag.groupArticle = _articlesService.GetArticlesGroups();
            return View();
        }
        [HttpPost("CreateArticles")]

        public async Task<IActionResult> CreateArticles(AddOrEditeArticlesDTO article,IFormFile Img,IFormFile mp3,IFormFile video, long selectedGroups)
        {
            ViewBag.groupArticle = _articlesService.GetArticlesGroups();

            if (ModelState.IsValid)
            {
                var res = await _articlesService.AddorEDiteArticles(article,selectedGroups,Img,mp3,video);

                switch (res)
                {
                    case ArticlesResult.succses:
                        TempData[SuccessMessage] = "مقاله فوق با موفقیت ایجاد شد ";

                        return RedirectToAction("FilterArticles");

                        break;

                    case ArticlesResult.error:
                        TempData[ErrorMessage] = "خطا";

                        break;

                }
            }

            return View(article);
        }

        [HttpGet("EditArticles")]

        public async Task<IActionResult> EditArticles(long articleid)
        {
            var getArticles = await _articlesService.GetArticlesByID(articleid);

            ViewBag.groupArticle = _articlesService.GetArticlesGroups();

            return View(getArticles);
        }
        [HttpPost("EditArticles")]

        public async Task<IActionResult> EditArticles(AddOrEditeArticlesDTO article, IFormFile Img, IFormFile mp3, IFormFile video, long selectedGroups)
        {
            ViewBag.groupArticle = _articlesService.GetArticlesGroups();

            if (ModelState.IsValid)
            {
                var res = await _articlesService.AddorEDiteArticles(article, selectedGroups, Img, mp3, video);

                switch (res)
                {
                    case ArticlesResult.succses:
                        TempData[SuccessMessage] = "مقاله فوق با موفقیت ایجاد شد ";

                        return RedirectToAction("FilterArticles");

                        break;

                    case ArticlesResult.error:
                        TempData[ErrorMessage] = "خطا";

                        break;

                }
            }

            return View(article);
        }
        [HttpGet("DeleteArticle")]

        public async Task<IActionResult> DeleteArticle(long Articleid)
        {
            var res = await _articlesService.DeleteArticleByid(Articleid);

            if (res == true)
            {
                TempData[SuccessMessage] = "مقاله فوق با موفقیت حذف  شد";

                return RedirectToAction("FilterArticles");

            }
            TempData[ErrorMessage] = "خطا";

            return RedirectToAction("FilterArticles");
        }

        [HttpGet("ManegeComment")]

        public async Task<IActionResult> ManegeComment(FilterCommentDTO filter)
        {
            filter.TakeEntity = 10;
            var filters =await _articlesService.FilterComment(filter);
            return View(filters);
        }

        [HttpGet("DeleteComment/{Commentid}")]

        public async Task<IActionResult> DeleteComment(long Commentid)
        {
            var res = await _articlesService.DeleteCommentyid(Commentid);

            if (res == true)
            {
                TempData[SuccessMessage] = "کامنت فوق با موفقیت حذف  شد";

                return RedirectToAction("ManegeComment");

            }
            TempData[ErrorMessage] = "خطا";

            return RedirectToAction("ManegeComment");
        }





    }
}
