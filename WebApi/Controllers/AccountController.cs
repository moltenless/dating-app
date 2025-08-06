using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Controllers;

public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
        RegisterDto registerDto)
    {
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            Member = new Member
            {
                DisplayName = registerDto.DisplayName,
                Gender = registerDto.Gender,
                City = registerDto.City,
                Country = registerDto.Country,
                DateOfBirth = registerDto.DateOfBirth,
            }
        };
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("identity", error.Description);
            }

            return ValidationProblem();
        }

        return user.ToDto(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(
        LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is null) return Unauthorized("Invalid email or password");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result) return Unauthorized("Invalid password");
        return user.ToDto(tokenService);
    }
}
