using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Behaviours;
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
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        
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

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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

        // <-- Register Extensions
        services.AddScoped<IMigrationExtension, MigrationExtension>();
        
        return services;
    }
}