using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region index

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
