using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;

namespace WebApi.Controllers;

[Authorize]
public class MembersController(
    IMemberRepository repository,
    IPhotoService photoService) : BaseApiController
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

    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> AddPhotoAsync([FromForm] IFormFile file)
    {
        var member = await repository.GetMemberForUpdateAsync(User.GetMemberId());
        if (member == null) return BadRequest("Cannot update member");

        var result = await photoService.UploadPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            MemberId = User.GetMemberId()
        };

        if (member.ImageUrl == null)
        {
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
        }
        member.Photos.Add(photo);

        if (await repository.SaveAllAsync()) return photo;
        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhotoAsync(int photoId)
    {
        var member = await repository.GetMemberForUpdateAsync(User.GetMemberId());
        if (member == null) return BadRequest("Cannot get member from token");

        var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
        if (member.ImageUrl == photo?.Url || photo == null)
        {
            return BadRequest("Cannot set this as main image");
        }

        member.ImageUrl = photo.Url;
        member.User.ImageUrl = photo.Url;

        if (await repository.SaveAllAsync()) return NoContent();
        return BadRequest("Problem setting main photo");
    }
}

