using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.Services.Exceptions;

public class UnauthorizedException: Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
    
}