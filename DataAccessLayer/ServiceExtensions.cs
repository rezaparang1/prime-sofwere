using DataAccessLayer.Interface.Customer_Club;
using DataAccessLayer.Repository.Customer_Club;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace DataAccessLayer
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // ثبت DbContext
            services.AddDbContext<DataAccessLayer.Database>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(typeof(DataAccessLayer.Database).Assembly.FullName);

                        // ✅ ارسال سه پارامتر (errorCodesToAdd = null)
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);

                        npgsqlOptions.CommandTimeout(30);
                    });

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IClubDiscountRepository, ClubDiscountRepository>();
            services.AddScoped<IPublicDiscountRepository, PublicDiscountRepository>();

            return services;
        }
    }
}
