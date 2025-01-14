using AdminPanelWithApi.Helpers.CompressImage;
using AdminPanelWithApi.Helpers.Image;
using Application.Behaviors;
using Auth.Middlewares;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Seeds;
using External.Infrastructure.Persistence.Data;
using FluentValidation;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Web.DependencyInjection;
namespace Auth.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
            services.AddMediatR(applicationAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddAutoMapper(applicationAssembly);
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
             
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); 
            services.AddTransient<CustomExceptionHandlerMiddleware>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigration>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, SwaggerUIConfiguration>();
            
            services.AddTransient<IImageHelper, ImageHelper>();
            return services;
        }
         
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }

        public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
             
            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JwtSetting:Issuer"],
                    ValidAudience = configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSetting:Key"] ?? ""))
                };
            });

            return services;
        }
        public static async Task<bool> AddSeedAsync(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await ContextSeed.Seed(userManager, roleManager);
              

                return true;
            }
            return false;
        }
      

    }
}
