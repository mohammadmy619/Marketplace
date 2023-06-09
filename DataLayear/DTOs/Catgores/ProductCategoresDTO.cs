using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Catgores
{
   public class ProductCategoresDTO 
    {

        public List<ProductCategory> ProductCategory { get; set; }


        public ProductCategoresDTO SetTickets(List<ProductCategory> productCategory)
        {
            this.ProductCategory = productCategory;

            return this;
        }

        //public ProductCategoryFilterDTO SetPaging(BasePaging paging)
        //{
        //    this.PageId = paging.PageId;
        //    this.AllEntitiesCount = paging.AllEntitiesCount;
        //    this.StartPage = paging.StartPage;
        //    this.EndPage = paging.EndPage;
        //    this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
        //    this.TakeEntity = paging.TakeEntity;
        //    this.SkipEntity = paging.SkipEntity;
        //    this.PageCount = paging.PageCount;
        //    return this;
        //}
    }
}
