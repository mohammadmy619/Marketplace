using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.Articles
{
   public class ArticlesComment: BaseEntity
    {

        public long ArticlesId { get; set; }

        public long? UserId { get; set; }


        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string Email { get; set; }


      
        public virtual User User { get; set; }


        [ForeignKey("ArticlesId")]
        public virtual Articles Articles { get; set; }


    }
}
