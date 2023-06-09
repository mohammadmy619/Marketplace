using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;

namespace MarcketDataLayer.Entities.Accuont.RolePermission
{
    public class UserRole:BaseEntity
    {
        #region properties
        public long UserId { get; set; }
        public long RoleId { get; set; }
        #endregion

        #region relations
        public User User { get; set; }
        public Role Role { get; set; }
        #endregion
    }
}
