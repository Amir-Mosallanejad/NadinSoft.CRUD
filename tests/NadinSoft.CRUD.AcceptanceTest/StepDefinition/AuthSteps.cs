using System.Text.Json;
using FluentAssertions;
using NadinSoft.CRUD.AcceptanceTest.Context;
using NadinSoft.CRUD.AcceptanceTest.Driver;
using NadinSoft.CRUD.AcceptanceTest.Dto;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NadinSoft.CRUD.AcceptanceTest.StepDefinition;

[Binding]
public class AuthSteps
{
    private readonly TestContext _context;
    private readonly ApiDriver _driver;

    public AuthSteps(TestContext context)
    {
        _context = context;
        _driver = new ApiDriver(new HttpClient { BaseAddress = new Uri("http://localhost:8080/") }, context);
    }

    [Given(@"I register a new user with:")]
    public async Task GivenIRegisterAUserWith(Table table)
    {
        RegisterDto? user = table.CreateInstance<RegisterDto>();

        await _driver.PostAsync("api/auth/register", user);
    }

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

    [Then(@"I should receive a JWT token")]
    public void ThenIShouldReceiveAJwtToken()
    {
        _context.JwtToken.Should().NotBeNullOrEmpty("JWT token should be extracted and stored");
    }
}