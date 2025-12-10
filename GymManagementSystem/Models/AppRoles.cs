using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.Models;

public class AppRoles : IdentityRole
{
    public const string Admin = "Admin";
    public const string Employee = "Employee";
    public const string Client = "Client";
}