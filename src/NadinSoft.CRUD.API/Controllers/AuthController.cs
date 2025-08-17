using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;
using System.Security.Claims;

namespace NadinSoft.CRUD.API.Controllers;

/// <summary>
/// Provides authentication endpoints for registering users, logging in, and retrieving the current user's info.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Sends requests to handlers and mediates communication between application components.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used to dispatch authentication requests.</param>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new application user.
    /// </summary>
    /// <param name="request">The registration request containing email, password, and confirmation password.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success or failure of the registration operation.
    /// </returns>
    [HttpPost("register")]
    public async Task<ApiResponse<object>> Register(RegisterApplicationUserRequest request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token on success.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> containing the JWT token as a <see cref="string"/> when successful;
    /// otherwise, a failure response with an error message.
    /// </returns>
    [HttpPost("login")]
    public async Task<ApiResponse<string>> Login(LoginApplicationUserRequest request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// Gets the identifier of the currently authenticated user.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the current user's ID, or an error if unavailable.
    /// </returns>
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMyInfo()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(userId);
    }
}