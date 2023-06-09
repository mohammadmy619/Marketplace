using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Contacts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MarcketDataLayer.DTOs.Contacts.FilterContactusDto;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class ContactUsController : AdminBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        public ContactUsController(IContactService contactService)
        {
            
            _contactService = contactService;
          
        }
        #endregion
        public async Task<IActionResult>  Index(FilterContactusDto filter)
        {

            filter.OrderBy = FilterContactOrder.CreateDate_DES;

            return View(await _contactService.GetAllContactUs(filter));
        }
        public async Task<IActionResult> Details(long id)
        {
            var Details =await _contactService.GetContactUs(id);

            return View(Details);
        }

        public async Task<IActionResult> Delete(long id)
        {
            var Details = await _contactService.DeleteContactUs(id);
            if (Details == false) return RedirectToAction("Details", id);
            TempData[SuccessMessage] = "باموفقیت حذف شد";
            return RedirectToAction("Index");
        }
    }
}
