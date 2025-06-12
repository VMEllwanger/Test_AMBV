using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
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
            Console.WriteLine("Iniciando a aplicação...");
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            Console.WriteLine("Builder criado...");

            builder.AddDefaultLogging();
            Console.WriteLine("Logging configurado...");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            Console.WriteLine("Controllers e API Explorer configurados...");

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();
            Console.WriteLine("HealthChecks e Swagger configurados...");

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"String de conexão: {connectionString}");

            builder.Services.AddDbContext<DefaultContext>(options =>
            {
                Console.WriteLine("Configurando DbContext...");
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                );
            });
            Console.WriteLine("DbContext configurado...");

            builder.Services.AddJwtAuthentication(builder.Configuration);
            Console.WriteLine("Autenticação JWT configurada...");

            builder.RegisterDependencies();
            Console.WriteLine("Dependências registradas...");

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);
            Console.WriteLine("AutoMapper configurado...");

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });
            Console.WriteLine("MediatR configurado...");

            // Adicionando o registro dos validadores do FluentValidation
            builder.Services.AddScoped<IValidator<Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.UpdateSaleCommand>, Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.UpdateSaleValidator>();
            builder.Services.AddScoped<IValidator<Ambev.DeveloperEvaluation.Application.Sales.GetSale.GetSaleCommand>, Ambev.DeveloperEvaluation.Application.Sales.GetSale.GetSaleValidator>();
            builder.Services.AddScoped<IValidator<Ambev.DeveloperEvaluation.Application.Sales.GetAllSale.GetAllSaleCommand>, Ambev.DeveloperEvaluation.Application.Sales.GetAllSale.GetAllSaleValidator>();
            builder.Services.AddScoped<IValidator<Ambev.DeveloperEvaluation.Application.Sales.DeleteSale.DeleteSaleCommand>, Ambev.DeveloperEvaluation.Application.Sales.DeleteSale.DeleteSaleValidator>();
            builder.Services.AddScoped<IValidator<Ambev.DeveloperEvaluation.Application.Sales.CreateSale.CreateSaleCommand>, Ambev.DeveloperEvaluation.Application.Sales.CreateSale.CreateSaleValidator>();
            Console.WriteLine("Validadores do FluentValidation registrados...");

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            Console.WriteLine("Pipeline de validação configurado...");

            var app = builder.Build();
            Console.WriteLine("Aplicação construída...");

            app.UseMiddleware<ValidationExceptionMiddleware>();
            Console.WriteLine("Middleware de validação configurado...");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                Console.WriteLine("Swagger configurado para ambiente de desenvolvimento...");
            }

            app.UseHttpsRedirection();
            Console.WriteLine("Redirecionamento HTTPS configurado...");

            app.UseAuthentication();
            app.UseAuthorization();
            Console.WriteLine("Autenticação e autorização configuradas...");

            app.UseBasicHealthChecks();
            Console.WriteLine("HealthChecks configurado...");

            app.MapControllers();
            Console.WriteLine("Controllers mapeados...");

            Console.WriteLine("Iniciando a aplicação...");
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro fatal: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Console.WriteLine("Finalizando a aplicação...");
            Log.CloseAndFlush();
        }
    }
}
