using Microsoft.AspNetCore.Identity;
using WalletAPI.Context;
using WalletAPI.Entities;
using WalletAPI.Interfaces;
using WalletAPI.Services;

namespace WalletAPI.ServiceCollections
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAccountServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService,AccountService>();

            #region Identity
            services.AddIdentityCore<User>();
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Other Identity options.....
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Override identity password rule
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM-._@+";

            });
            #endregion

            return services;
        }
    }
}
