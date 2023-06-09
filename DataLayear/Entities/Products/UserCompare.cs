using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.Products
{
    public class UserCompare : BaseEntity
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }

        #region relations
        public User User { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}
