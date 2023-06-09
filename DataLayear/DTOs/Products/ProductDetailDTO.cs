using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcketDataLayer.Entities.Products;

namespace MarcketDataLayer.DTOs.Products
{
    public class ProductDetailDTO

    {
        public long ProductId { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }



        public Entities.Store.Seller Seller { get; set; }

        public List<ProductGallery> ProductGalleries { get; set; }

        public List<ProductColor> ProductColors { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }

        public List<Product> RelatedProducts { get; set; }

        public List<MarcketDataLayer.Entities.Products.ProductDiscount> ProductDiscount { get; set; }
        public List<MarcketDataLayer.Entities.Products.ProductCommet> ProductCommet { get; set; }

    }

}
