using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tenancy
{
    /// <summary>
    /// Database Context class for multi-tenant applications.
    /// </summary>
    public class TenantDbContext(DbContextOptions<TenantDbContext> options) 
        : EFCoreStoreDbContext<ABCSchoolTeanantInfo>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // You can add additional configurations here if needed

            modelBuilder.Entity<ABCSchoolTeanantInfo>().ToTable("Tenants", "Multitenancy"); // Specify the table name if different
        }
    }
}
