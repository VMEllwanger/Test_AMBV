using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Infrastructure.Services;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting application...");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            Log.Information("Builder created");

            builder.AddDefaultLogging();
            Log.Information("Logging configured");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            Log.Information("Controllers and API Explorer configured");

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();
            Log.Information("HealthChecks and Swagger configured");

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Log.Information("Connection string: {ConnectionString}", connectionString);

            builder.Services.AddDbContext<DefaultContext>(options =>
            {
                Log.Information("Configuring DbContext");
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                );
            });
            Log.Information("DbContext configured");

            builder.Services.AddJwtAuthentication(builder.Configuration);
            Log.Information("JWT authentication configured");

            builder.RegisterDependencies();
            Log.Information("Dependencies registered");

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);
            Log.Information("AutoMapper configured");

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });
            Log.Information("MediatR configured");

            builder.Services.AddScoped<IValidator<Application.Sales.UpdateSale.UpdateSaleCommand>, Application.Sales.UpdateSale.UpdateSaleValidator>();
            builder.Services.AddScoped<IValidator<Application.Sales.GetSale.GetSaleCommand>, Application.Sales.GetSale.GetSaleValidator>();
            builder.Services.AddScoped<IValidator<Application.Sales.GetAllSale.GetAllSaleCommand>, Application.Sales.GetAllSale.GetAllSaleValidator>();
            builder.Services.AddScoped<IValidator<Application.Sales.DeleteSale.DeleteSaleCommand>, Application.Sales.DeleteSale.DeleteSaleValidator>();
            builder.Services.AddScoped<IValidator<Application.Sales.CreateSale.CreateSaleCommand>, Application.Sales.CreateSale.CreateSaleValidator>();
            builder.Services.AddScoped<IValidator<Application.Sales.CancelSale.CancelSaleCommand>, Application.Sales.CancelSale.CancelSaleValidator>();
            builder.Services.AddScoped<IValidator<Application.Sales.CancelItem.CancelItemCommand>, Application.Sales.CancelItem.CancelItemValidator>();
            Log.Information("FluentValidation validators registered");

            builder.Services.AddScoped<ISaleEventPublisher, SaleEventPublisher>();
            Log.Information("Event publisher registered");

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            Log.Information("Validation pipeline configured");

            var app = builder.Build();
            Log.Information("Application built");

            app.UseMiddleware<ValidationExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            Log.Information("Middlewares configured");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                Log.Information("Swagger configured for development environment");
            }

            app.UseHttpsRedirection();
            Log.Information("HTTPS redirection configured");

            app.UseAuthentication();
            app.UseAuthorization();
            Log.Information("Authentication and authorization configured");

            app.UseBasicHealthChecks();
            Log.Information("HealthChecks configured");

            app.MapControllers();
            Log.Information("Controllers mapped");

            Log.Information("Starting application...");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Fatal error: {Message}", ex.Message);
            Log.Fatal(ex, "Stack Trace: {StackTrace}", ex.StackTrace);
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.Information("Finalizing application...");
            Log.CloseAndFlush();
        }
    }
}
