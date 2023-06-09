using System.ComponentModel.DataAnnotations;
using MarcketDataLayer.DTOs.Site;


namespace MarcketDataLayer.DTOs.Account
{
    public class ForgotPasswordDTO : CaptchaViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }
    }

    public enum ForgotPasswordResult
    {
        Success,
        NotFound,
        Error
    }
}
