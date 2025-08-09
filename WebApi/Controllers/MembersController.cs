using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Controllers;

[Authorize]
public class MembersController(
    IUnitOfWork uow,
    IPhotoService photoService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers(
        [FromQuery] MemberParams memberParams)
    {
        memberParams.CurrentMemberId = User.GetMemberId();
        return Ok(await uow.MemberRepository.GetMembersAsync(memberParams));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetMember(string id)
    {
        var member = await uow.MemberRepository.GetMemberByIdAsync(id);
        if (member == null) return NotFound();
        return member;
    }

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(
        string id)
    {
        return Ok(await uow.MemberRepository.GetPhotosForMemberAsync(User.GetMemberId(), id));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateMember(MemberUpdateDto dto)
    {
        var memberId = User.GetMemberId();
        var member = await uow.MemberRepository.GetMemberForUpdateAsync(memberId);
        if (member is null) return BadRequest("Could not get member");

        member.DisplayName = dto.DisplayName ?? member.DisplayName;
        member.Description = dto.Description ?? member.Description;
        member.City = dto.City ?? member.City;
        member.Country = dto.Country ?? member.Country;

        member.User.DisplayName = dto.DisplayName ?? member.User.DisplayName;

        // uow.MemberRepository.Update(member); //optional

        if (await uow.Complete()) return NoContent();
        return BadRequest("Failed to update member");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> AddPhotoAsync([FromForm] IFormFile file)
    {
        var member = await uow.MemberRepository.GetMemberForUpdateAsync(User.GetMemberId());
        if (member == null) return BadRequest("Cannot update member");

        var result = await photoService.UploadPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            MemberId = User.GetMemberId(),
            Approved = false
        };

        member.Photos.Add(photo);

        if (await uow.Complete()) return photo;
        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhotoAsync(int photoId)
    {
        var member = await uow.MemberRepository.GetMemberForUpdateAsync(User.GetMemberId());
        if (member == null) return BadRequest("Cannot get member from token");

        var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);
        if (member.ImageUrl == photo?.Url || photo == null || !photo.Approved)
        {
            return BadRequest("Cannot set this as main image");
        }

        member.ImageUrl = photo.Url;
        member.User.ImageUrl = photo.Url;

        if (await uow.Complete()) return NoContent();
        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var member = await uow.MemberRepository.GetMemberForUpdateAsync(User.GetMemberId());
        if (member == null) return BadRequest("Cannot get member from token");

        var photo = await uow.MemberRepository.GetPhotoAsync(photoId, false);
        if (photo == null || photo.MemberId != member.Id)
            return BadRequest("This photo cannot be found by its id or it doesn't belong to the member");
        if(photo.Url == member.ImageUrl)
            return BadRequest("This photo is avatar and cannot be deleted");
        
        if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

        member.Photos.Remove(photo);
        if (await uow.Complete()) return Ok();
        return BadRequest("Problem deleting the photo");
    }
}

