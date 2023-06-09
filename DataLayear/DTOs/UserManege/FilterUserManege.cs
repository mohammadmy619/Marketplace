using MarcketDataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Accuont.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.UserManege
{
   public class FilterUserManege:BasePaging
    {
        public List<User> User { get; set; }

        public FilterUserManege SetUsers(List<User> users)
        {

            User = users;
            return this;
        }

        public FilterUserManege SetPaging(BasePaging paging)
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
    }
}
