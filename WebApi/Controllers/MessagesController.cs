using System;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Interfaces;

namespace WebApi.Controllers;

public class MessagesController(
    IMessageRepository messageRepository,
    IMemberRepository memberRepository) : BaseApiController

{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto dto)
    {
        var sender = await memberRepository.GetMemberByIdAsync(User.GetMemberId());
        var recipient = await memberRepository.GetMemberByIdAsync(dto.RecipientId);

        if (sender == null || recipient == null || sender.Id == dto.RecipientId)
            return BadRequest("Cannot send this message");

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = dto.Content
        };

        messageRepository.AddMessage(message);

        if (await messageRepository.SaveAllChangesAsync()) return message.ToDto();

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<MessageDto>>>
        GetMessagesByContainer(
        [FromQuery] MessageParams messageParams
        )
    {
        messageParams.MemberId = User.GetMemberId();
        return await messageRepository.GetMessagesForMember(messageParams);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(
        string recipientId)
    {
        return Ok(await messageRepository.GetMessageThread(User.GetMemberId(), recipientId));
    }
}
