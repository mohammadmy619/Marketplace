using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Contacts
{
   public class FilterProductCommetDto:BasePaging
    {

        public List<ProductCommet> ProductCommet { get; set; }

        public FilterProductCommetDto SetProductCommet(List<ProductCommet> productCommet)
        {
            this.ProductCommet = productCommet;
            return this;
        }



        public FilterProductCommetDto SetPaging(BasePaging paging)
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

        public FilterProductCommetOrder OrderBy { get; set; }

        public enum FilterProductCommetOrder
        {
            CreateDate_DES,
            CreateDate_ASC,
        }
    }
}
