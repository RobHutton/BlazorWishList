using BlazorWishList.Domain.Dtos;
using BlazorWishList.Domain.Entities;

namespace BlazorWishList.Data;

public interface IWishListRepository
{
    public Task<List<WishListItem>> GetWishListItemsAsync(string userId, string wisherId, bool isPurchased, bool isActive);
    public Task<WishListItem> GetWishListItemAsync(string wisherId, int itemId);
    public Task<WishListItem> CreateWishListItemAsync(string wisherId, WishListItem createItem);
    public Task<WishListItem> UpdateWishListItemAsync(string wisherId, int itemId, WishListItem updateItem);
    public Task<WishListItem> PurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId);
    public Task<WishListItem> UnPurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId);
    public Task<bool> DeleteWishListItemAsync(string wisherId, int itemId);
    public Task<bool> UnDeleteWishListItemAsync(string wisherId, int itemId);
    public Task<List<UserWishListCount>> GetUsersWithWishListInfoAsync();
}
