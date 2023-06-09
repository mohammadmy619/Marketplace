using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Contacts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MarcketDataLayer.DTOs.Contacts.FilterProductCommetDto;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class CommentProductsController : AdminBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        public CommentProductsController(IContactService contactService)
        {

            _contactService = contactService;

        }
        #endregion
        public async Task<IActionResult> Index(FilterProductCommetDto filter)
        {

            filter.OrderBy = FilterProductCommetOrder.CreateDate_DES;
            return View(await _contactService.GetAllComment(filter));
        }

        public async Task<IActionResult> Delete(long Id)
        {

            var Delete = await _contactService.DeleteComment(Id);
            if (Delete == false) return RedirectToAction("Index");
            TempData[SuccessMessage] = "باموفقیت حذف شد";
            return RedirectToAction("Index");
        }
    }
}
