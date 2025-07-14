using AutoMapper;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Common.Mappings;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<RegisterApplicationUserRequest, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Products, opt => opt.Ignore());
    }
}