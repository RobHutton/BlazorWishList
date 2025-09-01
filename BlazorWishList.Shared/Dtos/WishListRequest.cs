using System.ComponentModel.DataAnnotations;

namespace BlazorWishList.Domain.Dtos;

public class WishListRequest
{
    public WishListRequest() { }
    [Required(ErrorMessage = "Item Link is required.")]
    public required string Url { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(500, ErrorMessage = "Description is limited to 500 characters")]
    public required string Description { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "Merchant is a max of 255 characters")]
    public string Merchant { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "Category is a max of 255 characters")]
    public string Category { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "Size is a max of 255 characters")]
    public string Size { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "Color is a max of 255 characters")]
    public string Color { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Price range is a max of 50 characters")]
    public string PriceRange { get; set; } = string.Empty;

    [Required(ErrorMessage = "Wisher is required.")]
    public string WisherId { get; set; } = string.Empty;
}
