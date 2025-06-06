using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class ApplicationDbSeeder(
        IMultiTenantContextAccessor<ABCSchoolTeanantInfo> tenantInfoContextAccessor,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext)
    {
        private readonly IMultiTenantContextAccessor<ABCSchoolTeanantInfo> _multiTenantContextAccessor = tenantInfoContextAccessor;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task InitializeDatabasAsync(CancellationToken cancellationToken)
        {
            if (_applicationDbContext.Database.GetMigrations().Any())
            {
                if ((await _applicationDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    // If there are pending migrations, apply them
                    await _applicationDbContext.Database.MigrateAsync(cancellationToken);
                }

                // Seeding
                if (await _applicationDbContext.Database.CanConnectAsync(cancellationToken))
                {
                    // Default Roles, Assign permsssion/claims
                    await InitializeDefaultRolesAsync(cancellationToken);

                    // Create Users -> Assign roles to users
                    await InitializeAdminUserAsync(cancellationToken);
                }
            }
        }

        private async Task InitializeDefaultRolesAsync(CancellationToken cancellationToken)
        {

            foreach (var roleName in RoleConstants.DefaultRoles)
            {

                //Adding Role

                // Check if the default role already exists in the database if not there then incommingRole will be null so we are creating object
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken) is not ApplicationRole incomingRole)
                {
                    incomingRole = new ApplicationRole()
                    {
                        Name = roleName,
                        Description = $"{roleName} Role"
                    };

                    await _roleManager.CreateAsync(incomingRole);
                }

                //Now Role created , we can assign permissions/claims to the role

                if (roleName == RoleConstants.Basic)
                {
                    await AssignPermissionsToRoleAsync(SchoolPermissions.Basic, incomingRole, cancellationToken);
                }
                else if (roleName == RoleConstants.Admin)
                {
                    await AssignPermissionsToRoleAsync(SchoolPermissions.Admin, incomingRole, cancellationToken);

                    if (_multiTenantContextAccessor.MultiTenantContext.TenantInfo.Id == TenancyConstants.Root.Id)
                    {
                        await AssignPermissionsToRoleAsync(SchoolPermissions.Root, incomingRole, cancellationToken);
                    }
                }
            }
        }

        //Adding Default Permissions/Claims to the role
        protected async Task AssignPermissionsToRoleAsync(
            IReadOnlyList<SchoolPermission> incommingPermissions,
            ApplicationRole CurrentRole, CancellationToken cancellationToken)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(CurrentRole);

            foreach (var incommingPermission in incommingPermissions)
            {
                if (!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == incommingPermission.Name))
                {
                    await _applicationDbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
                    {
                        RoleId = CurrentRole.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = incommingPermission.Name,
                        Description = incommingPermission.Description,
                        Group = incommingPermission.Group,
                    }, cancellationToken);

                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        private async Task InitializeAdminUserAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_multiTenantContextAccessor.MultiTenantContext.TenantInfo.Email)) return;
            if (await _userManager.Users.SingleOrDefaultAsync(user => 
                user.Email == _multiTenantContextAccessor.MultiTenantContext.TenantInfo.Email)
                is not ApplicationUser incommingUser)
            {
                incommingUser = new ApplicationUser
                {
                    UserName = _multiTenantContextAccessor.MultiTenantContext.TenantInfo.Email,
                    Email = _multiTenantContextAccessor.MultiTenantContext.TenantInfo.Email,
                    FirstName = TenancyConstants.FirstName,
                    LastName = TenancyConstants.LastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _multiTenantContextAccessor.MultiTenantContext.TenantInfo.Email.ToUpperInvariant(),
                    NormalizedUserName = _multiTenantContextAccessor.MultiTenantContext.TenantInfo.Email.ToUpperInvariant(),
                    IsActive = true
                };

                var passwordHash = new PasswordHasher<ApplicationUser>();
                incommingUser.PasswordHash = passwordHash.HashPassword(incommingUser, TenancyConstants.DefaultPassword);

                await _userManager.CreateAsync(incommingUser);
            }
            // Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> will not add user to role if user is already in the role
            if (await _userManager.IsInRoleAsync(incommingUser, RoleConstants.Admin))
            {
                await _userManager.AddToRoleAsync(incommingUser, RoleConstants.Admin);
            }
        }
    }
}
