using GymManagementSystem.Dtos;
using GymManagementSystem.Models;
using Gym_Management_System.Services;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers;

[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IAuth _authService;
    private readonly IEmailService _emailService;
    private readonly IRegisterService _registerService;
    private readonly ILogger<RegisterController> _logger;

    public RegisterController(ILogger<RegisterController> logger, IAuth authService, IEmailService emailService,
        IRegisterService registerService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _registerService = registerService;
    }

    [HttpGet]
    [Route("/invite")]
    public async Task<IActionResult> SendInvitation([FromQuery] InvitationDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = new AccountModel
        {
            Email = dto.Email
        };
        var token = _authService.GenerateToken(user);
        var url = $"https://localhost:7252/complete-registration?token={token}";
        //await _emailService.SendEmailAsync(invitation.Email, "invitation", url);
        return Ok(url);
    }

    [HttpPost]
    [Authorize]
    [Route("/complete-invitation")]
    public async Task<IActionResult> CompleteInvitation([FromBody] CompleteInvitationDto dto)
    {
        // Check the validation of the dto
        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(await _registerService.CompleteInvitation(dto));
    }
}