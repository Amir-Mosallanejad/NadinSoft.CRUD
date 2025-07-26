using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

public class DeleteProductRequestHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly Mock<ILogger<DeleteProductRequestHandler>> _loggerMock = new();

    private readonly DeleteProductRequestHandler _handler;

    public DeleteProductRequestHandlerTests()
    {
        _handler = new DeleteProductRequestHandler(
            _productRepoMock.Object,
            _currentUserMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Fail_When_User_Is_Unauthorized()
    {
        _currentUserMock.Setup(x => x.UserId).Returns((string?)null);

        DeleteProductRequest request = new DeleteProductRequest(Guid.NewGuid());

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User is unauthorized.");
    }

    [Fact]
    public async Task Should_Return_Fail_When_Product_Not_Found()
    {
        Guid productId = Guid.NewGuid();

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");
        _productRepoMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Should_Return_Fail_When_User_Is_Not_Owner()
    {
        Guid productId = Guid.NewGuid();

        Product product = new Product
        {
            Id = productId,
            CreatedByUserId = "owner-id"
        };

        _currentUserMock.Setup(x => x.UserId).Returns("non-owner");
        _productRepoMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not owner");
    }

    [Fact]
    public async Task Should_Delete_Product_Successfully()
    {
        Guid productId = Guid.NewGuid();

        Product product = new Product
        {
            Id = productId,
            CreatedByUserId = "user-1"
        };

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");
        _productRepoMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _productRepoMock.Verify(x => x.Remove(product), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Fail_When_Exception_Occurs()
    {
        Guid productId = Guid.NewGuid();

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");
        _productRepoMock.Setup(x => x.GetByIdAsync(productId)).ThrowsAsync(new Exception("boom"));

        DeleteProductRequest request = new DeleteProductRequest(productId);

        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}
