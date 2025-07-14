using System;
using WebApi.Entities;

namespace WebApi.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberByIdAsync(string id);
    Task<Member?> GetMemberForUpdateAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId); 
}
