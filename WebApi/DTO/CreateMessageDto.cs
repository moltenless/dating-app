using System;

namespace WebApi.DTO;

public class CreateMessageDto
{
    public required string RecipientId { get; set; }
    public required string Content { get; set; }
}
