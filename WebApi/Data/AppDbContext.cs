using System;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    
}
