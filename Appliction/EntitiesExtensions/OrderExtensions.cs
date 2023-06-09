using MarcketDataLayer.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.EntitiesExtensions
{
    public static class OrderExtensions
    {
        public static string GetOrderDetailWithDiscountPrice(this UserOpenOrderDetailItemDTO detail)
        {
            if (detail.DiscountPercentage != null)
            {
                return (detail.ProductPrice * detail.DiscountPercentage.Value / 100 * detail.Count).ToString("#,0 تومان");
            }

            return "------";
        }
    }
}
