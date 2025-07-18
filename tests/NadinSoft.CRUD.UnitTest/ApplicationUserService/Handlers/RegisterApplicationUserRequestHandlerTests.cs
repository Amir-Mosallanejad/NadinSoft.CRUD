using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Handlers;

public class RegisterApplicationUserRequestHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<RegisterApplicationUserRequestHandler>> _loggerMock = new();

    private readonly RegisterApplicationUserRequestHandler _handler;

    public RegisterApplicationUserRequestHandlerTests()
    {
        Mock<IUserStore<ApplicationUser>> store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _handler = new RegisterApplicationUserRequestHandler(
            _userManagerMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Should_Fail_If_User_Already_Exists()
    {
        // Arrange
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest("test@mail.com", "Pa$$word", "Pa$$word");
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new ApplicationUser());

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User with this email already exists.");
    }

    [Fact]
    public async Task Should_Fail_If_CreateAsync_Fails()
    {
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest("test@mail.com", "Pa$$word", "Pa$$word");

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser?)null);

        ApplicationUser mappedUser = new ApplicationUser { Email = request.Email };
        _mapperMock.Setup(x => x.Map<ApplicationUser>(request)).Returns(mappedUser);

        IdentityResult identityResult = IdentityResult.Failed(
            new IdentityError { Description = "Password is too weak" },
            new IdentityError { Description = "Email is invalid" });

        _userManagerMock.Setup(x => x.CreateAsync(mappedUser, request.Password))
            .ReturnsAsync(identityResult);

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Password is too weak | Email is invalid");
    }

    [Fact]
    public async Task Should_Create_User_Successfully()
    {
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest("test@mail.com", "Pa$$word", "Pa$$word");

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser?)null);

        ApplicationUser mappedUser = new ApplicationUser { Email = request.Email };
        _mapperMock.Setup(x => x.Map<ApplicationUser>(request)).Returns(mappedUser);

        _userManagerMock.Setup(x => x.CreateAsync(mappedUser, request.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Return_Generic_Error_On_Exception()
    {
        RegisterApplicationUserRequest request = new RegisterApplicationUserRequest("test@mail.com", "pass", "pass");

        _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Oops"));

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}