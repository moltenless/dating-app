using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}

