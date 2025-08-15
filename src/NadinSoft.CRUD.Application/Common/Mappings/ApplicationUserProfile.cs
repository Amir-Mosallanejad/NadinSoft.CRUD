// <copyright file="ApplicationUserProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Common.Mappings;

using AutoMapper;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Defines AutoMapper mapping configuration for <see cref="ApplicationUser"/> entities.
/// </summary>
public class ApplicationUserProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationUserProfile"/> class.
    /// Configures the mapping between <see cref="RegisterApplicationUserRequest"/> and <see cref="ApplicationUser"/>.
    /// </summary>
    public ApplicationUserProfile()
    {
        this.CreateMap<RegisterApplicationUserRequest, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Products, opt => opt.Ignore());
    }
}