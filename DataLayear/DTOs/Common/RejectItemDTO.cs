using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Common
{
   public class RejectItemDTO
    { 
        public long Id { get; set; }

        [Display(Name = "توضیحات عدم تایید اطلاعات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RejectMessage { get; set; }
    }
}
