using System.Reflection;
using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniCommerce.API.Abstractions.Behaviours;
using MiniCommerce.API.Abstractions.Handlers;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Extensions;
using MiniCommerce.API.Middlewares;
using MiniCommerce.API.Repositories;
using Serilog;

namespace MiniCommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add this line to register MediatR services
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, 
            includeInternalTypes: true);
        
        return services;
    }
    
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // <-- Register Db Context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // <-- Register the lifetimes of repositories
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        // <-- Register Unit Of Work
        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<ApplicationDbContext>());

        // <-- Register Email Handler
        services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailHandler, EmailHandler>();
        
        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "MiniCommerce.API",
                Description = "MiniCommerce.API build on ASP.NET Core Web API 8",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact"),
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license"),
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });
            
            options.AddSecurityRequirement(new  OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    
                    new string[]{}
                }
            });
        });

        // <-- Register CORS
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });
        });
        
        return services;
    }

    public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IHostBuilder hostBuilder)
    {
        // <-- Register Serilog to read appsettings.json
        hostBuilder.UseSerilog((context, logger) =>
        {
            logger.ReadFrom.Configuration(context.Configuration);
        });
        
        return services;
    }

    public static IServiceCollection AddMiddlewareExtension(this IServiceCollection services)
    {
        // <-- Register Middlewares
        services.AddScoped<RequestContextLoggingMiddleware>();
        services.AddExceptionHandler<ExceptionHandlerMiddleware>();
        services.AddProblemDetails();

        // <-- Register Extensions
        services.AddScoped<IMigrationExtension, MigrationExtension>();
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var issuer = configuration["JwtSettings:Issuer"]!;
        var audience = configuration["JwtSettings:Audience"]!;
        var key = configuration["JwtSettings:Key"]!;
        
        services.AddScoped<IJwtTokenHandler, JwtTokenHandler>(_ => 
            new JwtTokenHandler(issuer, audience, key));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }). AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; // In production change {true}
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key))
            };
        });
        
        services.AddAuthorization();
        
        return services;
    }
}