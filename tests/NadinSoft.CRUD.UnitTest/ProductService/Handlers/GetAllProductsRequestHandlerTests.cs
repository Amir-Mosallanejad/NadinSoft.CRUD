using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.UnitTest.ProductService.Handlers;

public class GetAllProductsRequestHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetAllProductsRequestHandler>> _loggerMock = new();

    private readonly GetAllProductsRequestHandler _handler;

    public GetAllProductsRequestHandlerTests()
    {
        _handler = new GetAllProductsRequestHandler(
            _productRepoMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Paginated_Products_Successfully()
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
            CreatedByUserId = "user-1"
        };

        Product product2 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Bread",
            ProduceDate = DateTime.UtcNow.AddDays(-5),
            ManufacturePhone = "+989121234568",
            ManufactureEmail = "bread@factory.com",
            IsAvailable = false,
            CreatedByUserId = "user-2"
        };

        ProductResponseDto dto1 = new ProductResponseDto(
            product1.Id,
            product1.Name,
            product1.ProduceDate,
            product1.ManufacturePhone,
            product1.ManufactureEmail,
            product1.IsAvailable,
            product1.CreatedByUserId
        );

        ProductResponseDto dto2 = new ProductResponseDto(
            product2.Id,
            product2.Name,
            product2.ProduceDate,
            product2.ManufacturePhone,
            product2.ManufactureEmail,
            product2.IsAvailable,
            product2.CreatedByUserId
        );

        GetAllProductsRequest request = new GetAllProductsRequest
        {
            Name = "bread",
            Page = 1,
            PerPage = 2
        };

        _productRepoMock
            .Setup(r => r.GetProductsByFilters("bread", 1, 2))
            .ReturnsAsync((2, new List<Product> { product1, product2 }));

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

    [Fact]
    public async Task Should_Return_Failure_When_Exception_Occurs()
    {
        GetAllProductsRequest request = new GetAllProductsRequest
        {
            Name = "crash",
            Page = 1,
            PerPage = 5
        };

        _productRepoMock
            .Setup(r => r.GetProductsByFilters(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("DB crash"));

        ApiResponse<PaginatedResponse<ProductResponseDto>> result =
            await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("unexpected error");
    }
}