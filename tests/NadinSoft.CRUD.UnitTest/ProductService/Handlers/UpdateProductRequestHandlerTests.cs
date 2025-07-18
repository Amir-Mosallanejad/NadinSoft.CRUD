using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

public class UpdateProductRequestHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<UpdateProductRequestHandler>> _loggerMock = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();

    private readonly UpdateProductRequestHandler _handler;

    public UpdateProductRequestHandlerTests()
    {
        _handler = new UpdateProductRequestHandler(
            _productRepoMock.Object,
            _currentUserMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Failure_If_User_Not_Authenticated()
    {
        // Arrange
        _currentUserMock.Setup(c => c.UserId).Returns((string?)null);

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true)
        );

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User is unauthorized.");
    }

    [Fact]
    public async Task Should_Return_Failure_If_Product_Not_Found()
    {
        // Arrange
        string userId = "user-123";
        _currentUserMock.Setup(c => c.UserId).Returns(userId);

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true)
        );

        _productRepoMock.Setup(r => r.GetByIdAsync(command.Dto.Id))
            .ReturnsAsync((Product?)null);

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Should_Return_Failure_If_User_Is_Not_Owner()
    {
        // Arrange
        string userId = "user-123";
        Product product = new Product { Id = Guid.NewGuid(), CreatedByUserId = "other-user" };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _productRepoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        UpdateProductRequest command = new UpdateProductRequest(
            new UpdateProductRequestDto(
                product.Id,
                "Test",
                DateTime.UtcNow,
                "mail@mail.com",
                "123",
                true)
        );

        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not owner");
    }

    [Fact]
    public async Task Should_Update_Product_Successfully()
    {
        // Arrange
        string userId = "user-123";
        Product product = new Product { Id = Guid.NewGuid(), CreatedByUserId = userId };

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
                true)
        );


        // Act
        ApiResponse<object> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _productRepoMock.Verify(r => r.Update(product), Times.Once);
    }
}