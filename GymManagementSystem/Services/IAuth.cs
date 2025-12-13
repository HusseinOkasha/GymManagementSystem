using GymManagementSystem.Dtos;
using GymManagementSystem.Models;

namespace Gym_Management_System.Services;

public interface IAuth
{
    string GenerateToken(AccountModel user, IList<string> roles);
    Task<string> Login(LoginDto dto);
}