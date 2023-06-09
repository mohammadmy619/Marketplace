using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Contacts;
using MarketPlaceWeb.PresentationExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class TicketController : AdminBaseController
    {

        private IContactService _contactService;
        public TicketController(IContactService contactService)
        {
            _contactService = contactService;
        }
        // GET: TicketController
        public async Task<IActionResult> Index(FilterTicketDTO filter)
        {
            //filter.UserId = User.GetUserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_DES;

            return View(await _contactService.FilterTickets(filter));
        }

        // GET: TicketController/Details/5
        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> Details(long ticketId)
        {
            var ticket = await _contactService.GetTicketForShowByAdmin(ticketId, User.GetUserId());

            if (ticket == null) return NotFound();

            return View(ticket);
        }
        // GET: TicketController/Create
   

        // POST: TicketController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnswerTicketDTO answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[ErrorMessage] = "لطفا متن پیام خود را وارد نمایید";
            }

            if (ModelState.IsValid)
            {
                var res = await _contactService.AnswerTicketByAdmin(answer, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[InfoMessage] = "در صورت تکرار این مورد ، دسترسی شما به صورت کلی از سیستم قطع خواهد شد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("Details", "Ticket", new { ticketId = answer.Id });
        }

        // GET: TicketController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TicketController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TicketController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
