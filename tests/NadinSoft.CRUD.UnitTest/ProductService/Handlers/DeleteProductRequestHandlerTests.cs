using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

/// <summary>
/// Contains unit tests for <see cref="DeleteProductRequestHandler"/>.
/// </summary>
public class DeleteProductRequestHandlerTests
{
    /// <summary>
    /// Mock for <see cref="IProductRepository"/>.
    /// </summary>
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    /// <summary>
    /// Mock for <see cref="ICurrentUserService"/>.
    /// </summary>
    private readonly Mock<ICurrentUserService> _currentUserMock = new();

    /// <summary>
    /// Mock for <see cref="ILogger{DeleteProductRequestHandler}"/>.
    /// </summary>
    private readonly Mock<ILogger<DeleteProductRequestHandler>> _loggerMock = new();

    /// <summary>
    /// Mock instance of <see cref="ILocalizationService"/> used for unit testing.
    /// </summary>
    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Handler instance being tested.
    /// </summary>
    private readonly DeleteProductRequestHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductRequestHandlerTests"/> class.
    /// Initialize mocks and the handler.
    /// </summary>
    public DeleteProductRequestHandlerTests()
    {
        _handler = new DeleteProductRequestHandler(
            _unitOfWorkMock.Object,
            _currentUserMock.Object,
            _loggerMock.Object,
            _localizationMock.Object);
    }

    /// <summary>
    /// Tests that the handler fails when the current user is unauthorized.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenUserIsUnauthorized()
    {
        _currentUserMock.Setup(x => x.UserId).Returns((string?)null);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UserUnauthorized))
            .Returns("User is unauthorized.");

        DeleteProductRequest request = new DeleteProductRequest(Guid.NewGuid());

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User is unauthorized.");
    }

    /// <summary>
    /// Tests that the handler fails when the product to delete is not found.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenProductNotFound()
    {
        Guid productId = Guid.NewGuid();

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");
        _unitOfWorkMock.Setup(x => x.ProductRepository.GetByIdAsync(productId)).ReturnsAsync((Product?)null);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.ProductNotFound))
            .Returns("Product not found.");

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Product not found.");
    }

    /// <summary>
    /// Tests that the handler fails when the current user is not the owner of the product.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenUserIsNotOwner()
    {
        Guid productId = Guid.NewGuid();

        Product product = new Product
        {
            Id = productId,
            CreatedByUserId = "owner-id",
        };

        _currentUserMock.Setup(x => x.UserId).Returns("non-owner");
        _unitOfWorkMock.Setup(x => x.ProductRepository.GetByIdAsync(productId)).ReturnsAsync(product);
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.NotOwnerOfProductDelete))
            .Returns("You are not owner of this product.");

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not owner");
    }

    /// <summary>
    /// Tests that the handler deletes a product successfully when the user is the owner.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldDeleteProductSuccessfully()
    {
        Guid productId = Guid.NewGuid();

        Product product = new Product
        {
            Id = productId,
            CreatedByUserId = "user-1",
        };

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");
        _unitOfWorkMock.Setup(x => x.ProductRepository.GetByIdAsync(productId)).ReturnsAsync(product);

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _unitOfWorkMock.Verify(x => x.ProductRepository.Remove(product), Times.Once);
    }

    /// <summary>
    /// Tests that the handler returns a fail response when an unexpected exception occurs.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailWhenExceptionOccurs()
    {
        Guid productId = Guid.NewGuid();

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");
        _unitOfWorkMock.Setup(x => x.ProductRepository.GetByIdAsync(productId))
            .ThrowsAsync(new InvalidOperationException("boom"));
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UnexpectedError))
            .Returns("An unexpected error occurred.");

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}