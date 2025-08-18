using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using System.Linq.Expressions;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

/// <summary>
/// Contains unit tests for <see cref="CreateProductRequestHandler"/>.
/// </summary>
public class CreateProductRequestHandlerTests
{
    /// <summary>
    /// Mock for <see cref="IProductRepository"/>.
    /// </summary>
    private readonly Mock<IProductRepository> _productRepoMock = new();

    /// <summary>
    /// Mock for <see cref="ICurrentUserService"/>.
    /// </summary>
    private readonly Mock<ICurrentUserService> _currentUserMock = new();

    /// <summary>
    /// Mock for <see cref="IMapper"/>.
    /// </summary>
    private readonly Mock<IMapper> _mapperMock = new();

    /// <summary>
    /// Mock for <see cref="ILogger{CreateProductRequestHandler}"/>.
    /// </summary>
    private readonly Mock<ILogger<CreateProductRequestHandler>> _loggerMock = new();

    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Handler instance being tested.
    /// </summary>
    private readonly CreateProductRequestHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductRequestHandlerTests"/> class.
    /// Initialize mocks and the handler.
    /// </summary>
    public CreateProductRequestHandlerTests()
    {
        _handler = new CreateProductRequestHandler(
            _productRepoMock.Object,
            _currentUserMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _localizationMock.Object);
    }

    /// <summary>
    /// Tests that the handler fails when the user is unauthorized.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenUserIsUnauthorized()
    {
        // Arrange
        _currentUserMock.Setup(x => x.UserId).Returns((string?)null);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UserUnauthorized))
            .Returns("User is unauthorized.");
        CreateProductRequest request =
            new CreateProductRequest(
                new CreateProductRequestDto(
                    "Test",
                    DateTime.UtcNow,
                    "+989121234567",
                    "test@mail.com",
                    true));

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User is unauthorized.");
    }

    /// <summary>
    /// Tests that the handler fails when a product with the same email and produce date already exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenProductAlreadyExists()
    {
        CreateProductRequestDto dto =
            new CreateProductRequestDto(
                "Test",
                DateTime.UtcNow,
                "+989121234567",
                "test@mail.com",
                true);
        CreateProductRequest request = new CreateProductRequest(dto);

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");

        _productRepoMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(true);

        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.ProductAlreadyExists))
            .Returns("Product with same email and produce date already exists.");

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");
    }

    /// <summary>
    /// Tests that the handler creates a product successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldCreateProductSuccessfully()
    {
        CreateProductRequestDto dto =
            new CreateProductRequestDto(
                "Test",
                DateTime.UtcNow,
                "+989121234567",
                "test@mail.com",
                true);
        CreateProductRequest request = new CreateProductRequest(dto);
        Product mappedEntity = new Product
        {
            Name = dto.Name,
        };

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");

        _productRepoMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(false);

        _mapperMock.Setup(x => x.Map<Product>(dto)).Returns(mappedEntity);

        _productRepoMock.Setup(x => x.AddAsync(mappedEntity)).ReturnsAsync(mappedEntity);

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _productRepoMock.Verify(x => x.AddAsync(mappedEntity), Times.Once);
    }

    /// <summary>
    /// Tests that the handler returns a fail response when an exception is thrown.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenExceptionThrown()
    {
        CreateProductRequestDto dto =
            new CreateProductRequestDto(
                "Test",
                DateTime.UtcNow,
                "+989121234567",
                "test@mail.com",
                true);
        CreateProductRequest request = new CreateProductRequest(dto);

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");

        _productRepoMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ThrowsAsync(new InvalidOperationException("DB crash"));

        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UnexpectedError))
            .Returns("An unexpected error occurred.");

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}