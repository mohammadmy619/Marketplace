using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.ContactUs
{
   public class Ticket: BaseEntity
    {
        #region properties

        public long OwnerId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "وضعیت تیکت")]
        public TicketState TicketState { get; set; }

        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        public bool IsReadByOwner { get; set; }

        public bool IsReadByAdmin { get; set; }

        #endregion

        #region relations

        public User Owner { get; set; }
        public ICollection<TicketMessage> TicketMessages { get; set; }

        #endregion
    }

    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        Support,
        [Display(Name = "فنی")]
        Technical,
        [Display(Name = "آموزشی")]
        Academic
    }

    public enum TicketPriority
    {
        [Display(Name = "کم")]
        Low,
        [Display(Name = "متوسط")]
        Medium,
        [Display(Name = "زیاد")]
        High
    }

    public enum TicketState
    {
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }


}

