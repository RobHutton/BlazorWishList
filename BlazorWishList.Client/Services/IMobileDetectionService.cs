namespace BlazorWishList.Client.Services;

public interface IMobileDetectionService
{
    bool IsMobileDevice(bool overrideValue = false);
}
