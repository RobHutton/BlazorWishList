using System.ComponentModel.DataAnnotations;

namespace BlazorWishList.Domain.Entities;

public class WishListItem : BaseEntity
{
    [MaxLength(500)]
    public required string ItemDescription { get; set; }
    [MaxLength(500)]
    public required string ItemUrl { get; set; }
    [MaxLength(255)]
    public string Merchant { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Category { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Size { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Color { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    [MaxLength(50)]
    public string PriceRange { get; set; } = string.Empty;
    public bool IsPurchased { get; set; }
    // Foreign Key to ApplicationUser (Recipient)
    [Required]  // This enforces that the RecipientId must be provided
    public string RecipientId { get; set; } = string.Empty;

    // Navigation property to the recipient (ApplicationUser)
    public virtual ApplicationUser Recipient { get; set; } = null!;

    // Foreign Key to ApplicationUser (Gifter)
    public string GifterId { get; set; } = string.Empty;

    // Navigation property to the recipient (ApplicationUser)
    public virtual ApplicationUser? Gifter { get; set; } = null!;
}
