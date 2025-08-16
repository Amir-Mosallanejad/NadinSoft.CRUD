using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Handlers;

/// <summary>
/// Contains unit tests for <see cref="LoginApplicationUserRequestHandler"/>.
/// </summary>
public class LoginApplicationUserRequestHandlerTests
{
    /// <summary>
    /// Mock of <see cref="UserManager{ApplicationUser}"/> used to simulate user operations.
    /// </summary>
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

    /// <summary>
    /// Mock of <see cref="IJwtTokenGenerator"/> used to simulate JWT generation.
    /// </summary>
    private readonly Mock<IJwtTokenGenerator> _jwtMock = new();

    /// <summary>
    /// Mock of <see cref="ILogger{LoginApplicationUserRequestHandler}"/> for logging inside the handler.
    /// </summary>
    private readonly Mock<ILogger<LoginApplicationUserRequestHandler>> _loggerMock = new();

    /// <summary>
    /// Instance of the handler under test.
    /// </summary>
    private readonly LoginApplicationUserRequestHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginApplicationUserRequestHandlerTests"/> class.
    /// Sets up mocks and the handler instance.
    /// </summary>
    public LoginApplicationUserRequestHandlerTests()
    {
        Mock<IUserStore<ApplicationUser>> store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        _handler = new LoginApplicationUserRequestHandler(
            _userManagerMock.Object,
            _jwtMock.Object,
            _loggerMock.Object);
    }

    /// <summary>
    /// Tests that the handler returns failure when the user is not found.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenUserNotFound()
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

    /// <summary>
    /// Tests that the handler returns failure when the password is invalid.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenPasswordIsInvalid()
    {
        ApplicationUser user = new ApplicationUser
        {
            Email = "user@email.com",
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "wrongpass")).ReturnsAsync(false);

        LoginApplicationUserRequest request = new LoginApplicationUserRequest(user.Email!, "wrongpass");

        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid credentials.");
    }

    /// <summary>
    /// Tests that the handler returns success and a JWT token when credentials are valid.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnSuccessWhenCredentialsAreValid()
    {
        ApplicationUser user = new ApplicationUser
        {
            Email = "user@email.com",
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, "correctpass")).ReturnsAsync(true);
        _jwtMock.Setup(x => x.GenerateToken(user)).Returns("mocked-jwt-token");

        LoginApplicationUserRequest request = new LoginApplicationUserRequest(user.Email!, "correctpass");

        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("mocked-jwt-token");
    }

    /// <summary>
    /// Tests that the handler returns a generic failure message when an exception occurs.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenExceptionOccurs()
    {
        LoginApplicationUserRequest request = new LoginApplicationUserRequest("user@email.com", "pass");

        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("DB exploded"));

        ApiResponse<string> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}