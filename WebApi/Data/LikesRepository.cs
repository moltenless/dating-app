using System;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Data;

public class LikesRepository : ILikesRepository
{
    public void AddLike(MemberLike like)
    {
        
    }
    
    public void DeleteLike(MemberLike like)
    {
        
    }

    public Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        
    }

    public Task<MemberLike> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        
    }

    public Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId)
    {
        
    }

    public Task<bool> SaveAllChanges()
    {
        
    }
}
