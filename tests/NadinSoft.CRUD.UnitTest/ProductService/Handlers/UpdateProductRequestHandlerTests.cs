using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

/// <summary>
/// Contains unit tests for <see cref="UpdateProductRequestHandler"/>.
/// </summary>
public class UpdateProductRequestHandlerTests
{
    /// <summary>
    /// Mock for <see cref="IProductRepository"/>.
    /// </summary>
    private readonly Mock<IProductRepository> _productRepoMock = new();

    /// <summary>
    /// Mock for <see cref="IMapper"/>.
    /// </summary>
    private readonly Mock<IMapper> _mapperMock = new();

    /// <summary>
    /// Mock for <see cref="ILogger{UpdateProductRequestHandler}"/>.
    /// </summary>
    private readonly Mock<ILogger<UpdateProductRequestHandler>> _loggerMock = new();

    /// <summary>
    /// Mock for <see cref="ICurrentUserService"/>.
    /// </summary>
    private readonly Mock<ICurrentUserService> _currentUserMock = new();

    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Handler instance being tested.
    /// </summary>
    private readonly UpdateProductRequestHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductRequestHandlerTests"/> class.
    /// Initialize mocks and the handler.
    /// </summary>
    public UpdateProductRequestHandlerTests()
    {
        _handler = new UpdateProductRequestHandler(
            _productRepoMock.Object,
            _currentUserMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _localizationMock.Object);
    }

    /// <summary>
    /// Tests that updating fails when the user is not authenticated.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailureIfUserNotAuthenticated()
    {
        // Arrange
        _currentUserMock.Setup(c => c.UserId).Returns((string?)null);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UserUnauthorized))
            .Returns("User is unauthorized.");

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true));

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User is unauthorized.");
    }

    /// <summary>
    /// Tests that updating fails when the product does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailureIfProductNotFound()
    {
        // Arrange
        string userId = "user-123";
        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.ProductNotFound))
            .Returns("Product not found.");

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true));

        _productRepoMock.Setup(r => r.GetByIdAsync(command.Dto.Id))
            .ReturnsAsync((Product?)null);

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Product not found.");
    }

    /// <summary>
    /// Tests that updating fails when the user is not the owner of the product.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailureIfUserIsNotOwner()
    {
        // Arrange
        string userId = "user-123";
        Product product = new Product
        {
            Id = Guid.NewGuid(),
            CreatedByUserId = "other-user",
        };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _productRepoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.NotOwnerOfProductUpdate))
            .Returns("You are not owner of this product.");

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                product.Id,
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true));

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not owner");
    }

    /// <summary>
    /// Tests that a product is updated successfully when all conditions are met.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldUpdateProductSuccessfully()
    {
        // Arrange
        string userId = "user-123";
        Product product = new Product
        {
            Id = Guid.NewGuid(),
            CreatedByUserId = userId,
        };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _productRepoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map(It.IsAny<UpdateProductRequestDto>(), product));

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                product.Id,
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true));

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _productRepoMock.Verify(r => r.Update(product), Times.Once);
    }
}