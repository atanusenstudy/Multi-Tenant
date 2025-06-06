
using Infrastructure;

namespace WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Dependency Injection
            builder.Services.AddInfrastructureServices(builder.Configuration); // Registering Infrastructure Layer Services

            
            #endregion
            var app = builder.Build();

            //Database Seeder
            await app.Services.AddDatabaseInitializerAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            #region Middleware
            app.UseInfrastructure(); // Using Infrastructure Layer Middleware

            #endregion

            app.MapControllers();

            app.Run();
        }
    }
}
