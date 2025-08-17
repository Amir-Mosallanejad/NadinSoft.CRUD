using FluentAssertions;
using NadinSoft.CRUD.AcceptanceTest.Context;
using NadinSoft.CRUD.AcceptanceTest.Driver;
using NadinSoft.CRUD.AcceptanceTest.Dto;
using System.Text.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NadinSoft.CRUD.AcceptanceTest.StepDefinition;

/// <summary>
/// Defines the SpecFlow steps for product-related scenarios.
/// </summary>
[Binding]
public class ProductSteps
{
    /// <summary>
    /// JSON serializer options used for deserializing API responses.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Holds shared test data and responses.
    /// </summary>
    private readonly TestContext _context;

    /// <summary>
    /// Driver for performing HTTP requests to the API.
    /// </summary>
    private readonly ApiDriver _driver;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductSteps"/> class.
    /// </summary>
    /// <param name="context">The shared test context.</param>
    public ProductSteps(TestContext context)
    {
        _context = context;
        _driver = new ApiDriver(
            new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/"),
            },
            context);
    }

    /// <summary>
    /// Creates a product using the data provided in the table, using the stored JWT token for authentication.
    /// </summary>
    /// <param name="table">The SpecFlow table containing product creation data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When(@"I use the token to create a product with:")]
    public async Task WhenICreateProductWith(Table table)
    {
        CreateProductRequestDto? dto = table.CreateInstance<CreateProductRequestDto>();

        await _driver.PostAsync("api/product/create", dto, authenticated: true);

        ApiResponse<object>? parsed = JsonSerializer.Deserialize<ApiResponse<object>>(
            _context.LastResponseBody!,
            JsonOptions);

        _context.CreateResponse = parsed!;
    }

    /// <summary>
    /// Verifies that the product was successfully created, updated, or deleted.
    /// </summary>
    /// <param name="action">The action performed: "created", "updated", or "deleted".</param>
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

    /// <summary>
    /// Updates the product with the provided data table.
    /// </summary>
    /// <param name="table">The SpecFlow table containing updated product data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When(@"I update the product with:")]
    public async Task WhenIUpdateProductWith(Table table)
    {
        UpdateProductRequestDto? dto = table.CreateInstance<UpdateProductRequestDto>();

        dto.Id = _context.CreatedProductId;

        await _driver.PutAsync("api/product/update", dto);

        ApiResponse<object>? parsed = JsonSerializer.Deserialize<ApiResponse<object>>(
            _context.LastResponseBody!,
            JsonOptions);

        _context.UpdateResponse = parsed!;
    }

    /// <summary>
    /// Retrieves the product list from the API.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When("I retrieve the product list")]
    public async Task WhenIRetrieveTheProductList()
    {
        await _driver.GetAsync("api/product/get-all?page=1&pageSize=10");

        ApiResponse<PaginatedResponse<ProductResponseDto>>? parsed =
            JsonSerializer.Deserialize<ApiResponse<PaginatedResponse<ProductResponseDto>>>(
                _context.LastResponseBody!,
                JsonOptions);

        _context.GetAllResponse = parsed!;
    }

    /// <summary>
    /// Verifies that the retrieved product list is empty.
    /// </summary>
    [Then("the response should contain an empty list")]
    public void ThenTheResponseShouldContainAnEmptyList()
    {
        _context.GetAllResponse!.Data!.Items.Count.Should().Be(0);
    }

    /// <summary>
    /// Retrieves the product list and extracts the first product ID into the test context.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When("I retrieve the product list and extract the first product ID")]
    public async Task WhenIRetrieveAndExtractCreatedProductId()
    {
        await _driver.GetAsync("api/product/get-all?page=1&pageSize=10");

        ApiResponse<PaginatedResponse<ProductResponseDto>>? parsed =
            JsonSerializer.Deserialize<ApiResponse<PaginatedResponse<ProductResponseDto>>>(
                _context.LastResponseBody!,
                JsonOptions);

        parsed.Should().NotBeNull();
        parsed.IsSuccess.Should().BeTrue();
        parsed.Data!.Items.Should().NotBeEmpty();

        ProductResponseDto product = parsed.Data.Items.First();
        _context.CreatedProductId = product.Id;
    }

    /// <summary>
    /// Deletes the previously created product using its ID from the test context.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When("I delete the product")]
    public async Task WhenIDeleteTheProduct()
    {
        Guid productId = _context.CreatedProductId;

        await _driver.DeleteAsync($"api/product/delete?productId={productId}");

        ApiResponse<object>? parsed = JsonSerializer.Deserialize<ApiResponse<object>>(
            _context.LastResponseBody!,
            JsonOptions);

        _context.DeleteResponse = parsed!;
    }
}