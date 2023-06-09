using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceWeb.PresentationExtensions
{
    public static class HttpExtensions
    {
        public static string GetUserIp(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
