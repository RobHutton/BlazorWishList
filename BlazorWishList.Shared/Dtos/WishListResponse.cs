namespace BlazorWishList.Domain.Dtos;

public record struct WishListResponse
(
    int Id,
    string Url,
    string Description,
    string Merchant,
    string Category,
    string Size,
    string Color,
    string PriceRange,
    bool IsPurchased,
    DateTime? DatePurchased,
    bool IsDeleted,
    string WisherId,
    string PurchaserId
);
