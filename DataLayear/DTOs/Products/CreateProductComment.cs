using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Products
{
   public class CreateProductComment
    {
        public long ProductID { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Massege { get; set; }

        public long UserID { get; set; }
    }

    public enum CraeteComment
    {
        Success, 
        Error,
        NotForUser

    }
}
