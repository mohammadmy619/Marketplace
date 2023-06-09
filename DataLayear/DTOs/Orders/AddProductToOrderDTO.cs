using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Orders
{
   public class AddProductToOrderDTO
    {

        public long ProductId { get; set; }
        public long? ProductColorId { get; set; }
        public int Count { get; set; }

    }
}
