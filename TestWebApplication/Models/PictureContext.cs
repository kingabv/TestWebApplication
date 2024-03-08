using Microsoft.EntityFrameworkCore;

namespace TestWebApplication.Models;

public class PictureContext : DbContext
{
    public PictureContext(DbContextOptions<PictureContext> options)
        : base(options)
    {
    }

     public DbSet<PictureItem> PictureItems { get; set; } = null!;
}