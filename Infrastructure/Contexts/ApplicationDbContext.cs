using Domain.Entities;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(
            IMultiTenantContextAccessor<ABCSchoolTeanantInfo> tenantContextAccessor,
            DbContextOptions options) 
            : base(tenantContextAccessor, options)
        {
        }

        // Everytime we are using school then it will give us school entity
        public DbSet<School> Schools => Set<School>();
    }
}
