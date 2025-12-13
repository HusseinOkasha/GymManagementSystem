using Gym_Management_System.Services;
using GymManagementSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymManagementSystem.Controllers;

public class AuthController(IAuth _authService): ControllerBase
{
    [HttpPost]
    [Route("/api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok( await _authService.Login(dto));
    }
}