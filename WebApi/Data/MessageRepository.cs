using System;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Data;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    public void AddGroup(Group group)
    {
        
    }

    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public Task<Connection?> GetConnection(string connectionId) => throw new NotImplementedException();
    public Task<Group?> GetGroupForConnection(string connectionId) => throw new NotImplementedException();

    public async Task<Message?> GetMessage(string messageId)
    {
        return await context.Messages.FindAsync(messageId);
    }

    public Task<Group?> GetMessageGroup(string groupName) => throw new NotImplementedException();

    public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(
        MessageParams messageParams)
    {
        var query = context.Messages
            .OrderByDescending(x => x.MessageSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Outbox" => query.Where(x => x.SenderId == messageParams.MemberId 
                && !x.SenderDeleted),
            _ => query.Where(x => x.RecipientId == messageParams.MemberId
                && !x.RecipientDeleted)
        };

        var messagesQuery = query.Select(MessageExtensions.ToDtoProjection());

        return await PaginationHelper.CreateAsync(messagesQuery,
             messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IReadOnlyList<MessageDto>> GetMessageThread(
        string currentMemberId, string recipientId)
    {
        await context.Messages
            .Where(x => x.RecipientId == currentMemberId
                && x.SenderId == recipientId
                && x.DateRead == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.DateRead, DateTime.UtcNow));

        return await context.Messages
            .Where(x => (x.RecipientId == currentMemberId && !x.RecipientDeleted
                && x.SenderId == recipientId)
                    || (x.SenderId == currentMemberId && !x.SenderDeleted
                && x.RecipientId == recipientId))
            .OrderBy(x => x.MessageSent)
            .Select(MessageExtensions.ToDtoProjection())
            .ToListAsync();
    }

    public Task RemoveConnection(string connectionId) => throw new NotImplementedException();

    public async Task<bool> SaveAllChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
