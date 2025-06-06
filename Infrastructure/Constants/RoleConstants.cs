using System.Collections.ObjectModel;

namespace Infrastructure.Constants
{
    public static class RoleConstants
    {
        public const string Admin = nameof(Admin);
        public const string Basic = nameof(Basic);

        //System Default Role and we do not want it to be deleted, later according to requirement we can add in db.
        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(
            [
                Admin,
                Basic
            ]);

        public static bool IsDefaultRole(string roleName) => DefaultRoles.Contains(roleName);
    }
}
