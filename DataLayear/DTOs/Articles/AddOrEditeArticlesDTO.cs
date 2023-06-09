using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Articles
{
   public class AddOrEditeArticlesDTO
    {
        public long? ArticleId { get; set; }

        public long ArticlesGroupsId { get; set; }


    


        [Display(Name = "متن عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string ImageAlt { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string name { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(350)]
        public string Title { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        //[AllowHtml]
        public string Text { get; set; }



        //[Display(Name = "تاریخ ایجاد")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}")]
        //public DateTime DateTime { get; set; }


        [Display(Name = "تگ ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string Tags { get; set; }

     
        public string Mp3name { get; set; }

    
        public string Videoname { get; set; }


        public string ImageName { get; set; }


    }


    public enum ArticlesResult
    {
        succses,
        error
    }
}
