using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Articles
{
   public class AddorEditArticleGroupDTO
    {

        public long ArticleGroupID { get; set; }

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string GroupTitle { get; set; }

        [Display(Name = "توضیح مختصر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string GroupDescriptison { get; set; }

        public string imgname { get; set; }

    }

    public enum AddorEditArticleGroupResul
    {
        succses,
        Error
    }
}
