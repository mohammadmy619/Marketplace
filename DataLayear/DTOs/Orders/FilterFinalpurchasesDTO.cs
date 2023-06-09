using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Orders
{
   public class FilterFinalpurchasesDTO:BasePaging
    {
        public List<Order> Order { get; set; }

        public FilterFinalpurchasesDTO SetFinalpurchases(List<Order> order)
        {

            Order=order;
            return this;
        }

        public FilterFinalpurchasesDTO SetPaging(BasePaging paging)
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
