
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tenancy
{
    public class TenantDbSeeder : ITenantDbSeeder
    {
        private readonly TenantDbContext _tenantDbContext;
        private readonly IServiceProvider _serviceProvider;


        public TenantDbSeeder(TenantDbContext tenantDbContext, IServiceProvider serviceProvider)
        {
            _tenantDbContext = tenantDbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
        {
            // First Tenant must be created
            await InitializeDatabaseWithTenant(cancellationToken);

            // Then Initialize the database migrations
            foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync())
            {
                //Application DbSeeder
                await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
            }
        }

        private async Task InitializeDatabaseWithTenant(CancellationToken cancellationToken)
        {
            if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], cancellationToken) is null)
            {
                // Create the root tenant if it does not exist
                var rootTenant = new ABCSchoolTeanantInfo
                {
                    // ConnectionString is default connection string
                    Id = TenancyConstants.Root.Id,
                    Identifier = TenancyConstants.Root.Id,
                    Name = TenancyConstants.Root.Name,
                    Email = TenancyConstants.Root.Email,
                    FirstName = TenancyConstants.FirstName,
                    LastName = TenancyConstants.LastName,
                    IsActive = true,
                    ValidUpTo = DateTime.UtcNow.AddYears(2)
                };

                await _tenantDbContext.TenantInfo.AddAsync(rootTenant, cancellationToken);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task InitializeApplicationDbForTenantAsync(ABCSchoolTeanantInfo tenant, CancellationToken cancellationToken)
        {
            // This method can be used to initialize the application database for the tenant
            // For example, you can create default roles, users, etc. for the tenant
            // This is just a placeholder method and can be implemented as per your requirements

            using var scope = _serviceProvider.CreateScope();
            _serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
                .MultiTenantContext = new MultiTenantContext<ABCSchoolTeanantInfo>()
                {
                    TenantInfo = tenant
                };
            await scope.ServiceProvider
                .GetRequiredService<ApplicationDbSeeder>()
                .InitializeDatabasAsync(cancellationToken);
        }
    }
}
