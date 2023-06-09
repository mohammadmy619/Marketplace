using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Articles
{
   public class FilterArticlesGroupsDto:BasePaging
    {
        public string search { get; set; }

        public List<ArticlesGroups> ArticlesGroups { get; set; }


        public FilterArticlesGroupsDto SetArticlesGroups(List<ArticlesGroups> articlesGroups)
        {
            this.ArticlesGroups = articlesGroups;
            return this;
        }

        public FilterArticlesGroupsDto SetPaging(BasePaging paging)
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
