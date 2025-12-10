using System.ComponentModel.DataAnnotations;
using GymManagementSystem.Enums;

namespace GymManagementSystem.Dtos;

public class CompleteInvitationDto()
{
    public int Id { get; set; }
    public int BranchId { get; set; }

    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone Number is required")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(Role))]
    public string Role { get; set; }
}