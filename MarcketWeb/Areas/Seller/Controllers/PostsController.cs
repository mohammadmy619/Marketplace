using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Post;
using MarcketDataLayer.Entities.ProductOrder;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Seller.Controllers
{
    public class PostsController : SellerBaseController
    {

        
        private readonly ISellerService _sellerService;

        public PostsController( ISellerService sellerService)
        {
            
            _sellerService = sellerService;
        }

        [HttpGet("Posts-list")]
        public async Task<IActionResult>  Index(FilterPostsdDto filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            if (seller== null) return NotFound();


            return View( await _sellerService.FilterPosts(filter, seller.Id));
        }

        [HttpGet("DetailPost")]
        public async Task<IActionResult> DetailPost(long id)
        {
            var DetailPost = await _sellerService.DetailPost(id);
            if (DetailPost == null) return NotFound();


            return View(DetailPost);
        }
        [HttpPost("DetailPost")]
        public async Task<IActionResult> DetailPost( int Poststatus, long PostId)
        {
            var res = await _sellerService.EditPostStatus(Poststatus, PostId);
            switch (res)
            {
                case EditPostStatus.Error:
                    TempData[WarningMessage] = "خطا";
                    break;
                case EditPostStatus.Success:
                    TempData[SuccessMessage] = "عملیات ثبت انجام شد";
                    break;
               
            }
            var DetailPost = await _sellerService.DetailPost(PostId);
            if (DetailPost == null) return NotFound();

            return View(DetailPost);

        }



    }
}
