using System;

namespace WebApi.Helpers;

public class LikesParams : PagingParams
{
    public string MemberId { get; set; } = string.Empty;
    public string Predicate { get; set; } = "liked";
}
