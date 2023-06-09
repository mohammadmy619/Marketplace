using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcketDataLayer.Entities.Articles;
using MarcketDataLayer.DTOs.Paging;

namespace MarcketDataLayer.DTOs.Articles
{
   public class FilterArticlesDTO: BasePaging
    {
        public string search { get; set; }
        public string GroupName { get; set; }

        public string Tags { get; set; }

        public List<MarcketDataLayer.Entities.Articles.Articles> Articles { get; set; }


        public FilterArticlesDTO SetArticles(List<MarcketDataLayer.Entities.Articles.Articles> articles)
        {
            this.Articles = articles;
            return this;
        }

        public FilterArticlesDTO SetPaging(BasePaging paging)
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
