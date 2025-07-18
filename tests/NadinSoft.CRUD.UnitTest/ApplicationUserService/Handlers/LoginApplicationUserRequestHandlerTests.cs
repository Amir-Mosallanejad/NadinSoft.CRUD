using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Handlers;

public class LoginApplicationUserRequestHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IJwtTokenGenerator> _jwtMock = new();
    private readonly Mock<ILogger<LoginApplicationUserRequestHandler>> _loggerMock = new();

    private readonly LoginApplicationUserRequestHandler _handler;

    public LoginApplicationUserRequestHandlerTests()
    {
        Mock<IUserStore<ApplicationUser>> store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _handler = new LoginApplicationUserRequestHandler(
            _userManagerMock.Object,
            _jwtMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Fail_When_User_Not_Found()
    {
        // Arrange
        LoginApplicationUserRequest request = new LoginApplicationUserRequest("user@email.com", "Password123");

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Should_Return_Fail_When_Password_Is_Invalid()
    {
        ApplicationUser user = new ApplicationUser { Email = "user@email.com" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "wrongpass")).ReturnsAsync(false);

        LoginApplicationUserRequest request = new LoginApplicationUserRequest(user.Email!, "wrongpass");

        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Should_Return_Success_When_Credentials_Are_Valid()
    {
        ApplicationUser user = new ApplicationUser { Email = "user@email.com" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "correctpass")).ReturnsAsync(true);
        _jwtMock.Setup(x => x.GenerateToken(user)).Returns("mocked-jwt-token");

        LoginApplicationUserRequest request = new LoginApplicationUserRequest(user.Email!, "correctpass");

        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("mocked-jwt-token");
    }

    [Fact]
    public async Task Should_Return_Fail_When_Exception_Occurs()
    {
        LoginApplicationUserRequest request = new LoginApplicationUserRequest("user@email.com", "pass");

        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("DB exploded"));

        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}