namespace BlazorWishList.Client.Helpers
{
    public static class QueryHelper
    {
        public static string GetQueryValueFromUri(string fullUri, string key)
        {
            Uri uri = new Uri(fullUri);
            string queryString = uri.Query;
            return GetQueryValue(queryString, key);
        }

        public static int GetQueryValueFromUri(string fullUri, string key, int defaultValue = 0)
        {
            Uri uri = new Uri(fullUri);
            string queryString = uri.Query;
            string? value = GetQueryValue(queryString, key);

            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        public static string GetQueryValue(string queryString, string key)
        {
            Dictionary<string, string> parameters = ParseQuery(queryString);

            if (parameters.TryGetValue(key, out string? value) && value != null)
                return value;

            return string.Empty;
        }

        private static Dictionary<string, string> ParseQuery(string queryString)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(queryString))
                return result;

            // Remove leading "?" if present
            if (queryString.StartsWith("?"))
                queryString = queryString.Substring(1);

            string[] pairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('=', 2); // split into 2 parts only
                if (parts.Length == 2)
                {
                    string key = Uri.UnescapeDataString(parts[0]);
                    string value = Uri.UnescapeDataString(parts[1]);
                    result[key] = value;
                }
            }

            return result;
        }
    }
}
