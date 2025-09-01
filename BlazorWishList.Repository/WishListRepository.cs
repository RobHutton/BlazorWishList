using BlazorWishList.Data.DbContext;
using BlazorWishList.Domain.Dtos;
using BlazorWishList.Domain.Entities;
using BlazorWishList.Domain.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlazorWishList.Data;

public class WishListRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IWishListRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;
    public async Task<List<WishListItem>> GetWishListItemsAsync(string userId, string wisherId, bool isPurchased, bool isActive)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        if (userId == wisherId)
        {
            // Wisher: Show active/inactive items regardless of IsPurchased
            return await dbContext.WishListItems
                .Where(w => w.WisherId == wisherId
                            && w.IsDeleted == !isActive)
                .ToListAsync();
        }
        else
        {
            // Non-wisher: 
            // Show active items filtered by IsPurchased
            // OR inactive items IF purchased by this user
            return await dbContext.WishListItems
                .Where(w => w.WisherId == wisherId &&
                            (
                                (!w.IsDeleted && w.IsPurchased == isPurchased) ||   // active, filtered
                                (w.IsDeleted && w.IsPurchased && w.PurchaserId == userId) // inactive but purchased by user
                            ))
                .ToListAsync();
        }
    }
    public async Task<WishListItem> GetWishListItemAsync(string wisherId, int itemId)
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        List<WishListItem> items = dbContext.WishListItems.Where(e => e.WisherId == wisherId && e.Id == itemId).ToList();
        WishListItem result = new();
        if (items.Count > 0)
        {
            result = items[0];
        }
        return result;
        //return await dbContext.WishListItems
        //    .FirstOrDefaultAsync(w => w.WisherId == wisherId && w.Id == itemId && !w.IsDeleted)
        //    ?? throw new EntityNotFoundException(Errors.GetWishListItem);
    }
    public async Task<WishListItem> CreateWishListItemAsync(string wisherId, WishListItem createItem)
    {
        createItem.WisherId = wisherId;
        await using var dbContext = _contextFactory.CreateDbContext();
        dbContext.WishListItems.Add(createItem);
        await dbContext.SaveChangesAsync();
        return createItem;
    }
    public async Task<WishListItem> UpdateWishListItemAsync(string wisherId, int itemId, WishListItem item)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        var updateItem = await dbContext.WishListItems
            .FirstOrDefaultAsync(x => x.WisherId == wisherId && x.Id == itemId)
            ?? throw new EntityNotFoundException($"WishList Item with ID {itemId} not found.");

        updateItem.Category = item.Category;
        updateItem.Color = item.Color;
        updateItem.Comments = item.Comments;
        updateItem.Description = item.Description;
        updateItem.Merchant = item.Merchant;
        updateItem.PriceRange = item.PriceRange;
        updateItem.Size = item.Size;
        updateItem.Url = item.Url;
        updateItem.DateUpdated = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return updateItem;
    }
    public async Task<WishListItem> PurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        var purchaseItem = await dbContext.WishListItems
            .FirstOrDefaultAsync(x => x.WisherId == wisherId && x.Id == itemId)
            ?? throw new EntityNotFoundException($"WishList Item with ID {itemId} not found.");

        purchaseItem.DatePurchased = DateTime.Now;
        purchaseItem.PurchaserId = purchaserId;
        purchaseItem.IsPurchased = true;

        await dbContext.SaveChangesAsync();
        return purchaseItem;
    }

    public async Task<WishListItem> UnPurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        var purchaseItem = await dbContext.WishListItems
            .FirstOrDefaultAsync(x => x.WisherId == wisherId && x.Id == itemId)
            ?? throw new EntityNotFoundException($"WishList Item with ID {itemId} not found.");

        purchaseItem.DatePurchased = null;
        purchaseItem.PurchaserId = null;
        purchaseItem.IsPurchased = false;

        await dbContext.SaveChangesAsync();
        return purchaseItem;
    }

    public async Task<bool> DeleteWishListItemAsync(string userId, int itemId)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        var deleteItem = await dbContext.WishListItems
            .FirstOrDefaultAsync(x => x.WisherId == userId && x.Id == itemId)
            ?? throw new EntityNotFoundException($"WishList Item with ID {itemId} not found.");

        deleteItem.IsDeleted = true;
        deleteItem.DateDeleted = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnDeleteWishListItemAsync(string userId, int itemId)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        var deleteItem = await dbContext.WishListItems
            .FirstOrDefaultAsync(x => x.WisherId == userId && x.Id == itemId)
            ?? throw new EntityNotFoundException($"WishList Item with ID {itemId} not found.");

        deleteItem.IsDeleted = false;
        deleteItem.DateDeleted = null;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserWishListCount>> GetUsersWithWishListInfoAsync()
    {
        await using var dbContext = _contextFactory.CreateDbContext();
        var result = await dbContext.Users
            .Select(u => new UserWishListCount
            {
                Id = u.Id,
                UserName = u.UserName ?? "",
                Email = u.Email ?? "",
                Birthday = u.DateOfBirth.ToString("MMM dd"),
                ActiveWishListItemCount = u.WishListItems.Count(item => !item.IsDeleted),
                Sort = u.Sort,
                Photo = u.Photo
            })
            .Where(e => !string.IsNullOrEmpty(e.Email))
            .ToListAsync();
        return result.OrderByDescending(e => e.ActiveWishListItemCount).ThenBy(e => e.Sort).ToList();
    }
}
