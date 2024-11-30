
using WalletAPI.Context;
using Microsoft.EntityFrameworkCore;
using WalletAPI.ServiceCollections;

namespace WalletAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureAccountServices(builder.Configuration);
            var ConnectionString = builder.Configuration.GetConnectionString("sqlConnection");

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(ConnectionString,options => options.EnableRetryOnFailure()));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
