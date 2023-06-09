using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.Products
{
    public class ProductCommet:BaseEntity
    {
        public long ProductID { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Massege { get; set; }

        public long UserID { get; set; }


        #region relations


        public Product Product { get; set; }

        public User User { get; set; }
        #endregion
    }
}
