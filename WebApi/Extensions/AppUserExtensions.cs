using System;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Extensions;

public static class AppUserExtensions
{
    public static UserDto ToDto(
        this AppUser user,
        ITokenService tokenService)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            ImageUrl = user.ImageUrl,
            DisplayName = user.DisplayName,
            Token = tokenService.CreateToken(user),
        };
    }
}
