using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

public record LoginApplicationUserRequest(string Email, string Password) : IRequest<ApiResponse<string>>;