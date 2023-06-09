using MarcketDataLayer.Entities.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Contacts
{
   public class TicketDetailDTO
    {
        public Ticket Ticket { get; set; }

        public List<TicketMessage> TicketMessages { get; set; }
    }
}
