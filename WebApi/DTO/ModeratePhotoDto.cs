using System;

namespace WebApi.DTO;

public class ModeratePhotoDto
{
    public required int PhotoId { get; set; }
    public required bool Approve { get; set; }
}
