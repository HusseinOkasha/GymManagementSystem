using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Dtos;

public class InvitationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}