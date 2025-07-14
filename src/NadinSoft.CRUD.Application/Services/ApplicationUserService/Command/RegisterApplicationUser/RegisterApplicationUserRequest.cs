using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

public record RegisterApplicationUserRequest(string Email, string Password, string ConfirmPassword)
    : IRequest<ApiResponse<object>>;