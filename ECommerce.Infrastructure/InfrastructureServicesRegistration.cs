using ECommerce.Application.Contracts;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.DataSeeding;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Services;
using ECommerce.Infrastructure.Payments;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
            );

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            }
            );


            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddAuthentication(
                opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>

                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:7220",
                    ValidateAudience = true,
                    ValidAudience = "MyApp",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MYSECUirtyKeyForAuthENTICATIONMYSECUirtyKeyForAuthENTICATIONMYSECUirtyKeyForAuthENTICATION"))
                });



            services.AddSingleton<ICacheRepository, CacheRepository>();
            //Remember: scoped = per request, once done it is deleted.
            //singleton = one object through application lifetime, deleted once application closes.

            //We can't use scoped here because we need the connection to stay open otherwise the data will be deleted with it
            //normally in Web apps they shouldn't ever go offline, if they do then all the data in the In memory database
            //will be deleted, however this normally doesn't happen so we can use a single object of the connection 
            //throughout the application's lifetime

            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });


            //We use AddSingleton with the payment gateway because the secret key is 'static'
            //meaning that it's created only once during application startup
            //so we only need one object throughout our app
            services.AddSingleton<IPaymentGateway, StripePaymentGateway>();

            return services;
        }
    }
}
