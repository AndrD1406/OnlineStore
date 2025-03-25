using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Models;
using OnlineStore.DataAccess.Repository.Base;

namespace OnlineStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped(typeof(IEntityRepository<,>), typeof(EntityRepository<,>));
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddTransient<IPurchaseService, PurchaseService>();
            builder.Services.AddDbContext<OnlineStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            // Enable identity 
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<OnlineStoreDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, OnlineStoreDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, OnlineStoreDbContext, Guid>>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Home/Login";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromHours(Convert.ToDouble(builder.Configuration["Expires"]));
                options.SlidingExpiration = false;
            });
            
            builder.Services.AddAuthorization(auth =>
            {
                auth.AddPolicy("CookieAuthorization", policy => policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme).RequireAuthenticatedUser());
                auth.DefaultPolicy = auth.GetPolicy("CookieAuthorization");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
