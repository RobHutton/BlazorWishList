using BlazorWishList.Data;
using BlazorWishList.Domain.Dtos;
using BlazorWishList.Domain.Entities;
using BlazorWishList.Domain.Models;
using BlazorWishList.Domain.Models.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;

namespace BlazorWishList.Service
{
    public class WishListService(IWishListRepository wishListRepository, ILogger<WishListService> logger) : IWishListService
    {
        private readonly IWishListRepository _wishListRepository = wishListRepository;
        private readonly ILogger<WishListService> _logger = logger;
        public async Task<Result<List<WishListResponse>>> GetWishListItemsAsync(string userId, string wisherId, bool isPurchased, bool isActive)
        {
            try
            {
                var result = await _wishListRepository.GetWishListItemsAsync(userId, wisherId, isPurchased, isActive);
                var response = result.Adapt<List<WishListResponse>>();
                return Result<List<WishListResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.GetWishListItems);
                return Result<List<WishListResponse>>.Fail($"{Errors.GetWishListItems}: {ex.Message}");
            }
        }
        public async Task<Result<WishListResponse>> GetWishListItemAsync(string wisherId, int itemId)
        {
            try
            {
                var result = await _wishListRepository.GetWishListItemAsync(wisherId, itemId);
                var response = result.Adapt<WishListResponse>();
                return Result<WishListResponse>.Success(response);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.WishListItemNotFound);
                return Result<WishListResponse>.Fail($"{Errors.WishListItemNotFound}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.GetWishListItem);
                return Result<WishListResponse>.Fail($"{Errors.GetWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<WishListResponse>> CreateWishListItemAsync(string wisherId, WishListRequest createItem)
        {
            try
            {
                var newEntry = createItem.Adapt<WishListItem>();
                var result = await _wishListRepository.CreateWishListItemAsync(wisherId, newEntry);
                var response = result.Adapt<WishListResponse>();
                return Result<WishListResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.CreateWishListItem);
                return Result<WishListResponse>.Fail($"{Errors.CreateWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<WishListResponse>> UpdateWishListItemAsync(string wisherId, int itemId, WishListRequest updateItem)
        {
            try
            {
                var updateEntry = updateItem.Adapt<WishListItem>();
                var result = await _wishListRepository.UpdateWishListItemAsync(wisherId, itemId, updateEntry);
                var response = result.Adapt<WishListResponse>();
                return Result<WishListResponse>.Success(response);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.WishListItemNotFound);
                return Result<WishListResponse>.Fail($"{Errors.WishListItemNotFound}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.UpdateWishListItem);
                return Result<WishListResponse>.Fail($"{Errors.UpdateWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<WishListResponse>> PurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId)
        {
            try
            {
                var result = await _wishListRepository.PurchaseWishListItemAsync(wisherId, itemId, purchaserId);
                var response = result.Adapt<WishListResponse>();
                return Result<WishListResponse>.Success(response);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.WishListItemNotFound);
                return Result<WishListResponse>.Fail($"{Errors.WishListItemNotFound}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.PurchaseWishListItem);
                return Result<WishListResponse>.Fail($"{Errors.PurchaseWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<WishListResponse>> UnPurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId)
        {
            try
            {
                var result = await _wishListRepository.UnPurchaseWishListItemAsync(wisherId, itemId, purchaserId);
                var response = result.Adapt<WishListResponse>();
                return Result<WishListResponse>.Success(response);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.WishListItemNotFound);
                return Result<WishListResponse>.Fail($"{Errors.WishListItemNotFound}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.UnPurchaseWishListItem);
                return Result<WishListResponse>.Fail($"{Errors.UnPurchaseWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<bool>> DeleteWishListItemAsync(string wisherId, int itemId)
        {
            try
            {
                var isSuccess = await _wishListRepository.DeleteWishListItemAsync(wisherId, itemId);
                if (!isSuccess)
                {
                    throw new Exception(Errors.DeleteWishListItem);
                }
                return Result<bool>.Success(isSuccess);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.WishListItemNotFound);
                return Result<bool>.Fail($"{Errors.WishListItemNotFound}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.DeleteWishListItem);
                return Result<bool>.Fail($"{Errors.DeleteWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<bool>> UnDeleteWishListItemAsync(string wisherId, int itemId)
        {
            try
            {
                var isSuccess = await _wishListRepository.UnDeleteWishListItemAsync(wisherId, itemId);
                if (!isSuccess)
                {
                    throw new Exception(Errors.UnDeleteWishListItem);
                }
                return Result<bool>.Success(isSuccess);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.WishListItemNotFound);
                return Result<bool>.Fail($"{Errors.WishListItemNotFound}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.UnDeleteWishListItem);
                return Result<bool>.Fail($"{Errors.UnDeleteWishListItem}: {ex.Message}");
            }
        }
        public async Task<Result<List<UserWishListCount>>> GetUsersWithWishListInfoAsync()
        {
            try
            {
                var result = await _wishListRepository.GetUsersWithWishListInfoAsync() ?? [];
                return Result<List<UserWishListCount>>.Success(result);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "{error}", Errors.GetUserWishListCount);
                return Result<List<UserWishListCount>>.Fail($"{Errors.GetUserWishListCount}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{error}", Errors.GetUserWishListCount);
                return Result<List<UserWishListCount>>.Fail($"{Errors.GetUserWishListCount}: {ex.Message}");
            }
        }
    }
}
