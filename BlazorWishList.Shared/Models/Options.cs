namespace BlazorWishList.Domain.Models;

public class Options
{
    public static readonly string[] Sizes = { "SM", "MD", "LG", "XL", "2XL", "3XL" };
    public static readonly string[] Colors =
    {
        "Beige",
        "Black",
        "Blue",
        "Brown",
        "Gray",
        "Green",
        "Multi",
        "Navy",
        "Orange",
        "Pink",
        "Purple",
        "Rainbow",
        "Red",
        "Teal",
        "White",
        "Yellow"
    };
    public static readonly (string Description, int Bottom, int Top)[] PriceRanges =
    {
        ("Less than 20", 0, 20),
        ("20-40", 20, 40),
        ("40-60", 40, 60),
        ("60-80", 60, 80),
        ("80-100", 80, 100),
        ("100-150", 100, 150),
        ("150-200", 150, 200),
        ("200+", 200, int.MaxValue) // 'int.MaxValue' to represent a value greater than 200
    };
    public static readonly string[] WishlistCategories =
    {
        "Artwork",
        "Athletic",
        "Automotive",
        "Art & Crafts",
        "Beauty & Personal Care",
        "Books",
        "Clothing",
        "Crafts & DIY",
        "Electronics & Accessories",
        "Furniture",
        "Grocery",
        "Health & Wellness",
        "Home Decor & Lighting",
        "Jewelry",
        "Kitchen & Dining",
        "Music",
        "Office Supplies",
        "Perfume & Fragrance",
        "Pets",
        "Plants & Garden",
        "Purse, Wallet, Keychain",
        "Shoes",
        "Sports & Outdoors",
        "Toys & Games",
        "Under Garments"
    };
}
