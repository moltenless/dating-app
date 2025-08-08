using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities;

public class Group(string name)
{
    [Key]
    public string Name { get; set; } = name;
}
