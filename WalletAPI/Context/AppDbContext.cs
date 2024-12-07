using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Entities;

namespace WalletAPI.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.ToTable("Users");
            });


            builder.Entity<IdentityUserRole<string>>(u =>
            {
                u.ToTable("UsersRoles");
            });


            builder.Entity<IdentityRole>(u =>
            {
                u.ToTable("Roles");
            });


            builder.Entity<IdentityUserLogin<string>>(u =>
            {
                u.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserClaim<string>>(u =>
            {
                u.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserToken<string>>(u =>
            {
                u.ToTable("UserTokens");
            });


            builder.Entity<IdentityRoleClaim<string>>(u =>
            {
                u.ToTable("RoleClaims");
            });

        }
    }
}
