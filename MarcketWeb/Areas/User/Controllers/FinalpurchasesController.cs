using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Orders;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    public class FinalpurchasesController : UserBaseController
    {
        private readonly IOrderService _orderService;
        public FinalpurchasesController(IOrderService orderService)
        {
            _orderService = orderService;

        }
        [HttpGet("Final")]
        public async Task<IActionResult>  Index (FilterFinalpurchasesDTO filter)
        {
            filter.TakeEntity = 6;

            var Getfinal = await _orderService.GetAllFinalpurchasesbyuserId(filter, User.GetUserId());
            

            return View(Getfinal);
        }

        [HttpGet("detailFinalpurchases/{id}")]
        public async Task<IActionResult> detailFinalpurchases(long id)
        {
            var getDitailFinalpurchases = await _orderService.DetailFinalpurchases(id,User.GetUserId());

            return View(getDitailFinalpurchases);
        }
    }
}
