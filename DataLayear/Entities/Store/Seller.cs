using MarcketDataLayer.Entities.ProductOrder;
using MarcketDataLayer.Entities.Wallet;
using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.Store
{
   public class Seller: BaseEntity
    {
        #region properties

        public long UserId { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string StoreName { get; set; }

        [Display(Name = "تلفن فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Phone { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Address { get; set; }

        [Display(Name = "توضیحات فروشگاه")]
        public string Description { get; set; }

        [Display(Name = "یادداشت های ادمین")]
        public string AdminDescription { get; set; }

        [Display(Name = "توضیحات تایید / عدم تایید اطلاعات")]
        public string StoreAcceptanceDescription { get; set; }

        public StoreAcceptanceState StoreAcceptanceState { get; set; }

        #endregion

        #region relations

        public User User { get; set; }
        public ICollection<SellerWallet> SellerWallets { get; set; }
        public ICollection<Posts> Posts { get; set; }


        





        #endregion
    }

    public enum StoreAcceptanceState
    {
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
