using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    [Route("user")]
    public class UserBaseController : Controller
    {
        protected string ErrorMessage = "ErrorMessage";
        protected string WarningMessage = "WarningMessage";
        protected string SuccessMessage = "SuccessMessage";
       
        protected string InfoMessage = "InfoMessage";
       
    }
}
