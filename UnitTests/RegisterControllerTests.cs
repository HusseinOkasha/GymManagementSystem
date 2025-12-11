using FluentAssertions;
using Gym_Management_System.Services;
using GymManagementSystem.Controllers;
using GymManagementSystem.Dtos;
using GymManagementSystem.Models;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

public class RegisterControllerTests
{
    private readonly Mock<IRegisterService> _mockRegisterService;
    private readonly RegisterController _controller;
    private readonly Mock<IAuth> _mockAuthService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<ILogger<RegisterController>> _mockLogger;


    public RegisterControllerTests()
    {
        _mockRegisterService = new Mock<IRegisterService>();
        _mockAuthService = new Mock<IAuth>();
        _mockEmailService = new Mock<IEmailService>();
        _mockLogger = new Mock<ILogger<RegisterController>>();
        _controller = new RegisterController(_mockLogger.Object, _mockAuthService.Object, _mockEmailService.Object,
            _mockRegisterService.Object);
    }

    [Fact]
    public async Task Should_SendInvitation()
    {
        // Arrange
        AccountModel user = new AccountModel { Email = "e1@email.com" };
        _mockAuthService.Setup(s => s.GenerateToken(It.IsAny<AccountModel>())).Returns("Token");
        
        // Act
        InvitationDto dto = new InvitationDto() { Email = user.Email };
        var result = await _controller.SendInvitation(dto);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task ShouldNot_SendInvitation()
    {
        // Arrange
        AccountModel user = new AccountModel { Email = "e1email.com" };
        _mockAuthService.Setup(s => s.GenerateToken(It.IsAny<AccountModel>())).Returns("Token");
        _controller.ModelState.AddModelError("Email", "Email is required");
        
        // Act
        InvitationDto dto = new InvitationDto() { Email = user.Email };
        var result = await _controller.SendInvitation(dto);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CompleteInvitation_ValidEmail_ReturnOK()
    {
        // Arrange
        CompleteInvitationDto dto = new CompleteInvitationDto { Email = "e1@email.com" };
        _mockRegisterService.Setup(s => s.CompleteInvitation(It.IsAny<CompleteInvitationDto>())).ReturnsAsync("Token");
        
        // Act
        var result = await _controller.CompleteInvitation(dto);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task CompleteInvitation_InValidEmail_ReturnBadRequest()
    {
        // Arrange
        CompleteInvitationDto dto = new CompleteInvitationDto { Email = "e1email.com" };
        _mockRegisterService.Setup(s => s.CompleteInvitation(It.IsAny<CompleteInvitationDto>())).ReturnsAsync("Token");
        _controller.ModelState.AddModelError("Email", "Email is required");
        
        // Act
        var result = await _controller.CompleteInvitation(dto);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    
    
}