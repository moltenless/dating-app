using System;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public void Update(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }


    public async Task<Member?> GetMemberForUpdateAsync(string id)
    {
        return await context.Members
            .Include(x => x.User)
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams)
    {
        var query = context.Members.AsQueryable();

        query = query.Where(x => x.Id != memberParams.CurrentMemberId);
        if (memberParams.Gender != null)
            query = query.Where(x => x.Gender == memberParams.Gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MinAge));
        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = memberParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };

        return await PaginationHelper.CreateAsync(
                        query,
                        memberParams.PageNumber,
                        memberParams.PageSize);
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string requestingMemberId, string memberId)
    {
        if (requestingMemberId == memberId)
            return await context.Members
                .IgnoreQueryFilters() // Show your not approved photos to yourself
                .Where(x => x.Id == memberId)
                .SelectMany(x => x.Photos)
                .ToListAsync();
        else
            return await context.Members
                .Where(x => x.Id == memberId)
                .SelectMany(x => x.Photos)
                .ToListAsync();
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForModerationAsync()
    {
        return await context.Photos
            .IgnoreQueryFilters()
            .Where(p => !p.Approved)
            .ToListAsync();
    }

    public void ModeratePhotoAsync(Photo photo, bool approve)
    {
        if (approve)
        {
            photo.Approved = true;
        }
        else
        {
            context.Photos.Remove(photo);
        }
    }

    public async Task<Photo?> GetPhotoAsync(int photoId, bool onlyApproved)
    {
        if (onlyApproved)
            return await context.Photos.FindAsync(photoId);
        else
            return await context.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == photoId);
    }
}
