using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Contexts
{
    public abstract class BaseDbContext
        : MultiTenantIdentityDbContext
        <
            ApplicationUser,
            ApplicationRole,
            string,
            IdentityUserClaim<string>,
            IdentityUserRole<string>,
            IdentityUserLogin<string>,
            ApplicationRoleClaim,
            IdentityUserToken<string>
        >
    {
        /*
         * Hiding the TenantInfo(Because we have added our own property) property to avoid confusion with the base class MultiTenantIdentityDbContext
         * Which contains a generic TenantInfo property and we need to add functionality specific to out tenant 
         * like ConnectionString etc
         */
        private new ABCSchoolTeanantInfo TenantInfo { get; set; }
        protected BaseDbContext(IMultiTenantContextAccessor<ABCSchoolTeanantInfo> tenantContextAccessor, DbContextOptions options)
            : base(tenantContextAccessor, options)
        {
            // Ensure that my TenantInfo is hiding the base class TenantInfo
            TenantInfo = tenantContextAccessor.MultiTenantContext.TenantInfo;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Here we can configure the database connection string if needed, or other options
            // For example: optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
            if (!string.IsNullOrEmpty(TenantInfo?.ConnectionString))
            {
                optionsBuilder.UseSqlServer(TenantInfo.ConnectionString, option =>
                {
                    option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); // Specify the assembly where migrations are located
                });
            }
            else
            {
                // Fallback to a default connection string or throw an exception
            }
        }

        // Here we can configure the model as needed, for example, adding custom configurations
        // modelBuilder.Entity<ApplicationUser>().Property(u => u.FirstName).IsRequired(); // Example of custom configuration
        // You can also apply configurations from assemblies if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Applies configuration from all IEntityTypeConfiguration instances that are defined in provided assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
