namespace Retail.Models;

public class Review
{
    public int id { get; set; }
    public DateTime date { get; set; }
    public string status { get; set; }
    public int command_id { get; set; }
    public int product_id { get; set; }
    public int customer_id { get; set; }
    public int rating { get; set; }
    public string comment { get; set; }
}