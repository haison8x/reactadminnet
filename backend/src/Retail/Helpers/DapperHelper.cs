namespace Retail.Helpers;

using System.Collections.Generic;
using System.Dynamic;
using Dapper;
using static Dapper.SqlBuilder;

public static class DapperHelper
{
    public static SqlBuilder Where(this SqlBuilder builder, Dictionary<string, object> filter)
    {
        foreach (var keyValuePair in filter)
        {
            var key = keyValuePair.Key;

            var parameters = ToDynamicParameters(keyValuePair);

            if (key == "id")
            {
                builder.Where($"`{key}` IN @{key}", parameters);
            }
            else if (key == "groups")
            {
                builder.Where("`groups` LIKE @groups", new { groups = $"%{keyValuePair.Value}%" });
            }
            else if (key.EndsWith("_gte"))
            {
                builder.Where($"`{key.Replace("_gte", string.Empty)}` >= @{key}", parameters);
            }
            else if (key.EndsWith("_gt"))
            {
                builder.Where($"`{key.Replace("_gt", string.Empty)}` > @{key}", parameters);
            }
            else if (key.EndsWith("_lte"))
            {
                builder.Where($"`{key.Replace("_lte", string.Empty)}` <= @{key}", parameters);
            }
            else if (key.EndsWith("_lt"))
            {
                builder.Where($"`{key.Replace("_lt", string.Empty)}` < @{key}", parameters);
            }
            else
            {
                builder.Where($"`{key}` = @{key}", parameters);
            }
        }

        return builder;
    }

    private static dynamic ToDynamicParameters(KeyValuePair<string, object> keyValuePair)
    {
        var expandoObject = new ExpandoObject();
        var tempKeyValuePair = (ICollection<KeyValuePair<string, object>>)expandoObject;
        tempKeyValuePair.Add(keyValuePair);

        dynamic parameters = expandoObject;

        return parameters;
    }

    public static SqlBuilder OrderBy(this SqlBuilder builder, string[] sort)
    {
        return builder.OrderBy(sort is { Length: 2 } ? $"{sort[0]} {sort[1]}" : $"id ASC");
    }

    public static Template AddQueryTemplate(this SqlBuilder builder, string sql, int[] limit)
    {
        var start = limit is { Length: 2 } ? limit[0] : 0;
        var count = limit is { Length: 2 } ? limit[1] - limit[0] + 1 : int.MaxValue;

        return builder.AddTemplate(sql, new { start, count });
    }
}
