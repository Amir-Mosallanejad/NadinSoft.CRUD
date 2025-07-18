using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}