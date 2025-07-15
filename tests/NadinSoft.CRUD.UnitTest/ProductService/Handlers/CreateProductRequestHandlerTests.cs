using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

public class CreateProductRequestHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<CreateProductRequestHandler>> _loggerMock = new();

    private readonly CreateProductRequestHandler _handler;

    public CreateProductRequestHandlerTests()
    {
        _handler = new CreateProductRequestHandler(
            _productRepoMock.Object,
            _currentUserMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Fail_When_User_Is_Unauthorized()
    {
        // Arrange
        _currentUserMock.Setup(x => x.UserId).Returns((string?)null);
        CreateProductRequest request =
            new CreateProductRequest(new CreateProductRequestDto(
                Guid.NewGuid(),
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

    [Fact]
    public async Task Should_Return_Fail_When_Product_Already_Exists()
    {
        CreateProductRequestDto dto =
            new CreateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "+989121234567",
                "test@mail.com",
                true);
        CreateProductRequest request = new CreateProductRequest(dto);

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");

        _productRepoMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(true);

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");
    }

    [Fact]
    public async Task Should_Create_Product_Successfully()
    {
        CreateProductRequestDto dto =
            new CreateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "+989121234567",
                "test@mail.com",
                true);
        CreateProductRequest request = new CreateProductRequest(dto);
        Product mappedEntity = new Product { Name = dto.Name };

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

    [Fact]
    public async Task Should_Return_Fail_When_Exception_Thrown()
    {
        CreateProductRequestDto dto =
            new CreateProductRequestDto(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                "+989121234567",
                "test@mail.com",
                true);
        CreateProductRequest request = new CreateProductRequest(dto);

        _currentUserMock.Setup(x => x.UserId).Returns("user-1");

        _productRepoMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ThrowsAsync(new Exception("DB crash"));

        // Act
        ApiResponse<object> result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}