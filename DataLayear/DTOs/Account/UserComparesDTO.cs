using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Account
{
    public class UserComparesDTO : BasePaging
    {
        public List<UserCompare> UserCompares { get; set; }

       
        public UserComparesDTO SetCompares(List<UserCompare> userCompares)
        {
            this.UserCompares = userCompares;
            return this;
        }
        public UserComparesDTO SetPaging(BasePaging paging)
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

