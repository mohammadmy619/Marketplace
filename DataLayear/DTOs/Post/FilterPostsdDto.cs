using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Post
{
   public class FilterPostsdDto:BasePaging
    {

        public List<Posts> Posts { get; set; }
        #region methods

        public FilterPostsdDto SetPost(List<Posts> posts)
        {
            this.Posts = posts;

            return this;
        }

        public FilterPoststatus FilterPoststatus { get; set; }
        public FilterPostsdDto SetPaging(BasePaging paging)
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

    public enum FilterPoststatus
    {
        [Display(Name = "اراسال شده")]
        Posted,
        [Display(Name = "در حال بررسی")]
        UnderProgress,

        [Display(Name = "رد شده")]
        Rejected
    }
}
