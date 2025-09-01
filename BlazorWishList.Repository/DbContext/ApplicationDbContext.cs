using BlazorWishList.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorWishList.Data.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

    // DbSets for the entities you want to manage in the database
    public DbSet<WishListItem> WishListItems { get; set; }

    // Override OnModelCreating to configure relationships, constraints, etc.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<WishListItem>()
            .HasOne(w => w.Wisher)
            .WithMany(u => u.WishListItems)
            .HasForeignKey(w => w.WisherId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade/SetNull as needed

        modelBuilder.Entity<WishListItem>()
            .HasOne(w => w.Purchaser)
            .WithMany()
            .HasForeignKey(w => w.PurchaserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WishListItem>()
                .HasIndex(t => new { t.IsDeleted, t.WisherId, t.IsPurchased, t.PurchaserId })
                .HasDatabaseName("IX_WishListItem");
    }
}

