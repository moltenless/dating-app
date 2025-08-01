using System;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams);
    Task<Member?> GetMemberByIdAsync(string id);
    Task<Member?> GetMemberForUpdateAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId); 
}
