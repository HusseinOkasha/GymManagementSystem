using Gym_Management_System.Services;
using GymManagementSystem.Dtos;
using GymManagementSystem.Models;
using GymManagementSystem.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.Services;

public class RegisterService : IRegisterService
{
    private UserManager<AccountModel> _userManager;
    private IAuth _authService;

    public RegisterService(UserManager<AccountModel> userManager, IAuth authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<string> CompleteInvitation(CompleteInvitationDto dto)
    {
        // Create Account Model from the dto
        var user = new AccountModel
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber
        };

        // Save the user to the database
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) throw new UserCreationFailureException(result);

        // Assign role to the user 
        var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
        if (!roleResult.Succeeded) throw new AssigningUserRoleException(roleResult.Errors.ToString() ?? "");

        // Generate token for the user 
        var token = _authService.GenerateToken(user, new List<string>(){dto.Role});
        return token;
    }
}