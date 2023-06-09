using GoogleReCaptcha.V3.Interface;
using MarcketAppliction.Services.Interface;
using MarcketAppliction.Utils;
using MarcketDataLayer.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MarketPlaceWeb.EmailRenderView;
using MarketPlaceWeb.Auth;
using Microsoft.AspNetCore.Identity;

namespace MarketPlaceWeb.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constructor

        private readonly IUserServicecs _userService;
        private readonly ICaptchaValidator _captchaValidator;
        private IViewRenderService _viewRender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IUserServicecs userService, ICaptchaValidator captchaValidator, IViewRenderService viewRender, SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _viewRender=viewRender;
            _signInManager = signInManager;
        }

        #endregion

        #region register

        [HttpGet("register")]
        public IActionResult Register()
        {


            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.MobileExists:
                        TempData["ErrorMessage"] = "تلفن همراه وارد شده  یا ایمیل تکراری می باشد";
                        ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری یا ایمیل  می باشد");
                        break;
                    case RegisterUserResult.Success:
                        TempData["SuccessMessage"] = "ثبت نام شما با موفقیت انجام شد";
                        TempData["InfoMessage"] = " لطفا ایمیل خود را چک کنید   ";

                        var GetUser =await _userService.GetUserByEmail(register.Email);

                        string body =  _viewRender.RenderToStringAsync("ActiveEmail", GetUser);

                        SendEmail.Send(register.Email, "فعالسازی", body);

                        return RedirectToAction("Login");
                }
            }

            return View(register);
        }

        #endregion

        #region ActiveACCOUNT   
        [HttpGet]
        public async Task<IActionResult> ActiveAccount(string id)
        {
            ViewBag.IsActive =await _userService.ActiveAccount(id);

            if (ViewBag.IsActive)
            {
                TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد ";

            }
            return RedirectToAction("Login");
        }
        #endregion

        #region login



        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(login);
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.GetUserForLogin(login);
                switch (res)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case LoginUserResult.NotActivated:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                        break;
                    case LoginUserResult.Success:

                        var user = await _userService.GetUserByEmail(login.Email);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.Email),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                           
                        };

                        await HttpContext.SignInAsync(principal, properties);

                        TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";

                        return Redirect("/");
                }
            }


            return View(login);
        }

        #endregion


        #region loginGoogle
        [Route("provider/{provider}")]
        public IActionResult GetProvider(string provider)
        {
            var redirectUrl = Url.RouteUrl("ExternalLogin", Request.Scheme);

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }


        [Route("external-login", Name = "ExternalLogin")]
        public async Task<IActionResult> ExternalLogin()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            string userEmail = User.FindFirstValue(ClaimTypes.Email);


            var findUser =await _userService.GetUserByEmail(userEmail);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            if (findUser == null)
            {
               await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData[ErrorMessage] = "کاربر فوق یافت نشد";
                ViewBag.Erorr = "کاربر فوق یافت نشد";
                await _signInManager.SignOutAsync();

                return RedirectToAction("login");
            }
            var res =await _userService.GetUserForLoginByEmail(userEmail);

            //Todo: Login User

            switch (res)
            {
                case LoginUserResult.NotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                    await _signInManager.SignOutAsync();
                    break;
                case LoginUserResult.NotActivated:
                    TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                    await _signInManager.SignOutAsync();
                    break;
                case LoginUserResult.Success:

                    var user = await _userService.GetUserByEmail(userEmail);

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.Email),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(principal, properties);

                    TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";

                    return Redirect("/");
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        #endregion

        #region forgot password

        [HttpGet("forgot-pass")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-pass"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه ی عبور جدید برای شما ایمیل شد";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری ، کلمه ی عبور خود را تغییر دهید";

                        var GetUser =await _userService.RecoverUserPasswordForEmail(forgot.Email);

                        string body = _viewRender.RenderToStringAsync("_RecoverEmail", GetUser);

                        SendEmail.Send(forgot.Email, "بازیابی کلمه عبور", body);

                        return RedirectToAction("Login");
                }
            }

            return View(forgot);
        }

        #endregion
        #region log out

        [HttpGet("log-out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        #endregion

    }
}
