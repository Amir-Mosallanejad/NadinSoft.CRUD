using System.Text.Json;
using FluentAssertions;
using NadinSoft.CRUD.AcceptanceTest.Context;
using NadinSoft.CRUD.AcceptanceTest.Driver;
using NadinSoft.CRUD.AcceptanceTest.Dto;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NadinSoft.CRUD.AcceptanceTest.StepDefinition;

[Binding]
public class ProductSteps
{
    private readonly TestContext _context;
    private readonly ApiDriver _driver;

    public ProductSteps(TestContext context)
    {
        _context = context;
        _driver = new ApiDriver(new HttpClient { BaseAddress = new Uri("http://localhost:8080/") }, context);
    }

    [When(@"I use the token to create a product with:")]
    public async Task WhenICreateProductWith(Table table)
    {
        CreateProductRequestDto? dto = table.CreateInstance<CreateProductRequestDto>();

        await _driver.PostAsync("api/product/create", dto, authenticated: true);

        ApiResponse<object>? parsed = JsonSerializer.Deserialize<ApiResponse<object>>(_context.LastResponseBody!,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        _context.CreateResponse = parsed!;
    }

    [Then(@"the product should be ""(.*)"" successfully")]
    public void ThenTheProductShouldBeSuccessfully(string action)
    {
        switch (action.ToLowerInvariant())
        {
            case "created":
                _context.CreateResponse.Should().NotBeNull();
                _context.CreateResponse!.IsSuccess.Should().BeTrue();
                break;

            case "updated":
                _context.UpdateResponse.Should().NotBeNull();
                _context.UpdateResponse!.IsSuccess.Should().BeTrue();
                break;

            case "deleted":
                _context.DeleteResponse.Should().NotBeNull();
                _context.DeleteResponse!.IsSuccess.Should().BeTrue();
                break;
        }
    }

    [When(@"I update the product with:")]
    public async Task WhenIUpdateProductWith(Table table)
    {
        UpdateProductRequestDto? dto = table.CreateInstance<UpdateProductRequestDto>();

        dto.Id = _context.CreatedProductId;

        await _driver.PutAsync("api/product/update", dto);

        ApiResponse<object>? parsed = JsonSerializer.Deserialize<ApiResponse<object>>(
            _context.LastResponseBody!,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        _context.UpdateResponse = parsed!;
    }

    [When("I retrieve the product list")]
    public async Task WhenIRetrieveTheProductList()
    {
        await _driver.GetAsync("api/product/get-all?page=1&pageSize=10");

        ApiResponse<PaginatedResponse<ProductResponseDto>>? parsed =
            JsonSerializer.Deserialize<ApiResponse<PaginatedResponse<ProductResponseDto>>>(
                _context.LastResponseBody!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        _context.GetAllResponse = parsed!;
    }

    [Then("the response should contain an empty list")]
    public void ThenTheResponseShouldContainAnEmptyList()
    {
        _context.GetAllResponse!.Data!.Items.Count.Should().Be(0);
    }

    [When("I retrieve the product list and extract the first product ID")]
    public async Task WhenIRetrieveAndExtractCreatedProductId()
    {
        await _driver.GetAsync("api/product/get-all?page=1&pageSize=10");

        ApiResponse<PaginatedResponse<ProductResponseDto>>? parsed =
            JsonSerializer.Deserialize<ApiResponse<PaginatedResponse<ProductResponseDto>>>(
                _context.LastResponseBody!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        parsed.Should().NotBeNull();
        parsed.IsSuccess.Should().BeTrue();
        parsed.Data!.Items.Should().NotBeEmpty();

        ProductResponseDto product = parsed.Data.Items.First();
        _context.CreatedProductId = product.Id;
    }

    [When("I delete the product")]
    public async Task WhenIDeleteTheProduct()
    {
        Guid productId = _context.CreatedProductId;

        await _driver.DeleteAsync($"api/product/delete?productId={productId}");

        ApiResponse<object>? parsed = JsonSerializer.Deserialize<ApiResponse<object>>(
            _context.LastResponseBody!,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        _context.DeleteResponse = parsed!;
    }
}