namespace DieffeClean.Domain.Constants;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string CleaningUser = "CleaningUser";
}

public static class RolesName
{
    public static readonly Dictionary<string, string> RoleName = new Dictionary<string, string>()
    {
        { "SuperAdmin", "SuperAdmin" }, { "Admin", "Admin" }, { "CleaningUser", "User" }
    };
}