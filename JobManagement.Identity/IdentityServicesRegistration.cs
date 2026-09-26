using JobManagement.Application.Contracts.Identity;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.Models.Identity;
using JobManagement.Identity.DbContext;
using JobManagement.Identity.Models;
using JobManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JobManagement.Identity
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<FrontendSettings>(configuration.GetSection("Frontend"));

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("JobManagementDbConnectionString"))
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();


            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (configuration["JwtSettings:Key"]))
                };

                option.Events = new JwtBearerEvents
                {
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var endpoint = context.HttpContext.GetEndpoint();

                        var authorizeData = endpoint?
                            .Metadata
                            .GetOrderedMetadata<IAuthorizeData>();

                        var requiredRoles = authorizeData?
                            .Where(x => !string.IsNullOrWhiteSpace(x.Roles))
                            .SelectMany(x => x.Roles!.Split(','))
                            .Select(x => x.Trim())
                            .Distinct()
                            .ToList();

                        var message = requiredRoles?.Any() == true
                            ? $"You must have the {string.Join(" or ", requiredRoles)} role to perform this action."
                            : "You do not have permission to perform this action.";

                        await context.Response.WriteAsJsonAsync(new
                        {
                            statusCode = 403,
                            message
                        });
                    }
                };
            });        

            return services;
        }
    }
}
