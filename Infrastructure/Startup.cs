using Finbuckle.MultiTenant;
using Infrastructure.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// House Dependency Injection Sub-Grouping within the Infrastructure Layer.
    /// 
    /// Note: The actual dependency is inside Program.cs in WebIAPI project,
    /// from there we will be calling extension methods defining here.
    /// </summary>
    public static class Startup
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Register your infrastructure services here
            // For example, you can add database context, repositories, etc.

            // Example: services.AddDbContext<TenantDbContext>(options => ...);
            // Example: services.AddScoped<ITenantRepository, TenantRepository>();

            return services
                .AddDbContext<TenantDbContext>(options => 
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection"))) //
                .AddMultiTenant<ABCSchoolTeanantInfo>() //Add MultiTenant support with a header strategy
                    .WithHeaderStrategy(TenancyConstants.TenantIdName) // Tenant Header: When the user logged in first time, we expect them to specify tenant in header
                    .WithClaimStrategy(TenancyConstants.TenantIdName) // Tenant Claim: When the user logged in, we expect them to have a claim Key:  "tenant" which will be used to identify tenant
                    .WithEFCoreStore<TenantDbContext, ABCSchoolTeanantInfo>() // 
                    .Services
                .AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(config.GetConnectionString("DefaultConnection")));
        }

        /// <summary>
        ///  Including middleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            // Configure your middleware here
            // For example, you can use app.UseRouting(), app.UseEndpoints(), etc.
            // Example: app.UseAuthentication();
            // Example: app.UseAuthorization();


            return app.UseMultiTenant(); // Without these cannot use MultiTenant feature in the application
        }
    }
}
