using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Controllers;

public class AccountController(
    AppDbContext context,
    ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
        RegisterDto registerDto)
    {
        if (await EmailExists(registerDto.Email))
            return BadRequest("Email taken");

        using var hmac = new HMACSHA512();
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

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(
        LoginDto loginDto)
    {
        var unauthorizedMessage = "Invalid email or password";
        var user = await context.Users.SingleOrDefaultAsync(
            u => u.Email!.ToLower() == loginDto.Email.ToLower());
        if (user is null) return Unauthorized(unauthorizedMessage);
        
        return user.ToDto(tokenService);
    }

    private async Task<bool> EmailExists(string email)
        => await context.Users.AnyAsync(u => u.Email!.ToLower() == email.ToLower());
}
