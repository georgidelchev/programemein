using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Programemein.Data.Entities;

namespace Programemein.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ImageData> Images { get; set; }

        public DbSet<Meme> Memes { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
