using MarcketDataLayer.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Articles
{
   public class FilterCommentDTO:BasePaging
    {
        public string search { get; set; }
        public List<MarcketDataLayer.Entities.Articles.ArticlesComment> ArticlesComment { get; set; }


        public FilterCommentDTO SetComment(List<MarcketDataLayer.Entities.Articles.ArticlesComment> articlesComment)
        {
            this.ArticlesComment = articlesComment;
            return this;
        }

        public FilterCommentDTO SetPaging(BasePaging paging)
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
