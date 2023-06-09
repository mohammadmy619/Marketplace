using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.Articles
{
  public  class ArticlesGroups:BaseEntity
    {

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string GroupTitle { get; set; }

        [Display(Name = "توضیح مختصر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string GroupDescriptison { get; set; }
        public string ImageName { get; set; }
        //Navigation Property
        public virtual List<Articles> Articles { get; set; }
    }
}
