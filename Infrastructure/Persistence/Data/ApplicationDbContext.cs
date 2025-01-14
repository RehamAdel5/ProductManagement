using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace External.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IMemoryCache _cache;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IMemoryCache cache)
             : base(options)
        {
            _cache = cache;
        } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

    }
     
}
