using MarcketDataLayer.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public interface IPaymentService
    {

        PaymentStatus CreatePaymentRequest(string merchantId, int amount, string description,
      string callbackUrl, ref string redirectUrl, string userEmail = null, string userMobile = null);
        PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long refId);
        string GetAuthorityCodeFromCallback(HttpContext context);


    }
}
