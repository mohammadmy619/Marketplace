using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Account
{
   public class RecoverUserPasswordForEmailDTO
    {
        public string USerName { get; set; }
        public string Password { get; set; }
    }
}
