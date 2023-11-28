using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using MarcketAppliction.Services.Implementations;
using MarcketAppliction.Services.Interface;
using MarcketDataLayer.Repository;
using MarcketWeb.cash;
using MarketPlace.DataLayer.Context;
using MarketPlace.DataLayer.Repository;
using MarketPlaceWeb.Auth;
using MarketPlaceWeb.EmailRenderView;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile("MYLog/logtext.txt");

// Add services to the container.

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<MarketDBContext>()
                  .AddDefaultTokenProviders();


IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

#region config database

builder.Services.AddDbContext<MarketDBContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("MarketPlaceConnection"));
});

#endregion

builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericReopsitory<>));
builder.Services.AddScoped<IUserServicecs, UserService>();

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISellerWalletService, SellerWalletService>();
builder.Services.AddScoped<IProductDiscountService, ProductDiscountService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IArticlesService, ArticlesService>();
builder.Services.AddScoped<DapperUtility>();

builder.Services.AddSingleton<IViewRenderService, RenderViewToString>();




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/log-out";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
    //options.AccessDeniedPath = "/AccessDenied";
}).AddGoogle(option =>
{

    option.ClientId = "771196186909-mknug4qs44eghq1kr8ts8hai43o7rfv1.apps.googleusercontent.com";

    option.ClientSecret = "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";


});








#region html encoder

builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

#endregion



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/ErrorPage");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
