namespace GymManagementSystem.Services.Exceptions;

public class AssigningUserRoleException : Exception
{
    public AssigningUserRoleException(string message) : base(message)
    {
    }

    public AssigningUserRoleException(string message, Exception innerException) : base(message, innerException)
    {
    }
}