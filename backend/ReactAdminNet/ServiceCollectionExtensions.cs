
namespace ReactAdminNet;

using System.Data;
using System.Reflection;
using System.Text;
using Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Retail.DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRetailServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("reactadmin_db");
        services.AddTransient<IDbConnection, MySqlConnection>(sp => new MySqlConnection(connectionString));
        services.AddTransient<IRepository, Repository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICommandRepository, CommandRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IInvoiceRepository, InvoiceRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IReviewRepository, ReviewRepository>();

        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(AuthController).GetTypeInfo().Assembly);

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        static void CorsPolicy(CorsPolicyBuilder builder)
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }

        services.AddCors(options => { options.AddPolicy("AllowOrigin", CorsPolicy); });

        return services;
    }
}

