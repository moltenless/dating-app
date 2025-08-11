using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers;

public class AdminController(
    UserManager<AppUser> userManager,
    IUnitOfWork uow) : BaseApiController()
{
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await userManager.Users.ToListAsync();
        var userList = new List<object>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userList.Add(new
            {
                user.Id,
                user.Email,
                Roles = roles.ToList()
            });
        }

        return Ok(userList);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{userId}")]
    public async Task<ActionResult<IList<string>>> EditRoles(string userId, [FromQuery] string roles)
    {
        if (string.IsNullOrEmpty(roles)) return BadRequest("You must seelect at least one role");

        var selectedRoles = roles.Split(",").ToArray();

        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Could not retrieve user");

        var userRoles = await userManager.GetRolesAsync(user);
        var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded) return BadRequest("Failed to add to roles");
        result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded) return BadRequest("Added new roles but failed to remove from roles");

        return Ok(await userManager.GetRolesAsync(user));
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public async Task<ActionResult> GetPhotosForModeration()
    {
        return Ok(await uow.MemberRepository.GetPhotosForModerationAsync());
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("moderate-photo")]
    public async Task<ActionResult> ModeratePhoto([FromBody] ModeratePhotoDto dto)
    {
        var photo = await uow.MemberRepository.GetPhotoAsync(dto.PhotoId, false);
        if (photo is null) return BadRequest("Cannot find the photo by its id");

        uow.MemberRepository.ModeratePhotoAsync(photo, dto.Approve);

        var member = await uow.MemberRepository.GetMemberForUpdateAsync(photo.MemberId);
        if (dto.Approve && member is not null && member.ImageUrl is null)
        {
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
        }

        if (await uow.Complete()) return Ok();
        return BadRequest("Something went wrong internally");
    }
}
