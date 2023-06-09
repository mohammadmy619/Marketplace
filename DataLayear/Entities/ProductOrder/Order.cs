using MarcketDataLayer.Entities.Store;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.ProductOrder
{
   public class Order: BaseEntity
    {
        //public Order()
        //{
        //    OrderDetails = new List<OrderDetail>();
        //}

        #region properties

        public long UserId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool IsPaid { get; set; }

        [Display(Name = "کد پیگیری")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string TracingCode { get; set; }


        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        #endregion

        #region relations

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<OrderIsPaidDetials> OrderIsPaidDetials { get; set; }

        public ICollection<Posts> Posts { get; set; }

        
        //public ICollection<GoodsSold> GoodsSold { get; set; }




        #endregion
    }



}
