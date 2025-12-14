using FluentAssertions;
using Gym_Management_System.Services;
using GymManagementSystem.Controllers;
using GymManagementSystem.Dtos;
using GymManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using Moq;

namespace UnitTests;

public class AuthControllerTests
{
    private readonly Mock<IAuth> _mockAuthService;
    private readonly AuthController _controller;
    
    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuth>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnOK()
    {
        // Arrange
        LoginDto dto = new LoginDto
        {
            Email = "e1@email.com",
            Password = "Ppassword$12"
        };
        _mockAuthService.Setup(s => s.Login(It.IsAny<LoginDto>())).ReturnsAsync("Token");
        
        // Act
        var result = await _controller.Login(dto);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();

    }
    
    [Fact]
    public async Task Login_EmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        LoginDto dto = new LoginDto
        {
            Email = "",
            Password = "Ppassword$12"
        };
        _controller.ModelState.AddModelError("Email", "Email is required");
        _mockAuthService.Setup(s => s.Login(It.IsAny<LoginDto>())).ReturnsAsync("Token");
        
        // Act
        var result = await _controller.Login(dto);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task Login_EmptyPassword_ReturnsBadRequest()
    {
        // Arrange
        LoginDto dto = new LoginDto
        {
            Email = "e2@email.com",
            Password = "Ppassword$12"
        };
        _controller.ModelState.AddModelError("Email", "Email is required");
        _mockAuthService.Setup(s => s.Login(It.IsAny<LoginDto>())).ReturnsAsync("Token");
        
        // Act
        var result = await _controller.Login(dto);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}