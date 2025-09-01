using System.Text.Json;

namespace BlazorWishList.Client.Helpers;

public static class JsonHelper
{
    public static string ToJson<T>(T data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    public static T FromJson<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default!;

        return JsonSerializer.Deserialize<T>(json) ?? default!;
    }

    public static T Clone<T>(T data)
    {
        // Serialize to JSON and then deserialize to create a deep copy
        if (data == null) return default!;
        string json = ToJson(data);
        return FromJson<T>(json)!;
    }

    public static bool IsObjectMatch<T>(T? obj1, T? obj2)
    {
        if (obj1 == null && obj2 == null)
            return true;

        if (obj1 == null || obj2 == null)
            return false;

        string a = ToJson(obj1);
        string b = ToJson(obj2);
        return string.Equals(a, b, StringComparison.Ordinal);
    }
}
