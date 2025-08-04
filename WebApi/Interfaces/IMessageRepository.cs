using System;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message?> GetMessage(string messageId);
    Task<PaginatedResult<MessageDto>> GetMessagesForMember();
    Task<IReadOnlyList<MessageDto>> GetMessageThread(
        string currentMemberId,
        string recipientId);
    Task<bool> SaveAllChangesAsync();
}
