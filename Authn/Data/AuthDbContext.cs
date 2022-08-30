using Microsoft.EntityFrameworkCore;

namespace Authn.Data
{
    public class AuthDbContext:DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId);
                entity.Property(e => e.Provider).HasMaxLength(250);
                entity.Property(e => e.NameIdentifier).HasMaxLength(250);
                entity.Property(e => e.Username).HasMaxLength(250);
                entity.Property(e => e.Password).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Firstname).HasMaxLength(250);
                entity.Property(e => e.Lastname).HasMaxLength(250);
                entity.Property(e => e.Mobile).HasMaxLength(250);
                entity.Property(e => e.Roles).HasMaxLength(250);

                entity.HasData(new AppUser
                {
                    Provider = "Cookies",
                    UserId = 1,
                    Email = "walbhota@gmail.com",
                    Username = "walbhota@gmail.com",
                    Password = "bigman",
                    Firstname = "Walter",
                    Lastname = "Ebhota",
                    Mobile = "08113867034",
                    Roles = "Admin",
                    NameIdentifier = ""
                });

            });
        }
    }
}
