using BlazorWishList.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorWishList.Data.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>  // Inherit from IdentityDbContext for IdentityUser
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for the entities you want to manage in the database
    public DbSet<WishListItem> WishListItems { get; set; }

    // Override OnModelCreating to configure relationships, constraints, etc.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);  // Call the base method to configure Identity
        modelBuilder.Entity<WishListItem>()
                .HasIndex(t => new { t.IsDeleted, t.RecipientId })
                .HasDatabaseName("IX_WishListItem_IsDeleted_RecipientId");

        // Configure the relationship between WishListItem and ApplicationUser
        modelBuilder.Entity<WishListItem>()
            .HasOne(w => w.Recipient)  // One WishListItem has one Recipient
            .WithMany()  // One ApplicationUser can have many WishListItems
            .HasForeignKey(w => w.RecipientId)  // The foreign key for Recipient
            .IsRequired();  // Ensure that the Recipient is always set

        // Configure the relationship for the Gifter (optional)
        modelBuilder.Entity<WishListItem>()
            .HasOne(w => w.Gifter)  // One WishListItem has one Gifter
            .WithMany()  // One ApplicationUser can have many Gifted items
            .HasForeignKey(w => w.GifterId)  // The foreign key for Gifter
            .OnDelete(DeleteBehavior.SetNull);  // Optional: Set to null if Gifter is deleted
    }
}

