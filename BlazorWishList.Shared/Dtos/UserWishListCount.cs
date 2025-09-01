namespace BlazorWishList.Domain.Dtos
{
    public class UserWishListCount
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Birthday { get; set; }
        public required string Photo { get; set; }
        public int ActiveWishListItemCount { get; set; }
        public int Sort { get; set; } = 0;
        public string UserDisplay => $"{UserName} ({Birthday})";
    }
}
