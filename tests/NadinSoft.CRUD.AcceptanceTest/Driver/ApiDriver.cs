using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using NadinSoft.CRUD.AcceptanceTest.Context;

namespace NadinSoft.CRUD.AcceptanceTest.Driver;

public class ApiDriver
{
    private readonly HttpClient _client;
    private readonly TestContext _context;

    public ApiDriver(HttpClient client, TestContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task PostAsync(string url, object body, bool authenticated = false)
    {
        StringContent content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        if (authenticated && _context.JwtToken != null)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.JwtToken);
        }

        HttpResponseMessage response = await _client.PostAsync(url, content);
        await CaptureResponse(response);
    }

    public async Task PutAsync(string url, object body)
    {
        StringContent content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.JwtToken);

        HttpResponseMessage response = await _client.PutAsync(url, content);
        await CaptureResponse(response);
    }

    public async Task DeleteAsync(string urlWithQuery)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.JwtToken);
        HttpResponseMessage response = await _client.DeleteAsync(urlWithQuery);
        await CaptureResponse(response);
    }

    public async Task GetAsync(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);
        await CaptureResponse(response);
    }

    private async Task CaptureResponse(HttpResponseMessage response)
    {
        _context.LastResponse = response;
        _context.LastResponseBody = await response.Content.ReadAsStringAsync();
    }
}