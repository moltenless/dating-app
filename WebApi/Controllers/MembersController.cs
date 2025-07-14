using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Entities;
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
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (memberId is null) return BadRequest("Oops - no id found in token");

        var member = await repository.GetMemberByIdAsync(memberId);
        if (member is null) return BadRequest("Could not get member");

        member.DisplayName = dto.DisplayName ?? member.DisplayName;
        member.Description = dto.Description ?? member.Description;
        member.City = dto.City ?? member.City;
        member.Country = dto.Country ?? member.Country;

        // repository.Update(member);

        if (await repository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update member");
    }
}

