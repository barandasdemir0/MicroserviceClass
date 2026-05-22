using Microsoft.EntityFrameworkCore;
using Todos.WebApi.Models;

namespace Todos.WebApi.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Todo> Todos { get; set; }

}
