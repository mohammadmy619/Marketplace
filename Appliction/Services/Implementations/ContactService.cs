using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Contacts;
using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.Contacts;
using MarcketDataLayer.Entities.ContactUs;
using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarcketDataLayer.DTOs.Contacts.FilterContactusDto;
using static MarcketDataLayer.DTOs.Contacts.FilterProductCommetDto;

namespace MarcketAppliction.Services.Implementations
{
    public class ContactService : IContactService
    {
        #region constructor
        private IGenericRepository<ContactUs> _contactUsRepository;
        private  IGenericRepository<Ticket> _ticketRepository;
        private  IGenericRepository<TicketMessage> _ticketMessageRepository;
        private IGenericRepository<ProductCommet> _ProductCommeteRepository;

        public ContactService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository, IGenericRepository<ProductCommet> ProductCommeteRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _ticketRepository = ticketRepository;
            _ProductCommeteRepository=ProductCommeteRepository;
    }

        #endregion

        #region contact us

        public async Task<FilterContactusDto> GetAllContactUs(FilterContactusDto filter)
        {
            var query = _contactUsRepository.GetQuery().Where(x=>!x.IsDelete).Include(u => u.User).AsNoTracking().AsQueryable();
   
          
            #region state

         
            switch (filter.OrderBy)
            {
                case FilterContactOrder.CreateDate_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterContactOrder.CreateDate_DES:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
            }

            #endregion

           
            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetContactUs(allEntities);
        }
        public async Task<bool> DeleteContactUs(long id)
        {
            var get = await _contactUsRepository.GetEntityById(id);
            if (get == null) return false;
            _contactUsRepository.DeleteEntity(get);
           await _contactUsRepository.SaveChanges();

            return true;


        }

        public async Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId)
        {
            var newContact = new ContactUs
            {
                UserId = userId != null && userId.Value != 0 ? userId.Value : (long?)null,
                Subject = contact.Subject,
                Email = contact.Email,
                UserIp = userIp,
                Text = contact.Text,
                FullName = contact.FullName
            };

            await _contactUsRepository.AddEntity(newContact);
            await _contactUsRepository.SaveChanges();
        }
        public async Task<ContactUs> GetContactUs(long id)
        {
            var getContactUs =await _contactUsRepository.GetQuery().Where(z => z.Id == id).Include(s=>s.User).FirstOrDefaultAsync();
            if (getContactUs == null) return null;

          

            return  getContactUs;
        }
        #endregion
        #region Comment
        public async Task<FilterProductCommetDto> GetAllComment(FilterProductCommetDto filter)
        {
            var query = _ProductCommeteRepository.GetQuery().Where(x => !x.IsDelete).Include(u => u.User).Include(c=>c.Product).AsQueryable();


            #region state


            switch (filter.OrderBy)
            {
                case FilterProductCommetOrder.CreateDate_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterProductCommetOrder.CreateDate_DES:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
            }

            #endregion


            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProductCommet(allEntities);
        }

        public Task<ProductCommet> GetComment(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteComment(long id)
        {
            var get =await _ProductCommeteRepository.GetEntityById(id);

            if (get == null) return false;
            _ProductCommeteRepository.DeleteEntity(get);
          await  _ProductCommeteRepository.SaveChanges();

            return true;
            
        }
        #endregion

        #region ticket

        public async Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId)
        {
            if (string.IsNullOrEmpty(ticket.Text)) return AddTicketResult.Error;

            // add ticket
            var newTicket = new Ticket
            {
                OwnerId = userId,
                IsReadByOwner = true,
                TicketPriority = ticket.TicketPriority,
                Title = ticket.Title,
                TicketSection = ticket.TicketSection,
                TicketState = TicketState.UnderProgress
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();

            // add ticket message
            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                Text = ticket.Text,
                SenderId = userId,
            };

            await _ticketMessageRepository.AddEntity(newMessage);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }

        #endregion
        #region ticket


        public async Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter)
        {
            var query = _ticketRepository.GetQuery().Include(u=>u.Owner).AsQueryable();
            var user =await query.Select(x => x.Owner).FirstOrDefaultAsync() ;
            //filter.User = user;
            #region state

            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
            }

            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDate_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_DES:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
            }

            #endregion

            #region filter

            if (filter.TicketSection != null)
                query = query.Where(s => s.TicketSection == filter.TicketSection.Value);

            if (filter.TicketPriority != null)
                query = query.Where(s => s.TicketPriority == filter.TicketPriority.Value);

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.OwnerId == filter.UserId.Value);

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            #endregion
            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetTickets(allEntities).SetUser(user);
            
        }
        public async Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().AsQueryable()
                .Include(s => s.Owner)
                .SingleOrDefaultAsync(s => s.Id == ticketId);

            if (ticket == null || ticket.OwnerId != userId) return null;

            return new TicketDetailDTO
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetQuery().AsQueryable()
                    .OrderByDescending(s => s.CreateDate)
                    .Where(s => s.TicketId == ticketId && !s.IsDelete).ToListAsync()
            };
        }

        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;
            
            if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;

            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text
            };

            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;
            ticket.TicketState = TicketState.UnderProgress;
            await _ticketRepository.SaveChanges();
            return AnswerTicketResult.Success;
        }

        public async Task<TicketDetailDTO> GetTicketForShowByAdmin(long ticketId, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().AsQueryable()
                .Include(s => s.Owner)
                .SingleOrDefaultAsync(s => s.Id == ticketId);
            ticket.IsReadByAdmin = true;
            ticket.IsReadByOwner = false;
            
            await _ticketRepository.SaveChanges();
            //if (ticket == null || ticket.OwnerId != userId) return null;

            return new TicketDetailDTO
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetQuery().AsQueryable()
                    .OrderByDescending(s => s.CreateDate)
                    .Where(s => s.TicketId == ticketId && !s.IsDelete).ToListAsync()
            };
            //return null;
        }


        public async Task<AnswerTicketResult> AnswerTicketByAdmin(AnswerTicketDTO answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;

            //if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;

            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text
            };

            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByAdmin = true;
            ticket.IsReadByOwner = true;
            ticket.TicketState = TicketState.Answered;
            await _ticketRepository.SaveChanges();
            return AnswerTicketResult.Success;
        }


        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
        }

     
        #endregion


    }
}
