using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Accuont.RolePermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.UserManege
{
   public class FilterRolesDTO:BasePaging
    {
        public string RoleName { get; set; }
        public List<Role> Roles { get; set; }


        #region methods
        public FilterRolesDTO SetRoles(List<Role> roles)
        {
            this.Roles = roles;
            return this;
        }

        public FilterRolesDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }
}
