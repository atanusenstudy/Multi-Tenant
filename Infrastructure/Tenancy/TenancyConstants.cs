namespace Infrastructure.Tenancy
{
    public class TenancyConstants
    {
        public const string TenantIdName = "tenant";
        public const string DefaultPassword = "Admin@1234";
        public const string FirstName = "Root";
        public const string LastName = "Admin";

        public static class Root
        {
            public const string Id = "root";
            public const string Name = "root";
            public const string Email = "admin.root@abcschool.com";
        }
    }
}
