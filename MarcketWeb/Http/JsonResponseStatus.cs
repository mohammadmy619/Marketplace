using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Http
{
    public class JsonResponseStatus
    {
        public static JsonResult SendStatus(JsonResponseStatusType type, string message, object data)
        {
            return new JsonResult(new { status = type.ToString(), message = message, data = data });
        }
    }

    public enum JsonResponseStatusType
    {
        Success,
        Warning,
        Danger,
        Info
    }
}
