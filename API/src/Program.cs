using Scalar.AspNetCore;
using Serilog;

namespace src;

public abstract class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) => 
                loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Application terminated unexpectedly: \n" + exception.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}