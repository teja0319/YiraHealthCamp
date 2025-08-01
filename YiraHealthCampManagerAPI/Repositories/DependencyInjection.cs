using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces;
using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Services;

namespace YiraHealthCampManagerAPI.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtCred = new JWTTokenConfigCreds();
            configuration.Bind("JWTTokenConfig", jwtCred);
            services.AddSingleton(jwtCred);


            #region DbContext & Identity Model
            //Identity Model Injection
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                //opt.Lockout.AllowedForNewUsers = true;
                //opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(24);
                //opt.Lockout.MaxFailedAccessAttempts = 5;
                //opt.Password.RequireDigit = true;
                //opt.Password.RequireLowercase = true;
                //opt.Password.RequireUppercase = true;
                //opt.Password.RequiredLength = 6;
                //opt.Password.RequireNonAlphanumeric = true;
                //opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<YiraDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(30));


            //Database Injection
            services.AddDbContext<YiraDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("YiraSqlConn"));
                opt.EnableSensitiveDataLogging();
            });

            // Register HttpClient
            //services.AddTransient<LoggingHandler>();
            //services.AddHttpClient("CustomLogger").AddHttpMessageHandler<LoggingHandler>();

            #endregion


            #region Repositories
            //Repositories Injection            
            //services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddScoped<IHealthCampRepository, HealthCampRepository>();


            #endregion

            #region 



            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IHealthCampService, HealthCampService>();



            #endregion

            //#region MessageHandlers


            //#endregion

          


            return services;
        }
    }
}