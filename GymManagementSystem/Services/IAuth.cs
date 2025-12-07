using GymManagementSystem.Models;

namespace Gym_Management_System.Services;

public interface IAuth
{
    string GenerateToken(AccountModel user);
    
}