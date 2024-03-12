using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Abstraction;
using PCShop.Data;
using PCShop.Entities;
using PCShop.Infrastructure;
using PCShop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>()
                  .AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<AppDbContext>()
                  .AddDefaultTokenProviders();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Users/Login";
    options.LogoutPath = "/Users/Logout";

    options.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = redirectContext =>
        {
            string redirectUri = redirectContext.RedirectUri;

            UriHelper.FromAbsolute(
                redirectUri,
                out string scheme,
                out HostString host,
                out PathString path,
                out QueryString query,
                out FragmentString fragment);

            redirectUri = UriHelper.BuildAbsolute(scheme, host, path);

            redirectContext.Response.Redirect(redirectUri);

            return Task.CompletedTask;
        }
    };
});


builder.Services.Configure<IdentityOptions>(option =>
{
    option.SignIn.RequireConfirmedEmail = false;
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 5;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredUniqueChars = 0;
});

var app = builder.Build();
app.PrepareDatabase().Wait();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
