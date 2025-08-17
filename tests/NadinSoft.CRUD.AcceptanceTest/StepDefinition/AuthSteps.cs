using FluentAssertions;
using NadinSoft.CRUD.AcceptanceTest.Context;
using NadinSoft.CRUD.AcceptanceTest.Driver;
using NadinSoft.CRUD.AcceptanceTest.Dto;
using System.Text.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NadinSoft.CRUD.AcceptanceTest.StepDefinition;

/// <summary>
/// Defines the SpecFlow steps for authentication scenarios.
/// </summary>
[Binding]
public class AuthSteps
{
    /// <summary>
    /// Holds shared test data and responses.
    /// </summary>
    private readonly TestContext _context;

    /// <summary>
    /// Driver for performing HTTP requests to the API.
    /// </summary>
    private readonly ApiDriver _driver;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthSteps"/> class.
    /// </summary>
    /// <param name="context">The shared test context.</param>
    public AuthSteps(TestContext context)
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
    /// Registers a new user using the provided data table.
    /// </summary>
    /// <param name="table">The SpecFlow table containing user registration data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Given(@"I register a new user with:")]
    public async Task GivenIRegisterAUserWith(Table table)
    {
        RegisterDto? user = table.CreateInstance<RegisterDto>();

        await _driver.PostAsync("api/auth/register", user);
    }

    /// <summary>
    /// Logs in using credentials provided in the data table and stores the JWT token.
    /// </summary>
    /// <param name="table">The SpecFlow table containing login credentials.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [When(@"I login using:")]
    public async Task WhenILoginUsing(Table table)
    {
        LoginDto? creds = table.CreateInstance<LoginDto>();

        await _driver.PostAsync("api/auth/login", creds);

        JsonElement json = JsonSerializer.Deserialize<JsonElement>(_context.LastResponseBody!);
        string? token = json.GetProperty("data").GetString();

        token.Should().NotBeNullOrEmpty();
        _context.JwtToken = token;
    }

    /// <summary>
    /// Asserts that a JWT token has been received and stored in the test context.
    /// </summary>
    [Then(@"I should receive a JWT token")]
    public void ThenIShouldReceiveAJwtToken()
    {
        _context.JwtToken.Should().NotBeNullOrEmpty("JWT token should be extracted and stored");
    }
}