using EnsekTest.Application.Interface;
using EnsekTest.Application.Services;
using EnsekTest.Persistence.Context;
using EnsekTest.Persistence.Data;
using EnsekTest.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Persistence.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<EnsekDbContext>(opt =>
            {
                //opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<EnsekDbContext>(options =>
    options.UseSqlite("Data Source=Ensek.db"));


            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
            services.AddScoped<MeterReadingValidationService>();
            services.AddScoped<IMeterReadingService,MeterReadingService>();
            services.AddScoped<ICsvParserService,CsvParserService>();
            services.AddScoped<IDatabaseSeederService, DatabaseSeederService>();
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }
    }
}
