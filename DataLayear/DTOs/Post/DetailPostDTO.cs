using MarcketDataLayer.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Post
{
   public class DetailPostDTO
    {
        public long Id { get; set; }


        

        public string ProductTitle { get; set; }


        public string ProductImageName { get; set; }

        public long? ProductColorId { get; set; }

        

        public int Count { get; set; }

    
        public int ProductPrice { get; set; }

        
        public int ProductColorPrice { get; set; }

        
        public string ColorName { get; set; }

        

        public int? DiscountPercentage { get; set; }
         

        public Poststatus Poststatus { get; set; }

        public long UserId { get; set; }
        public string Email { get; set; }

     

        public string Mobile { get; set; }

   
        public string FirstName { get; set; }

      
     
        public string LastName { get; set; }

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

        public int GetTotalPrice()
        {
            //return Details.Sum(s => (s.ProductPrice + s.ProductColorPrice) * s.Count);

            var res = Count * (ProductPrice + ProductColorPrice);

            return res;
        }
        //public int GetTotalDiscounts()
        //{
        //    return Details.Sum(s => s.GetOrderDetailWithDiscountPriceAmount());
        //}
    }
}
