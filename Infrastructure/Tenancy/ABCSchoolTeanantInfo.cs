using Finbuckle.MultiTenant.Abstractions;

namespace Infrastructure.Tenancy
{
    /// <summary>
    /// This will represent Database tenant info
    /// Tenant represent calss
    /// 
    /// We well be giving tenant to use their own database, and also allow tenant to use our default database.
    /// </summary>
    public class ABCSchoolTeanantInfo : ITenantInfo
    {
        #region Default Values from ITenantInfo
        public string Id { get; set; } //Primary Key in the database
        public string Identifier { get; set; } //Primary Key in the database
        public string Name { get; set; }
        #endregion

        #region Our Custom Properties
        public string ConnectionString { get; set; } //Connection string to the tenant database
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ValidUpTo { get; set; } //This is the date when tenant subscription will expire
        public bool IsActive { get; set; } //This is the status of the tenant, if tenant is active or not
        #endregion
    }
}
