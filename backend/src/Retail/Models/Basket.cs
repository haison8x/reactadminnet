namespace Retail.Models;

public class Basket
{
    public int id { get; set; }

    public int command_id { get; set; }
    public int product_id { get; set; }
    public int quantity { get; set; }
}
