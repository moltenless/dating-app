using System;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public void Update(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }


    public async Task<Member?> GetMemberForUpdateAsync(string id) {
        return await context.Members
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync()
    {
        return await context.Members.ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        return await context.Members
            .Where(x => x.Id == memberId)
            .SelectMany(x => x.Photos)
            .ToListAsync();
    }
}
