namespace BlazorWishList.Domain.Models;

public class Option
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
        "Navy",
        "Orange",
        "Pink",
        "Purple",
        "Red",
        "Teal",
        "White",
        "Yellow"
    };
    public static readonly (string Description, int Bottom, int Top)[] PriceRanges =
    {
        ("Less than 20", 0, 20),
        ("20 - 50", 20, 50),
        ("50 - 100", 50, 100),
        ("100 - 200", 100, 200),
        ("Over 200", 200, int.MaxValue) // 'int.MaxValue' to represent a value greater than 200
    };
    public static readonly string[] WishlistCategories =
    {
        "Automotive",
        "Art & Crafts",
        "Beauty & Personal Care",
        "Books",
        "Clothing",
        "Electronics",
        "Furniture",
        "Grocery",
        "Health & Wellness",
        "Home & Kitchen",
        "Jewelry",
        "Music",
        "Office Supplies",
        "Pets",
        "Sports & Outdoors",
        "Toys & Games"
    };
}
