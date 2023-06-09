using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Contacts
{
   public class FilterContactusDto:BasePaging
    {

        public List<ContactUs> ContactUs { get; set; }

        public FilterContactusDto SetContactUs(List<ContactUs> ContactUs)
        {
            this.ContactUs = ContactUs;
            return this;
        }

     

        public FilterContactusDto SetPaging(BasePaging paging)
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

        public FilterContactOrder OrderBy { get; set; }

        public enum FilterContactOrder
        {
            CreateDate_DES,
            CreateDate_ASC,
        }
    }

}

   
