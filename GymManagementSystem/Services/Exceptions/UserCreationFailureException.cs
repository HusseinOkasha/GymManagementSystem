using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.Services.Exceptions;

public class UserCreationFailureException : Exception
{   
    public IdentityResult identityResult {get; set; }
    public UserCreationFailureException(IdentityResult result) : base(string.Join(",",
        result.Errors.Select(error => error.Description)))
    {
        identityResult = result;
    }
    
}