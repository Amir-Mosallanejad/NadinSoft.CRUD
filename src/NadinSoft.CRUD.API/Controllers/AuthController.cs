using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

namespace NadinSoft.CRUD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<object>> Register(RegisterApplicationUserRequest request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("login")]
    public async Task<ApiResponse<string>> Login(LoginApplicationUserRequest request)
    {
        return await _mediator.Send(request);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMyInfo()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(userId);
    }
}