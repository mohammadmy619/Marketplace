using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Account
{
   public class UserFavoritsDTO: BasePaging
    {


        public List<UserFavorite> UserFavorites { get; set; }

        #region methods
        public UserFavoritsDTO SetFavorites(List<UserFavorite> userFavorites)
        {
            this.UserFavorites = userFavorites;
            return this;
        }

        public UserFavoritsDTO SetPaging(BasePaging paging)
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
