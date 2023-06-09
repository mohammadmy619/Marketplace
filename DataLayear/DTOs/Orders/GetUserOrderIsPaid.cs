using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Orders
{
   public class GetUserOrderIsPaid
    {
        public long UserId { get; set; }

        public string Description { get; set; }

        public List<DetailFinalpurchases> Details { get; set; }

        public int GetTotalPrice()
        {
            return Details.Sum(s => (s.ProductPrice + s.ProductColorPrice) * s.Count);
        }
        public int GetTotalDiscounts()
        {
            return Details.Sum(s => s.GetOrderDetailWithDiscountPriceAmount());
        }
    }
}
