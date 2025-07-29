using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Controllers;

[Authorize]
public class MembersController(IMemberRepository repository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        => Ok(await repository.GetMembersAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetMember(string id)
    {
        var member = await repository.GetMemberByIdAsync(id);
        if (member == null) return NotFound();
        return member;
    }

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(
        string id)
    {
        return Ok(await repository.GetPhotosForMemberAsync(id));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateMember(MemberUpdateDto dto)
    {
        var memberId = User.GetMemberId();
        var member = await repository.GetMemberForUpdateAsync(memberId);
        if (member is null) return BadRequest("Could not get member");

        member.DisplayName = dto.DisplayName ?? member.DisplayName;
        member.Description = dto.Description ?? member.Description;
        member.City = dto.City ?? member.City;
        member.Country = dto.Country ?? member.Country;

        member.User.DisplayName = dto.DisplayName ?? member.User.DisplayName;

        // repository.Update(member); //optional

        if (await repository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update member");
    }
}

