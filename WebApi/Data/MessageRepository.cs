using System;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Data;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(string messageId)
    {
        return await context.Messages.FindAsync(messageId);
    }

    public Task<PaginatedResult<MessageDto>> GetMessagesForMember()
    {
        throw new Exception();
    }
    
    public Task<IReadOnlyList<MessageDto>> GetMessageThread(
        string currentMemberId, string recipientId)
    {
        throw new Exception();
    }

    public async Task<bool> SaveAllChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
