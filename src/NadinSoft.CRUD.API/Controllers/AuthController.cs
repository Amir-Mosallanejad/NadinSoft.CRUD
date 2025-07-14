using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public AuthController(UserManager<ApplicationUser> userManager, IMediator mediator, IConfiguration config)
    {
        _userManager = userManager;
        _mediator = mediator;
        _config = config;
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