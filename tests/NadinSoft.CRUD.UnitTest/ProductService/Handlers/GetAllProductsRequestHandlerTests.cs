using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using System.Linq.Expressions;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

/// <summary>
/// Contains unit tests for <see cref="GetAllProductsRequestHandler"/>.
/// </summary>
public class GetAllProductsRequestHandlerTests
{
    /// <summary>
    /// Mock for <see cref="IUnitOfWork"/>.
    /// </summary>
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    /// <summary>
    /// Mock for <see cref="IMapper"/>.
    /// </summary>
    private readonly Mock<IMapper> _mapperMock = new();

    /// <summary>
    /// Mock for <see cref="ILogger{GetAllProductsRequestHandler}"/>.
    /// </summary>
    private readonly Mock<ILogger<GetAllProductsRequestHandler>> _loggerMock = new();

    /// <summary>
    /// Mock instance of <see cref="ILocalizationService"/> used for unit testing.
    /// </summary>
    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Handler instance being tested.
    /// </summary>
    private readonly GetAllProductsRequestHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllProductsRequestHandlerTests"/> class.
    /// Initialize mocks and the handler.
    /// </summary>
    public GetAllProductsRequestHandlerTests()
    {
        _handler = new GetAllProductsRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _localizationMock.Object);
    }

    /// <summary>
    /// Tests that the handler returns paginated products successfully.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnPaginatedProductsSuccessfully()
    {
        // Arrange
        Product product1 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Milk",
            ProduceDate = DateTime.UtcNow.AddDays(-10),
            ManufacturePhone = "+989121234567",
            ManufactureEmail = "milk@factory.com",
            IsAvailable = true,
            IsValid = true,
            CreatedByUserId = Guid.NewGuid(),
        };

        Product product2 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Bread",
            ProduceDate = DateTime.UtcNow.AddDays(-5),
            ManufacturePhone = "+989121234568",
            ManufactureEmail = "bread@factory.com",
            IsAvailable = false,
            IsValid = true,
            CreatedByUserId = Guid.NewGuid(),
        };

        ProductResponseDto dto1 = new ProductResponseDto(
            product1.Id,
            product1.Name,
            product1.ProduceDate,
            product1.ManufacturePhone,
            product1.ManufactureEmail,
            product1.IsAvailable,
            product1.IsValid,
            product1.CreatedByUserId);

        ProductResponseDto dto2 = new ProductResponseDto(
            product2.Id,
            product2.Name,
            product2.ProduceDate,
            product2.ManufacturePhone,
            product2.ManufactureEmail,
            product2.IsAvailable,
            product2.IsValid,
            product2.CreatedByUserId);

        GetAllProductsRequest request = new GetAllProductsRequest
        {
            Name = "bread",
            Page = 1,
            PerPage = 2,
        };

        _unitOfWorkMock
            .Setup(r => r.GetRepository<Product>().GetByFiltersAsync(p => p.Name.Contains(request.Name), 1, 2))
            .ReturnsAsync(
                (2, new List<Product>
                {
                    product1,
                    product2,
                }));

        _mapperMock.Setup(m => m.Map<ProductResponseDto>(product1)).Returns(dto1);
        _mapperMock.Setup(m => m.Map<ProductResponseDto>(product2)).Returns(dto2);

        // Act
        ApiResponse<PaginatedResponse<ProductResponseDto>> result =
            await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Items.Should().HaveCount(2);
        result.Data.TotalCount.Should().Be(2);

        ProductResponseDto first = result.Data.Items.First();
        first.Id.Should().Be(dto1.Id);
        first.Name.Should().Be(dto1.Name);
        first.ManufactureEmail.Should().Be(dto1.ManufactureEmail);
    }

    /// <summary>
    /// Tests that the handler returns failure when an unexpected exception occurs.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ShouldReturnFailureWhenExceptionOccurs()
    {
        GetAllProductsRequest request = new GetAllProductsRequest
        {
            Name = "crash",
            Page = 1,
            PerPage = 5,
        };

        _unitOfWorkMock
            .Setup(r => r.GetRepository<Product>()
                .GetByFiltersAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException("DB crash"));
        _localizationMock.Setup(x => x.GetApiMessageResource(ApiMessageResourceKey.UnexpectedError))
            .Returns("An unexpected error occurred.");

        ApiResponse<PaginatedResponse<ProductResponseDto>> result =
            await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("unexpected error");
    }
}