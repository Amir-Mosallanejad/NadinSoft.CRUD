using NadinSoft.CRUD.AcceptanceTest.Context;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NadinSoft.CRUD.AcceptanceTest.Driver;

/// <summary>
/// Provides a wrapper for sending HTTP requests to the API and
/// capturing responses for acceptance tests.
/// </summary>
public class ApiDriver
{
    /// <summary>
    /// The HTTP client used to send requests to the API.
    /// </summary>
    private readonly HttpClient _client;

    /// <summary>
    /// The shared test context that stores responses, tokens, and test-related state.
    /// </summary>
    private readonly TestContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiDriver"/> class.
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/> used to send requests to the API.</param>
    /// <param name="context">The shared <see cref="TestContext"/> for storing responses and state.</param>
    public ApiDriver(HttpClient client, TestContext context)
    {
        _client = client;
        _context = context;
    }

    /// <summary>
    /// Sends a POST request to the specified URL with a serialized JSON body.
    /// Optionally includes authentication if a JWT token is available.
    /// </summary>
    /// <param name="url">The endpoint URL to send the POST request to.</param>
    /// <param name="body">The object to serialize as the JSON request body.</param>
    /// <param name="authenticated">Whether to include the JWT token in the request header.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Sends a PUT request to the specified URL with a serialized JSON body,
    /// including the JWT token in the request header.
    /// </summary>
    /// <param name="url">The endpoint URL to send the PUT request to.</param>
    /// <param name="body">The object to serialize as the JSON request body.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PutAsync(string url, object body)
    {
        StringContent content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.JwtToken);

        HttpResponseMessage response = await _client.PutAsync(url, content);
        await CaptureResponse(response);
    }

    /// <summary>
    /// Sends a DELETE request to the specified URL with query parameters,
    /// including the JWT token in the request header.
    /// </summary>
    /// <param name="urlWithQuery">The endpoint URL (including query string) to send the DELETE request to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(string urlWithQuery)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.JwtToken);
        HttpResponseMessage response = await _client.DeleteAsync(urlWithQuery);
        await CaptureResponse(response);
    }

    /// <summary>
    /// Sends a GET request to the specified URL.
    /// </summary>
    /// <param name="url">The endpoint URL to send the GET request to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task GetAsync(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);
        await CaptureResponse(response);
    }

    /// <summary>
    /// Captures the HTTP response and updates the shared <see cref="TestContext"/>
    /// with the response and its body content.
    /// </summary>
    /// <param name="response">The HTTP response to capture.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task CaptureResponse(HttpResponseMessage response)
    {
        _context.LastResponse = response;
        _context.LastResponseBody = await response.Content.ReadAsStringAsync();
    }
}