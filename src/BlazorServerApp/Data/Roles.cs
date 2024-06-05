namespace BlazorServerApp.Data;

public static class Roles
{
    public const string Admin = nameof(Admin);

    public static readonly string[] All = new[]
    {
        Admin
    };
}