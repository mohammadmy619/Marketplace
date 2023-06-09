using MarcketDataLayer.Entities.Products;
using MarcketDataLayer.Entities.Store;
using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.ProductOrder
{
    public class Posts:BaseEntity
    {

        public long UserId { get; set; }
        public long OrderId { get; set; }

        public long SellerId { get; set; }
        public long ProductId { get; set; }

        [Display(Name = "عنوان محصول")]

        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]

        public string ProductTitle { get; set; }


        public string ProductImageName { get; set; }

        public long? ProductColorId { get; set; }

        [Display(Name = "تعداد")]

        public int Count { get; set; }

        [Display(Name = "قیمت محصول")]
        public int ProductPrice { get; set; }

        [Display(Name = "قیمت رنگ محصول")]
        public int ProductColorPrice { get; set; }

        [Display(Name = " رنگ محصول")]
        public string ColorName { get; set; }

        [Display(Name = " تخقیف محصول")]

        public int? DiscountPercentage { get; set; }

        public Poststatus Poststatus { get; set; }

        #region relations
        public User User { get; set; }
        public Seller Seller { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }

        public ProductColor ProductColor { get; set; }

       
        #endregion
    }
    public enum Poststatus
    {
        [Display(Name = "اراسال شده")]
        Posted,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
       
        [Display(Name = "رد شده")]
        Rejected
    }
}
