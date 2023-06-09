using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Products
{
   public class CreateOrEditProductGalleryDTO
    {
        [Display(Name = "اولویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int DisplayPriority { get; set; }

        [Display(Name = "تصویر")]
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
    }
    public enum CreateOrEditProductGalleryResult
    {
        Success,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }
}
