using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Orders
{
   public class UserOpenOrderDetailItemDTO
    {
        public long DetailId { get; set; }
        public long ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImageName { get; set; }

        public long? ProductColorId { get; set; }

        public int Count { get; set; }

        public int ProductPrice { get; set; }

        public int ProductColorPrice { get; set; }

        public string ColorName { get; set; }
        public int? DiscountPercentage { get; set; }

        public int GetOrderDetailWithDiscountPriceAmount()
        {
            if (this.DiscountPercentage != null)
            {
                return (this.ProductPrice + this.ProductColorPrice) * this.DiscountPercentage.Value / 100 * this.Count;
            }

            return 0;
        }

        public int GetTotalAmountByDiscount()
        {
            return (ProductPrice + ProductColorPrice) * Count - this.GetOrderDetailWithDiscountPriceAmount();
        }


        public string GetOrderDetailWithDiscountPrice()
        {
            if (this.DiscountPercentage != null)
            {
                return this.GetOrderDetailWithDiscountPriceAmount().ToString("#,0 تومان");
            }

            return "------";
        }

    }

  
}
