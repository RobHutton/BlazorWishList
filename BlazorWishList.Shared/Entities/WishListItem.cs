namespace BlazorWishList.Domain.Entities;

public class WishListItem : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Merchant { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? Comments { get; set; }
    public string PriceRange { get; set; } = string.Empty;
    public bool IsPurchased { get; set; } = false;
    public DateTime? DatePurchased { get; set; }
    public string WisherId { get; set; } = string.Empty;
    public ApplicationUser? Wisher { get; set; }
    public string? PurchaserId { get; set; } = null!;
    public ApplicationUser? Purchaser { get; set; }
    public int Sort { get; set; } = 0;
}
