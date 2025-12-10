using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.Models;

public class AccountModel : IdentityUser
{
    public int BranchId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}