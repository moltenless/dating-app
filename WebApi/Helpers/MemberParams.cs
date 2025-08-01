using System;

namespace WebApi.Helpers;

public class MemberParams : PagingParams
{
    public string? Gender { get; set; }
    public string? CurrentMemberId { get; set; }
    
}
