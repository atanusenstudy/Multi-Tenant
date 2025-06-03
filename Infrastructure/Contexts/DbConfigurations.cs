using Domain.Entities;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts
{
    internal class DbConfigurations
    {
        /// <summary>
        /// For ApplicationUser,ApplicationRole we are explicitlly defining the table name and schema
        /// because by default Identity will create tables in the "dbo" schema and name will be Asp.net.....
        /// 
        /// Note : We are naming Database table in plural for this project
        /// 
        /// Note: IsMultiTenant() is an extension method provided by Finbuckle.MultiTenant package
        /// </summary>
        internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
                builder.ToTable("Users", "Identity")
                .IsMultiTenant(); // This will add TenantId column to the table and also configure the entity to be multi-tenant aware
            }
        }

        internal class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
        {
            public void Configure(EntityTypeBuilder<ApplicationRole> builder)
            {
                builder.ToTable("Roles", "Identity")
                .IsMultiTenant();
            }
        }

        // No need to specify 'string' as it's specified in ApplicationRoleClaim which inherited IdentityRoleClaim<string>.
        internal class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
        {
            public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
            {
                builder.ToTable("RoleClaims", "Identity")
                .IsMultiTenant();
            }
        }

        internal class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
        {
            public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
            {
                builder.ToTable("UserRoles", "Identity")
                .IsMultiTenant();
            }
        }

        internal class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
        {
            public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
            {
                builder.ToTable("UserClaims", "Identity")
                .IsMultiTenant();
            }
        }

        internal class IdentityUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
        {
            public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
            {
                builder.ToTable("UserLogins", "Identity")
                .IsMultiTenant();
            }
        }

        internal class IdentityUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
        {
            public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
            {
                builder.ToTable("UserTokens", "Identity")
                .IsMultiTenant();
            }
        }

        internal class SchoolConfig : IEntityTypeConfiguration<School>
        {
            public void Configure(EntityTypeBuilder<School> builder)
            {
                builder.ToTable("Schools", "Identity")
                    .IsMultiTenant();

                builder
                    .Property(School => School.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            }
        }
    }
}
