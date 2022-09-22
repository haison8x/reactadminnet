namespace Retail.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Queries;

public static class QueryHelper
{
    public static GetListQuery ToGetListQuery(string filter, string range, string sort)
    {
        return new GetListQuery
        {
            Filter = ToFilter(filter),
            Range = ToRange(range),
            Sort = ToSort(sort)
        };
    }

    private static Dictionary<string, object> ToFilter(string value)
    {
        return ToDictionary(value);
    }

    private static int[] ToRange(string value)
    {
        TryParseJson(value, out int[] range);

        if (range == null || range.Length < 2)
        {
            return Array.Empty<int>();
        }

        return range;
    }

    private static string[] ToSort(string value)
    {
        TryParseJson(value, out string[] sort);

        if (sort == null)
        {
            return Array.Empty<string>();
        }

        return sort;
    }

    private static Dictionary<string, object> ToDictionary(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return new Dictionary<string, object>();
        }

        return JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
    }

    public static int[] ToIdFilters(string filter)
    {
        var dictionary = ToDictionary(filter);
        if (!dictionary.ContainsKey("id") || dictionary["id"] is not JArray)
        {
            return Array.Empty<int>();
        }

        return (dictionary["id"] as JArray)?.ToObject<int[]>();
    }

    private static bool TryParseJson<T>(string value, out T result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default(T);
            return false;
        }

        var success = true;
        var settings = new JsonSerializerSettings
        {
            Error = (_, args) => { success = false; args.ErrorContext.Handled = true; },
            MissingMemberHandling = MissingMemberHandling.Error
        };
        result = JsonConvert.DeserializeObject<T>(value, settings);

        return success;
    }
}