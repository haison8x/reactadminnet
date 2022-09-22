namespace Retail.Queries;

public class GetListQuery
{
    public int[] Range { get; set; }

    public string[] Sort { get; set; }

    public Dictionary<string, object> Filter { get; set; }
}
