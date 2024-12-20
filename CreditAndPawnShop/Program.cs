
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CAPS.DataModels;
using CAPS.Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using CAPS.Data;
using CAPS.Services;
using CAPS.Data.Data;


namespace CreditAndPawnShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            //});

            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<ILoanService, LoanService>();
            builder.Services.AddScoped<IPawnShopAdminService, PawnShopAdminService>();
            builder.Services.AddScoped<IPawnShopService, PawnShopService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

                       

            var app = builder.Build();

            using (var data = app.Services.CreateScope())
            {
                var services = data.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var context = services.GetRequiredService<ApplicationDbContext>();

                await Seeder.SeedDBAsync(context, userManager);
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var context = services.GetRequiredService<ApplicationDbContext>();



                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await AdminRoleSeeder.SeedAdminRoleAsync(roleManager);
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await AdminRoleSeeder.SeedUserRoleAsync(roleManager);
                }


                if (!await userManager.Users.AnyAsync(u => u.UserName == "admin@admin.com"))
                {
                    await AdminRoleSeeder.SeedAdminUserAsync(userManager, roleManager);
                }

               

            }
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
            app.MapRazorPages();

            app.Run();

           
        }


    }
}