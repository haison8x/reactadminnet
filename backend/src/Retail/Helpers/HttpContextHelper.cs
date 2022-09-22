namespace Retail.Helpers;

using Microsoft.AspNetCore.Http;

public static class HttpContextHelper
{
    public static void IncludeContentRange(this HttpContext context, string content, int[] range, int totalCount)
    {
        if (range == null || range.Length < 2)
        {
            return;
        }

        var headerValue = $"{content} {range[0]}-{range[1]}/{totalCount}";
        context?.Response?.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
        context?.Response?.Headers.Add("Content-Range", headerValue);
    }

}