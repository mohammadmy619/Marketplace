using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.ContactUs;
using MarketPlace.DataLayer.Entities.Accuont.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.DTOs.Contacts
{
   public class FilterTicketDTO: BasePaging
    {
        public FilterTicketDTO SetTickets(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(BasePaging paging)
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
        public FilterTicketDTO SetUser(User User)
        {
            this.User = User;
            return this;
        }


        #region properties

        public string Title { get; set; }

        public long? UserId { get; set; }

        public FilterTicketState FilterTicketState { get; set; }

        public TicketSection? TicketSection { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        public FilterTicketOrder OrderBy { get; set; }

        public List<Ticket> Tickets { get; set; }
        public User User { get; set; }


        #endregion
    }

    public enum FilterTicketState
    {
        All,
        NotDeleted,
        Deleted
    }

    public enum FilterTicketOrder
    {
        CreateDate_DES,
        CreateDate_ASC,
    }
}
