using BlazorWishList.Domain.Dtos;
using BlazorWishList.Domain.Models;

namespace BlazorWishList.Service;

public interface IWishListService
{
    public Task<Result<List<WishListResponse>>> GetWishListItemsAsync(string userId, string wisherId, bool isPurchased, bool isActive);
    public Task<Result<WishListResponse>> GetWishListItemAsync(string wisherId, int itemId);
    public Task<Result<WishListResponse>> CreateWishListItemAsync(string wisherId, WishListRequest createItem);
    public Task<Result<WishListResponse>> UpdateWishListItemAsync(string wisherId, int itemId, WishListRequest updateItem);
    public Task<Result<WishListResponse>> PurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId);
    public Task<Result<WishListResponse>> UnPurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId);
    public Task<Result<bool>> DeleteWishListItemAsync(string wisherId, int itemId);
    public Task<Result<bool>> UnDeleteWishListItemAsync(string wisherId, int itemId);
    public Task<Result<List<UserWishListCount>>> GetUsersWithWishListInfoAsync();
}
