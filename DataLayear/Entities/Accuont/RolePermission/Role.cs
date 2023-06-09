
using MarketPlace.DataLayer.Entities.Commen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarcketDataLayer.Entities.Accuont.RolePermission
{
    public class Role : BaseEntity
    {
        #region properties

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string RoleTitle { get; set; }
        #endregion

        #region relations
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
