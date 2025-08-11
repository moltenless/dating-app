using System;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams);
    Task<Member?> GetMemberByIdAsync(string id);
    Task<Member?> GetMemberForUpdateAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string requestingMemberId, string memberId);
    Task<IReadOnlyList<Photo>> GetPhotosForModerationAsync();
    void ModeratePhotoAsync(Photo photo, bool approve);
    Task<Photo?> GetPhotoAsync(int photoId, bool onlyApproved);
}
