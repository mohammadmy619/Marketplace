using MarcketDataLayer.DTOs.Contacts;
using MarcketDataLayer.Entities.Contacts;
using MarcketDataLayer.Entities.ContactUs;
using MarcketDataLayer.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
    public interface IContactService : IAsyncDisposable
    {
        Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId);
        Task<FilterContactusDto> GetAllContactUs(FilterContactusDto filter);
        Task<ContactUs> GetContactUs(long id);
        Task<bool> DeleteContactUs(long id);

        Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId);
        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter);
        Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId);

        Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answer, long userId);

        Task<TicketDetailDTO> GetTicketForShowByAdmin(long ticketId, long userId);

        Task<AnswerTicketResult> AnswerTicketByAdmin(AnswerTicketDTO answer, long userId);

        Task<FilterProductCommetDto> GetAllComment(FilterProductCommetDto filter);
    
        Task<bool> DeleteComment(long id);


    }
}
