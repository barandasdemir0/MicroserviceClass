using Microsoft.EntityFrameworkCore;
using Todos.Categories.WebAPI.Models;

namespace Todos.Categories.WebAPI.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }

}
