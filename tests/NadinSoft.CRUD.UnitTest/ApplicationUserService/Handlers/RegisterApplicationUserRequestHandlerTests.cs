using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Handlers;

/// <summary>
/// Contains unit tests for <see cref="RegisterApplicationUserRequestHandler"/>.
/// </summary>
public class RegisterApplicationUserRequestHandlerTests
{
    /// <summary>
    /// Mock of <see cref="UserManager{ApplicationUser}"/> used for testing user operations.
    /// </summary>
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

    /// <summary>
    /// Mock of <see cref="IMapper"/> used for mapping requests to entities.
    /// </summary>
    private readonly Mock<IMapper> _mapperMock = new();

    /// <summary>
    /// Mock of <see cref="ILogger{RegisterApplicationUserRequestHandler}"/> for logging inside the handler.
    /// </summary>
    private readonly Mock<ILogger<RegisterApplicationUserRequestHandler>> _loggerMock = new();

    /// <summary>
    /// Mock instance of <see cref="ILocalizationService"/> used for unit testing.
    /// </summary>
    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Instance of the handler under test.
    /// </summary>
    private readonly RegisterApplicationUserRequestHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterApplicationUserRequestHandlerTests"/> class.
    /// Sets up mocks and the handler instance.
    /// </summary>
    public RegisterApplicationUserRequestHandlerTests()
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

        _handler = new RegisterApplicationUserRequestHandler(
            _userManagerMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _localizationMock.Object);
    }

    /// <summary>
    /// Tests that the handler fails when a user with the given email already exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldFailIfUserAlreadyExists()
    {
        // Arrange
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest(
            "test@mail.com",
            "Pa$$word",
            "Pa$$word");
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new ApplicationUser());
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.EmailAlreadyExists))
            .Returns("User with this email already exists.");

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User with this email already exists.");
    }

    /// <summary>
    /// Tests that the handler fails when creating a new user fails due to identity errors.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldFailIfCreateAsyncFails()
    {
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest(
            "test@mail.com",
            "Pa$$word",
            "Pa$$word");

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser?)null);

        ApplicationUser mappedUser = new ApplicationUser
        {
            Email = request.Email,
        };
        _mapperMock.Setup(x => x.Map<ApplicationUser>(request)).Returns(mappedUser);
        _localizationMock.Setup(x =>
                x.GetApiMessageResource(
                    It.Is<string>(s => s.Contains("Password") || s.Contains("Email")),
                    It.IsAny<object[]>()))
            .Returns((string s, object[] _) => s);

        IdentityResult identityResult = IdentityResult.Failed(
            new IdentityError
            {
                Description = "Password is too weak",
            },
            new IdentityError
            {
                Description = "Email is invalid",
            });

        _userManagerMock.Setup(x => x.CreateAsync(mappedUser, request.Password))
            .ReturnsAsync(identityResult);

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Password is too weak | Email is invalid");
    }

    /// <summary>
    /// Tests that the handler successfully creates a user when all conditions are met.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldCreateUserSuccessfully()
    {
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest(
            "test@mail.com",
            "Pa$$word",
            "Pa$$word");

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser?)null);

        ApplicationUser mappedUser = new ApplicationUser
        {
            Email = request.Email,
        };
        _mapperMock.Setup(x => x.Map<ApplicationUser>(request)).Returns(mappedUser);

        _userManagerMock.Setup(x => x.CreateAsync(mappedUser, request.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that the handler returns error message if an unhandled exception occurs.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnGenericErrorOnException()
    {
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest("test@mail.com", "pass", "pass");

        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Oops"));
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UnexpectedError))
            .Returns("An unexpected error occurred.");

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}