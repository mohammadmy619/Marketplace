using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Contacts
{
   public class AnswerTicketDTO
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }

        
    }
    public enum AnswerTicketResult
    {
        NotForUser,
        NotFound,
        Success
    }
}
